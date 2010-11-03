using White.Core.UIItems.ListBoxItems;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using White.Core.UIItems;
using White.Core.UIItems.TabItems;
using White.Core.UIItems.TreeItems;

namespace Marcusoft.BDD.WhiteStepHelper
{
    [Binding()]
    public class BaseSteps : WhiteStepBase
    {

        // <summary>
        // Asserts that the application is started
        // </summary>
        // <remarks></remarks>
        [Given(@"that the application is started")]
        [Then(@"the application should be started")]
        public void ApplicationIsStarted()
        {
            Assert.AreEqual(true, SUT.IsRunning());
        }

        // <summary>
        // Asserts that the window under test has the given title
        // </summary>
        // <param name="expectedTitle">the expected title of the window</param>
        // <remarks></remarks>
        [Given(@"that the '(.*)' window is shown")]
        [Then(@"the '(.*)' window is shown")]
        public void WindowUnderTestHaveTitle(string expectedTitle)
        {
            SetWindowUnderTest(expectedTitle);
            Assert.IsTrue(WindowUnderTest.Visible);
            Assert.AreEqual(expectedTitle, WindowUnderTest.Title);
        }

        // <summary>
        // Asserts that the window under test has the given title
        // </summary>
        // <param name="expectedTitle">the expected title of the window</param>
        // <remarks></remarks>
        [Given(@"that the window has the title '(.*)'")]
        [Then(@"the window should have the title '(.*)'")]
        public void WindowIsShown(string expectedTitle)
        {
            SetWindowUnderTest(expectedTitle);
            Assert.IsTrue(WindowUnderTest.Visible);
            Assert.AreEqual(expectedTitle, WindowUnderTest.Title);
        }

        // <summary>
        // Clicks the control with the given name
        // </summary>
        // <param name="wellKnownName">the wellknown name of the control</param>
        // <remarks></remarks>
        [Given(@"that I click on '(.*)'")]
        [When(@"I click on '(.*)'")]
        public void ClickControl(string wellKnownName)
        {
            WindowUnderTest.GetControlByWellKnownName(wellKnownName).Click();
        }

        // <summary>
        // Asserts that the textbox text is the expected text
        // </summary>
        // <param name="wellKnownName">the wellknown name of the textbox</param>
        // <param name="expectedText">the text to expect</param>
        // <remarks></remarks>
        [Then(@"the text in textbox '(.*)' should be '(.*)'")]
        [Given(@"that the text in textbox '(.*)' should be '(.*)'")]
        public void TextBoxShouldHaveText(string wellKnownName, string expectedText)
        {
            var txt = WindowUnderTest.GetControlByWellKnownName<TextBox>(wellKnownName);
            Assert.AreEqual(expectedText, txt.Text);
        }

        // <summary>
        // Asserts that the textbox text starts with the expected text
        // </summary>
        // <param name="wellKnownName">the wellknown name of the textbox</param>
        // <param name="expectedText">the text to expect</param>
        // <remarks></remarks>
        [Then(@"the textbox '(.*)' starts with the text '(.*)'")]
        [Given(@"that the textbox '(.*)' starts with the text '(.*)'")]
        public void TextBoxTextStartsWith(string wellKnownName, string expectedText)
        {
            var txt = WindowUnderTest.GetControlByWellKnownName<TextBox>(wellKnownName);
            Assert.IsTrue(txt.Text.StartsWith(expectedText));
        }

        // <summary>
        // Asserts that the label text contains the expected text
        // </summary>
        // <param name="wellKnownLabelName">the wellknown name of the label</param>
        // <param name="expectedText">the text to expect</param>
        // <remarks></remarks>
        [Given(@"that the label '(.*)' contains the text '(.*)'")]
        [Then(@"the label '(.*)' should contain the text '(.*)'")]
        protected void Assert_LabelContainsText(string wellKnownLabelName, string expectedText)
        {
            var lbl = WindowUnderTest.GetControlByWellKnownName<Label>(wellKnownLabelName);
            Assert.AreEqual(expectedText, lbl.Text);
        }

