using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TechTalk.SpecFlow;
using White.Core.Factory;
using White.Core.UIItems;
using White.Core.UIItems.Finders;
using White.Core.UIItems.WindowItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using White.Core.WindowsAPI;
using White.Core.UIItems.TreeItems;

namespace Marcusoft.BDD.WhiteStepHelper
{
    /// <summary>
    /// Base-class for step definitions with White
    /// </summary>
    /// <remarks></remarks>
    public abstract class WhiteStepBase
    {
        private readonly IIdResolver _idResolver;

        private Dictionary<string, IUIItem> _controlDic = new Dictionary<string, IUIItem>();

        /// <summary>
        /// Constructor that takes an IdResolver to use
        /// when resolving wellknown-name to id's
        /// </summary>
        /// <param name="idResolver">the idResolver to use</param>
        /// <remarks></remarks>
        protected WhiteStepBase(IIdResolver idResolver)
        {
            _idResolver = idResolver;
        }

        /// <summary>
        /// The application we're currently testing
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        protected static White.Core.Application ApplicationUnderTest { get; set; }

        /// <summary>
        /// The current active window or dialog in the application
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>It's this dialog that the controls are search for</remarks>
        protected static Window CurrentWindowUnderTest { get; set; }

        /// <summary>
        /// Starts or attach to the application for the given file name
        /// </summary>
        /// <param name="fileName"></param>
        /// <remarks></remarks>
        protected static void AttachOrLaunchInRunningCurrentDirectory(string fileName)
        {
            var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            filePath = Path.Combine(filePath, fileName);
            ApplicationUnderTest = White.Core.Application.AttachOrLaunch(new ProcessStartInfo(filePath));
        }

        /// <summary>
        /// Starts or attach to the application for the given file name
        /// and get hold of the window with the given window title
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="windowTitle"></param>
        /// <remarks></remarks>
        protected static void StartWindowInApplication(string fileName, string windowTitle)
        {
            AttachOrLaunchInRunningCurrentDirectory(fileName);
            SetWindowUnderTest(windowTitle);
        }