        // <summary>
        // Writes the text into the textbox with the well known name
        // </summary>
        // <param name="wellKnownName">the name of the textbox</param>
        // <param name="textToWrite">the text to write</param>
        // <remarks></remarks>
        [Given(@"that I write '(.*)' in the textbox '(.*)'")]
        [When(@"I write '(.*)' in the textbox '(.*)'")]
        public void WriteInTextBox(string textToWrite, string wellKnownName)
        {
            WindowUnderTest.GetControlByWellKnownName<TextBox>(wellKnownName).BulkText = textToWrite;
        }

        // <summary>
        // Asserts that the control of the name is present on the 
        // current window under test
        // </summary>
        // <param name="wellKnownName">the well known name of the control</param>
        // <remarks></remarks>
        [Given(@"that the control '(.*)' is shown")]
        [Then(@"the control '(.*)' should be shown")]
        public void ControlIsPresentAndEnabled(string wellKnownName)
        {
            var c = WindowUnderTest.GetControlByWellKnownName(wellKnownName);
            Assert.IsNotNull(c);
            Assert.IsTrue(c.Enabled);
        }

        // <summary>
        // Asserts that the expected item text is the text of
        // the selected item of the combo-box
        // </summary>
        // <param name="expectedItemText"></param>
        // <param name="wellKnownSelectBoxName"></param>
        // <remarks></remarks>
        [Given(@"that '(.*)' is selected in selectbox '(.*)'")]
        [Then(@"'(.*)' should be selected in selectbox '(.*)'")]
        public void ItemShouldBeSelectedInSelectBox(string expectedItemText, string wellKnownSelectBoxName)
        {
            var sel = WindowUnderTest.GetControlByWellKnownName<ComboBox>(wellKnownSelectBoxName);
            Assert.AreEqual(expectedItemText, sel.SelectedItemText);
        }

        // <summary>
        // Selects a value by text in the selectbox
        // </summary>
        // <param name="itemTextToSelect">the text of the item to select</param>
        // <param name="wellKnownSelectBoxName">the wellknown name of the selectbox</param>
        // <remarks></remarks>
        [When(@"I select '(.*)' in the selectbox '(.*)'")]
        public void SelectInSelectBox(string itemTextToSelect, string wellKnownSelectBoxName)
        {
            WindowUnderTest.GetControlByWellKnownName<ComboBox>(wellKnownSelectBoxName).Select(itemTextToSelect);
        }

        // <summary>
        // Double clicks the control with the given name
        // </summary>
        // <param name="wellKnownName">the wellknown name of the control</param>
        // <remarks></remarks>
        [When(@"I doubleclick on '(.*)'")]
        public void DoubleClickControl(string wellKnownName)
        {
            WindowUnderTest.GetControlByWellKnownName(wellKnownName).DoubleClick();
        }

        // <summary>
        // The dialog wit the a given title is shown
        // </summary>
        // <param name="dialogTitle">the title of the dialog</param>
        // <remarks></remarks>
        [Given(@"that the '(.*)' dialog is shown")]
        [Then(@"the '(.*)' dialog is shown")]
        public void DialogWindowIsShown(string dialogTitle)
        {
            var d = WindowUnderTest.GetModalDialogByTitle(dialogTitle);
            SetWindowUnderTest(d);
            Assert.AreEqual(true, d.IsFocussed);
        }

        /// <summary>
        /// Selects the tab with <paramref name="tabTitleToSelect"/> in <paramref name="tabControlWellKnownName"/>
        /// </summary>
        /// <param name="tabTitleToSelect"></param>
        /// <param name="tabControlWellKnownName"></param>
        [When(@"I select the '(.*)' on the tabsystem '(.*)'")]
        public void SelectTab(string tabTitleToSelect, string tabControlWellKnownName)
        {
            var tabControl = WindowUnderTest.GetControlByWellKnownName<Tab>(tabControlWellKnownName);
            tabControl.SelectTabPage(tabTitleToSelect);
        }