        /// <summary>
        /// Sets the window under test to the window with the given title
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <remarks></remarks>
        protected static void SetWindowUnderTest(string windowTitle)
        {
            try
            {
                CurrentWindowUnderTest = ApplicationUnderTest.GetWindow(windowTitle, InitializeOption.WithCache.AndIdentifiedBy(windowTitle));
            }
            catch (Exception ex)
            {
                var message = string.Format("Cannot find a window with the title '{0}', in the application '{1}'", windowTitle, ApplicationUnderTest.Name);
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Returns the dialog with the title
        /// displayed from the CurrentWindowUnderTest
        /// </summary>
        /// <param name="dialogTitle">the title of the dialog</param>
        /// <returns>the dialog window</returns>
        /// <remarks></remarks>
        protected Window GetModalDialogByTitle(string dialogTitle)
        {
            var dialog = CurrentWindowUnderTest.ModalWindow(dialogTitle);

            var message = string.Format("No dialog with title {0} found", dialogTitle);
            Assert.IsNotNull(dialog, message);

            CurrentWindowUnderTest = dialog;
            return dialog;
        }

        /// <summary>
        /// Returns true if the application under test is running
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected static bool IsApplicationUnderTestRunning
        {
            get
            {
                return ApplicationUnderTest != null & ApplicationUnderTest.HasExited == false;
            }
        }

        /// <summary>
        /// Closes the application under test
        /// </summary>
        /// <remarks></remarks>
        protected static void CloseApplicationUnderTest()
        {
            if (IsApplicationUnderTestRunning)
            {
                ApplicationUnderTest.KillAndSaveState();
            }
        }

        /// <summary>
        /// Returns the control with the wellknown name sent to the function
        /// The wellknown name is resolved using the current IdResolver
        /// </summary>
        /// <param name="wellKnownName">wellknown name of control</param>
        /// <returns>the control with the wellknown name</returns>
        /// <remarks></remarks>
        protected IUIItem GetControl(string wellKnownName)
        {
            var id = _idResolver.IdFromWellKnownName(wellKnownName, CurrentWindowUnderTest);
            var c = CurrentWindowUnderTest.Get(SearchCriteria.ByAutomationId(id));

            Assert.IsNotNull(c, string.Format("Could not find a control of with well-known name {0} (translated id: {1})", wellKnownName, id));
            return c;
        }

        /// <summary>
        /// Returns the control with the wellknown name sent to the function
        /// The wellknown name is resolved using the current IdResolver
        /// </summary>
        /// <typeparam name="T">the type of the control to return</typeparam>
        /// <param name="wellKnownName">wellknown name of control</param>
        /// <returns>the control with the wellknown name</returns>
        /// <remarks></remarks>
        protected T GetControl<T>(string wellKnownName) where T : UIItem
        {
            var id = _idResolver.IdFromWellKnownName(wellKnownName, CurrentWindowUnderTest);
            var c = CurrentWindowUnderTest.Get<T>(id);

            Assert.IsNotNull(c, string.Format("Could not find a control of type '{2}' of with well-known name {0} (translated id: {1})", wellKnownName, id, typeof(T).Name));
            return (T)c;
        }
        
        /// <summary>
        /// Returns the node with the given text in the tree
        /// </summary>
        /// <param name="wellKnownTreeName">the wellknown name of the tree</param>
        /// <param name="nodeText">the text of the node</param>
        /// <returns>the node with the given text in the tree</returns>
        /// <remarks></remarks>
        protected TreeNode NodeInTree(string wellKnownTreeName, string nodeText)
        {
            var treeControl = GetControl<Tree>(wellKnownTreeName);
            var node = FindTreeNodeByText(treeControl.Nodes, nodeText);

            Assert.IsNotNull(node, string.Format("Could not find node with text {1} in tree {0}", wellKnownTreeName, nodeText));

            return node;
        }

        /// <summary>
        /// Asserts that the node with given text is not in the given tree
        /// </summary>
        /// <param name="wellKnownTreeName">the wellknown-name of the tree to search</param>
        /// <param name="nodeText">the text of the node</param>
        /// <remarks></remarks>
        protected void Assert_NodeNotInTree(string wellKnownTreeName, string nodeText)
        {
            var tree = GetControl<Tree>(wellKnownTreeName);

            Assert.IsNull(FindTreeNodeByText(tree.Nodes, nodeText));
        }

        /// <summary>
        /// Returns the level where the node is present
        /// </summary>
        /// <param name="wellKnownTreeName">the name of the tree</param>
        /// <param name="nodeText">the text of the node</param>
        /// <returns>the level where the node is present</returns>
        /// <remarks></remarks>
        protected int NodeLevelForNodeInTree(string wellKnownTreeName, string nodeText)
        {
            var tree = GetControl<Tree>(wellKnownTreeName);
            var nodeForText = NodeInTree(wellKnownTreeName, nodeText);

            return tree.GetPathTo(nodeForText).Count;
        }

        /// <summary>
        /// Presses the sent-in specialkey in the current window
        /// </summary>
        /// <param name="keys"></param>
        /// <remarks></remarks>
        protected void PressKey(KeyboardInput.SpecialKeys keys)
        {
            CurrentWindowUnderTest.Keyboard.HoldKey(KeyboardInput.SpecialKeys.RIGHT);
        }

        /// <summary>
        /// Selects the row with the given number in the listview
        /// </summary>
        /// <param name="wellKnownListViewName">the name of the list view</param>
        /// <param name="rowIndex">the row index</param>
        /// <remarks></remarks>
        protected void SelectRowInListView(string wellKnownListViewName, int rowIndex)
        {
            var listView = GetControl<ListView>(wellKnownListViewName);
            var indexToSelect = rowIndex;
            listView.Rows[indexToSelect].Select();
        }

        /// <summary>
        /// Asserts that the selected node in the tree has the sent-in text
        /// </summary>
        /// <param name="wellKnownTreeName">the name of the tree</param>
        /// <param name="expectedTextOfSelectedNode">the expected tree for the selected node</param>
        /// <remarks></remarks>
        protected void Assert_SelectedNodeHasNodeText(string wellKnownTreeName, string expectedTextOfSelectedNode)
        {
            var n = GetControl<Tree>(wellKnownTreeName).SelectedNode;
            Assert.AreEqual(expectedTextOfSelectedNode, n.Text);
        }
        
        /// <summary>
        /// Asserts that the label has the expected text
        /// </summary>
        /// <param name="wellKnownName">the wellknown name of the label</param>
        /// <param name="expectedText">the expected text of the label</param>
        /// <remarks></remarks>
        protected void Assert_LabelTextContains(string wellKnownName, string expectedText)
        {
            var lbl = GetControl<Label>(wellKnownName);
            Assert.AreEqual(expectedText, lbl.Text);
        }

        /// <summary>
        /// Asserts that the tree has the nodes as it's toplevel nodes
        /// </summary>
        /// <param name="wellKnownName">the wellknown name of the tree</param>
        /// <param name="nodeNames">the name of the nodes the tree should have as toplevel nodes</param>
        /// <remarks></remarks>
        protected void Assert_TreeHasTopLevelNodes(string wellKnownName, List<string> nodeNames)
        {
            var tree = GetControl<Tree>(wellKnownName);

            foreach (var nodeName in nodeNames)
            {
                Assert.IsTrue(tree.HasNode(nodeName.Trim()));
            }
        }
        
        /// <summary>
        /// Asserts that the tree has the expected number of nodes
        /// </summary>
        /// <param name="expectedNumberOfNodes">the expected number of nodes</param>
        /// <param name="wellKnownTreeName">the well known name of the tree</param>
        /// <remarks></remarks>
        protected void Assert_NumberOfNodesInTree(int expectedNumberOfNodes, string wellKnownTreeName)
        {
            Assert.AreEqual(expectedNumberOfNodes, GetControl<Tree>(wellKnownTreeName).Nodes.Count);
        }
        
        /// <summary>
        /// Asserts that the control of the given typen and name is present on the 
        /// current window under test
        /// </summary>
        /// <typeparam name="T">the type of the control to check</typeparam>
        /// <param name="wellKnownName">the well known name of the control</param>
        /// <remarks></remarks>
        protected void Assert_ControlOfTypeIsPresentOnWindowUnderTest<T>(string wellKnownName) where T : UIItem
        {
            Assert.IsNotNull(GetControl<T>(wellKnownName));
        }

        /// <summary>
        /// Asserts that the list has the expected number of rows
        /// </summary>
        /// <param name="wellKnownNameOfList">the name of the list</param>
        /// <param name="expectedNumberOfRows">the expected number of rows</param>
        /// <remarks></remarks>
        protected void Assert_NumberOfRowsInList(string wellKnownNameOfList, int expectedNumberOfRows)
        {
            var listView = GetControl<ListView>(wellKnownNameOfList);
            Assert.AreEqual(expectedNumberOfRows, listView.Rows.Count);
        }
        
        /// <summary> 
        /// Asserts that the text for the cell contains the expected text 
        /// </summary> 
        /// <param name="wellKnownListName">the name of the listview</param> 
        /// <param name="rowNumber">the row number to check</param> 
        /// <param name="columnNumber">the column number</param> 
        /// <param name="expectedValue">the expected value</param> 
        /// <remarks></remarks> 
        protected void Assert_CellOnRowInListContainsValue(string wellKnownListName, int rowNumber, int columnNumber, string expectedValue)
        {
            Assert.AreEqual(expectedValue, GetCellText(wellKnownListName, rowNumber, columnNumber, GetControl<ListView>(wellKnownListName)));
        }
        
        /// <summary> 
        /// Asserts that the row with the given number is the selected row 
        /// of the listview with the well known name 
        /// </summary> 
        /// <param name="wellKnownListViewName">the name of the list view</param> 
        /// <param name="rowNumber">the number of the to select</param> 
        /// <remarks></remarks> 
        protected void Assert_RowIsSelectedRow(string wellKnownListViewName, int rowNumber)
        {
            var listView = GetControl<ListView>(wellKnownListViewName);

            var expectedSelectedRow = GetRowByRowNumber(wellKnownListViewName, rowNumber, listView);
            var expectedSelectedCell = GetCellByColumnNumber(wellKnownListViewName, rowNumber, 1, expectedSelectedRow);

            var actualSelectedRows = GetSelectedRows(wellKnownListViewName, listView);
            var actualSelectedCell = GetCellByColumnNumber(wellKnownListViewName, 1, 1, actualSelectedRows[0]);

            Assert.AreEqual(expectedSelectedCell.Text, actualSelectedCell.Text);
        }

        /// <summary> 
        /// Asserts that the row number in the list has the expected data 
        /// </summary> 
        /// <param name="wellKnownListName">the wellknown name of the list</param> 
        /// <param name="rowNumber">the number of the row to check</param> 
        /// <param name="expectedData">the expected data</param> 
        /// <remarks></remarks>
        protected void Assert_RowHasTableData(string wellKnownListName, int rowNumber, Table expectedData)
        {
            var listView = GetControl<ListView>(wellKnownListName);
            var rowFromGUI = listView.Rows[rowNumber - 1];

            foreach (var expectedRowFromStep in expectedData.Rows)
            {
                foreach (var columnName in expectedData.Header)
                {
                    var textInGUI = GetCellInGUIByName(wellKnownListName, rowFromGUI, columnName).Text;
                    var expectedTextFromStep = expectedRowFromStep[columnName];

                    Assert.AreEqual(expectedTextFromStep, textInGUI);
                }
            }
        }

        private static string GetCellText(string wellKnownListName, int rowNumber, int columnNumber, ListView listView)
        {
            var row = GetRowByRowNumber(wellKnownListName, rowNumber, listView);
            var cell = GetCellByColumnNumber(wellKnownListName, rowNumber, columnNumber, row);

            return cell.Text;
        }

        private static ListViewCell GetCellByColumnNumber(string wellKnownListName, int rowNumber, int columnNumber, ListViewRow row)
        {
            var cellmess = string.Format("The column {0} on the row {1} cannot be found in the list '{2}'. That row contains {3} cells", columnNumber, rowNumber, wellKnownListName, row.Cells.Count);
            Assert.IsTrue(columnNumber <= row.Cells.Count, cellmess);

            return row.Cells[columnNumber - 1];
        }

        private static ListViewRow GetRowByRowNumber(string wellKnownListName, int rowNumber, ListView list)
        {
            var rowMess = string.Format("The row {0} cannot be found in the list '{1}', which contains {2} rows", rowNumber, wellKnownListName, list.Rows.Count);
            Assert.IsTrue(rowNumber <= list.Rows.Count, rowMess);

            return list.Rows[rowNumber - 1];
        }

        private static ListViewRows GetSelectedRows(string wellKnownListViewName, ListView listView)
        {
            var selectedMess = string.Format("The list '{0}' doesn't have any selected rows", wellKnownListViewName);
            Assert.IsNotNull(listView.SelectedRows, selectedMess);
            Assert.AreNotEqual(0, listView.SelectedRows.Count, selectedMess);

            return listView.SelectedRows;
        }
        
        private static ListViewCell GetCellInGUIByName(string wellKnownListName, ListViewRow rowInGUI, string columnName)
        {
            var cellFromGUI = rowInGUI.Cells[columnName];
            var message = string.Format("Column '{0}' cannot be found in the list '{1}'", columnName, wellKnownListName);

            Assert.IsNotNull(cellFromGUI, message);
            return cellFromGUI;
        }

        private static TreeNode FindTreeNodeByText(TreeNodes nodesToSearch, string nodeText)
        {
            TreeNode nodeToReturn = null;

            foreach (var node in nodesToSearch)
            {
                if (node.Text == nodeText)
                {
                    nodeToReturn = node;
                    break;
                }

                if ((node.Nodes.Count > 0))
                {
                    nodeToReturn = FindTreeNodeByText(node.Nodes, nodeText);
                }
            }

            return nodeToReturn;
        }

    }
}