        /// <summary>
        /// Asserts that the <paramref name="expectedSelectedTabTitle"/> is the selected
        /// title in the <paramref name="tabControlWellKnownName"/>
        /// </summary>
        /// <param name="expectedSelectedTabTitle"></param>
        /// <param name="tabControlWellKnownName"></param>
        [Given(@"that '(.*)' is the selected tab on the tabsystem '(.*)'")]
        [Then(@"'(.*)' is the selected tab on the tabsystem '(.*)'")]
        public void TabIsSelected(string expectedSelectedTabTitle, string tabControlWellKnownName)
        {
            var tabControl = WindowUnderTest.GetControlByWellKnownName<Tab>(tabControlWellKnownName);
            Assert.AreEqual(expectedSelectedTabTitle, tabControl.SelectedTab.Name);
        }

        /// <summary>
        /// Asserts that the tree has the expected number of nodes
        /// </summary>
        /// <param name="expectedNumberOfNodes">the expected number of nodes</param>
        /// <param name="wellKnownTreeName">the well known name of the tree</param>
        /// <remarks></remarks>
        [Given(@"that the tree '(.*)' should have (\d+) top level nodes")]
        [Then(@"the tree '(.*)' should have (\d+) nodes top level nodes")]
        protected void Assert_NumberOfNodesInTree(string wellKnownTreeName, int expectedNumberOfNodes)
        {
            var tree = WindowUnderTest.GetControlByWellKnownName<Tree>(wellKnownTreeName);
            Assert.AreEqual(expectedNumberOfNodes, tree.Nodes.Count);
        }

        /// <summary>
        /// Expands the node with the given text
        /// </summary>
        /// <param name="nodeText">the text of the node to expand</param>
        /// <param name="wellKnownTreeName">the wellknown name of the tree</param>
        [When(@"I expand the node '(.*)' in tree '(.*)'")]
        public void ExpandTreeNode(string nodeText, string wellKnownTreeName)
        {
            var tree = WindowUnderTest.GetControlByWellKnownName<Tree>(wellKnownTreeName);
            tree.GetNodeByName(nodeText).Expand();
        }

        /// <summary>
        /// Expands the node with the given text
        /// </summary>
        /// <param name="nodeText">the text of the node to expand</param>
        /// <param name="wellKnownTreeName">the wellknown name of the tree</param>
        [When(@"I collapse the node '(.*)' in tree '(.*)'")]
        public void Collapse(string nodeText, string wellKnownTreeName)
        {
            var tree = WindowUnderTest.GetControlByWellKnownName<Tree>(wellKnownTreeName);
            tree.GetNodeByName(nodeText).Collapse();
        }

        /// <summary>
        /// Asserts that the node has the expected number of subnodes
        /// </summary>
        /// <param name="nodeText">the node text to look for</param>
        /// <param name="wellKnownTreeName">the tree to look in</param>
        /// <param name="expectedNumberOfSubnodes">the expected number of subnodes</param>
        [Given(@"that the node '(.*)' in tree '(.*)' has (\d+) subnodes")]
        [Then(@"the node '(.*)' in tree '(.*)' has (\d+) subnodes")]
        public void Assert_NodeHasNumberOfSubNodes(string nodeText, string wellKnownTreeName, int expectedNumberOfSubnodes)
        {
            var tree = WindowUnderTest.GetControlByWellKnownName<Tree>(wellKnownTreeName);
            var node = tree.GetNodeByName(nodeText);
            Assert.AreEqual(expectedNumberOfSubnodes, node.Nodes.Count);
        }

        [When(@"I expand down to '(.*)' in tree '(.*)'")]
        public void ExpandDownToNode(string nodeText,string wellKnownTreeName)
        {
            var tree = WindowUnderTest.GetControlByWellKnownName<Tree>(wellKnownTreeName);
            tree.GetNodeByName(nodeText);
        }
    }
}