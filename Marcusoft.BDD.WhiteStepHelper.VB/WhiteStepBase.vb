Imports TechTalk.SpecFlow
Imports White.Core.Factory
Imports White.Core.UIItems.TreeItems
Imports White.Core
Imports White.Core.UIItems
Imports White.Core.UIItems.WindowItems
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports White.Core.WindowsAPI

''' <summary>
''' Base-class for step definitions with White
''' </summary>
''' <remarks></remarks>
Public MustInherit Class WhiteStepBase
    Private ReadOnly _idResolver As IIdResolver
    Private _controlDic As Dictionary(Of String, IUIItem) = New Dictionary(Of String, IUIItem)

    ''' <summary>
    ''' Constructor that takes an IdResolver to use
    ''' when resolving wellknown-name to id's
    ''' </summary>
    ''' <param name="idResolver">the idResolver to use</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal idResolver As IIdResolver)
        _idResolver = idResolver
    End Sub

    ''' <summary>
    ''' The application we're currently testing
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Shared Property ApplicationUnderTest As White.Core.Application

    ''' <summary>
    ''' The current active window or dialog in the application
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>It's this dialog that the controls are search for</remarks>
    Protected Shared Property CurrentWindowUnderTest As Window

    ''' <summary>
    ''' Starts or attach to the application for the given file name
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <remarks></remarks>
    Public Shared Sub AttachOrLaunchInRunningCurrentDirectory(ByVal fileName As String)
        Dim filePath = My.Application.Info.DirectoryPath + "\" + fileName
        ApplicationUnderTest = White.Core.Application.AttachOrLaunch(New ProcessStartInfo(filePath))
    End Sub

    ''' <summary>
    ''' Starts or attach to the application for the given file name
    ''' and get hold of the window with the given window title
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <param name="windowTitle"></param>
    ''' <remarks></remarks>
    Public Shared Sub StartWindowInApplication(ByVal fileName As String, ByVal windowTitle As String)
        AttachOrLaunchInRunningCurrentDirectory(fileName)
        SetWindowUnderTest(windowTitle)
    End Sub

    ''' <summary>
    ''' Sets the window under test to the window with the given title
    ''' </summary>
    ''' <param name="windowTitle"></param>
    ''' <remarks></remarks>
    Public Shared Sub SetWindowUnderTest(ByVal windowTitle As String)
        Try
            CurrentWindowUnderTest = ApplicationUnderTest.GetWindow(windowTitle, InitializeOption.WithCache.AndIdentifiedBy(windowTitle))
        Catch ex As Exception
            Dim message = String.Format("Cannot find a window with the title '{0}', in the application '{1}'", windowTitle, ApplicationUnderTest.Name)
            Throw New ArgumentException(message)
        End Try
    End Sub

    ''' <summary>
    ''' Returns the dialog with the title
    ''' displayed from the CurrentWindowUnderTest
    ''' </summary>
    ''' <param name="dialogTitle">the title of the dialog</param>
    ''' <returns>the dialog window</returns>
    ''' <remarks></remarks>
    Protected Function GetModalDialogByTitle(ByVal dialogTitle As String) As Window
        Dim dialog = CurrentWindowUnderTest.ModalWindow(dialogTitle)

        Dim message = String.Format("No dialog with title {0} found", dialogTitle)
        Assert.IsNotNull(dialog, message)

        CurrentWindowUnderTest = dialog
        Return dialog
    End Function

    ''' <summary>
    ''' Returns true if the application under test is running
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsApplicationUnderTestRunning() As Boolean
        Return ApplicationUnderTest IsNot Nothing And ApplicationUnderTest.HasExited = False
    End Function

    ''' <summary>
    ''' Closes the application under test
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub CloseApplicationUnderTest()
        If IsApplicationUnderTestRunning() Then
            ApplicationUnderTest.KillAndSaveState()
        End If
    End Sub

    
    ''' <summary>
    ''' Returns the control with the wellknown name sent to the function
    ''' The wellknown name is resolved using the current IdResolver
    ''' </summary>
    ''' <param name="wellKnownName">wellknown name of control</param>
    ''' <returns>the control with the wellknown name</returns>
    ''' <remarks></remarks>
    Protected Function GetControl(ByVal wellKnownName As String) As IUIItem
        Dim id = _idResolver.IdFromWellKnownName(wellKnownName, CurrentWindowUnderTest)
        Dim c = CurrentWindowUnderTest.Get(Finders.SearchCriteria.ByAutomationId(id))

        Assert.IsNotNull(c, String.Format("Could not find a control of with well-known name {0} (translated id: {1})", wellKnownName, id))
        Return c
    End Function
    
    ''' <summary>
    ''' Returns the control with the wellknown name sent to the function
    ''' The wellknown name is resolved using the current IdResolver
    ''' </summary>
    ''' <typeparam name="T">the type of the control to return</typeparam>
    ''' <param name="wellKnownName">wellknown name of control</param>
    ''' <returns>the control with the wellknown name</returns>
    ''' <remarks></remarks>
    Protected Function GetControl(Of T As UIItem)(ByVal wellKnownName As String) As T
        Dim id = _idResolver.IdFromWellKnownName(wellKnownName, CurrentWindowUnderTest)
        Dim c = CurrentWindowUnderTest.Get(Of T)(id)

        Assert.IsNotNull(c, String.Format("Could not find a control of type '{2}' of with well-known name {0} (translated id: {1})", wellKnownName, id, GetType(T).Name))
        Return DirectCast(c, T)
    End Function


    ''' <summary>
    ''' Returns the node with the given text in the tree
    ''' </summary>
    ''' <param name="wellKnownTreeName">the wellknown name of the tree</param>
    ''' <param name="nodeText">the text of the node</param>
    ''' <returns>the node with the given text in the tree</returns>
    ''' <remarks></remarks>
    Protected Function NodeInTree(ByVal wellKnownTreeName As String, ByVal nodeText As String) As TreeItems.TreeNode
        Dim treeControl = GetControl(Of TreeItems.Tree)(wellKnownTreeName)
        Dim node = FindTreeNodeByText(treeControl.Nodes, nodeText)

        Assert.IsNotNull(node, String.Format("Could not find node with text {1} in tree {0}", wellKnownTreeName, nodeText))

        Return node
    End Function

    ''' <summary>
    ''' Asserts that the node with given text is not in the given tree
    ''' </summary>
    ''' <param name="wellKnownTreeName">the wellknown-name of the tree to search</param>
    ''' <param name="nodeText">the text of the node</param>
    ''' <remarks></remarks>
    Protected Sub Assert_NodeNotInTree(ByVal wellKnownTreeName As String, ByVal nodeText As String)
        Dim tree = GetControl(Of Tree)(wellKnownTreeName)

        Assert.IsNull(FindTreeNodeByText(tree.Nodes, nodeText))
    End Sub

    Private Function FindTreeNodeByText(ByVal nodesToSearch As TreeItems.TreeNodes, ByVal nodeText As String) As TreeItems.TreeNode
        Dim nodeToReturn As TreeItems.TreeNode = Nothing

        For Each tn In nodesToSearch
            If tn.Text = nodeText Then
                nodeToReturn = tn
                Exit For
            End If

            If (tn.Nodes.Count > 0) Then
                nodeToReturn = FindTreeNodeByText(tn.Nodes, nodeText)
            End If
        Next

        Return nodeToReturn
    End Function

    ''' <summary>
    ''' Returns the level where the node is present
    ''' </summary>
    ''' <param name="wellKnownTreeName">the name of the tree</param>
    ''' <param name="nodeText">the text of the node</param>
    ''' <returns>the level where the node is present</returns>
    ''' <remarks></remarks>
    Protected Function NodeLevelForNodeInTree(ByVal wellKnownTreeName As String, ByVal nodeText As String) As Integer
        Dim tree = GetControl(Of Tree)(wellKnownTreeName)
        Dim nodeForText = NodeInTree(wellKnownTreeName, nodeText)

        Return tree.GetPathTo(nodeForText).Count
    End Function

    ''' <summary>
    ''' Presses the sent-in specialkey in the current window
    ''' </summary>
    ''' <param name="keys"></param>
    ''' <remarks></remarks>
    Protected Sub PressKey(ByVal keys As KeyboardInput.SpecialKeys)
        CurrentWindowUnderTest.Keyboard.HoldKey(WindowsAPI.KeyboardInput.SpecialKeys.RIGHT)
    End Sub



 

   


    ''' <summary>
    ''' Selects the row with the given number in the listview
    ''' </summary>
    ''' <param name="wellKnownListViewName">the name of the list view</param>
    ''' <param name="rowIndex">the row index</param>
    ''' <remarks></remarks>
    Protected Sub SelectRowInListView(ByVal wellKnownListViewName As String, ByVal rowIndex As Integer)
        Dim listView = GetControl(Of ListView)(wellKnownListViewName)
        Dim indexToSelect = rowIndex
        listView.Rows(indexToSelect).Select()
    End Sub

    ''' <summary>
    ''' Returns the text of the cell on the row for the listbox
    ''' </summary>
    ''' <param name="wellKnownListName">the name of the listview</param>
    ''' <param name="rowNumber">the row number to check</param>
    ''' <param name="columnNumber">the column number</param>
    ''' <returns>the text of the cell</returns>
    ''' <remarks></remarks>
    Private Function GetCellText(ByVal wellKnownListName As String, ByVal rowNumber As Integer, ByVal columnNumber As Integer) As String
        Dim list = GetControl(Of ListView)(wellKnownListName)
        Dim row = list.Rows(rowNumber - 1)
        Dim cell = row.Cells(columnNumber - 1)
        Return cell.Text
    End Function

    ''' <summary>
    ''' Asserts that the selected node in the tree has the sent-in text
    ''' </summary>
    ''' <param name="wellKnownTreeName">the name of the tree</param>
    ''' <param name="expectedTextOfSelectedNode">the expected tree for the selected node</param>
    ''' <remarks></remarks>
    Protected Sub Assert_SelectedNodeHasNodeText(ByVal wellKnownTreeName As String, ByVal expectedTextOfSelectedNode As String)
        Dim n = GetControl(Of Tree)(wellKnownTreeName).SelectedNode
        Assert.AreEqual(expectedTextOfSelectedNode, n.Text)
    End Sub


    ''' <summary>
    ''' Asserts that the label has the expected text
    ''' </summary>
    ''' <param name="wellKnownName">the wellknown name of the label</param>
    ''' <param name="expectedText">the expected text of the label</param>
    ''' <remarks></remarks>
    Protected Sub Assert_LabelTextContains(ByVal wellKnownName As String, ByVal expectedText As String)
        Dim lbl = GetControl(Of Label)(wellKnownName)
        Assert.AreEqual(expectedText, lbl.Text)
    End Sub

    ''' <summary>
    ''' Asserts that the tree has the nodes as it's toplevel nodes
    ''' </summary>
    ''' <param name="wellKnownName">the wellknown name of the tree</param>
    ''' <param name="nodeNames">the name of the nodes the tree should have as toplevel nodes</param>
    ''' <remarks></remarks>
    Protected Sub Assert_TreeHasTopLevelNodes(ByVal wellKnownName As String, ByVal nodeNames As List(Of String))
        Dim tree = GetControl(Of TreeItems.Tree)(wellKnownName)

        For Each nodeName In nodeNames
            Assert.IsTrue(tree.HasNode(nodeName.Trim()))
        Next
    End Sub


    ''' <summary>
    ''' Asserts that the tree has the expected number of nodes
    ''' </summary>
    ''' <param name="expectedNumberOfNodes">the expected number of nodes</param>
    ''' <param name="wellKnownTreeName">the well known name of the tree</param>
    ''' <remarks></remarks>
    Protected Sub Assert_NumberOfNodesInTree(ByVal expectedNumberOfNodes As Integer, ByVal wellKnownTreeName As String)
        Assert.AreEqual(expectedNumberOfNodes, GetControl(Of TreeItems.Tree)(wellKnownTreeName).Nodes.Count)
    End Sub



    ''' <summary>
    ''' Asserts that the control of the given typen and name is present on the 
    ''' current window under test
    ''' </summary>
    ''' <typeparam name="T">the type of the control to check</typeparam>
    ''' <param name="wellKnownName">the well known name of the control</param>
    ''' <remarks></remarks>
    Protected Sub Assert_ControlOfTypeIsPresentOnWindowUnderTest(Of T As UIItem)(ByVal wellKnownName As String)
        Assert.IsNotNull(GetControl(Of T)(wellKnownName))
    End Sub

   

    ''' <summary>
    ''' Asserts that the row with the given number is the selected row
    ''' of the listview with the well known name
    ''' </summary>
    ''' <param name="wellKnownListViewName">the name of the list view</param>
    ''' <param name="rowIndex">the row index to select</param>
    ''' <remarks></remarks>
    Protected Sub Assert_RowIsSelectedRow(ByVal wellKnownListViewName As String, ByVal rowIndex As Integer)
        Dim listView = GetControl(Of ListView)(wellKnownListViewName)

        Dim expectedSelectedRow = listView.Rows(rowIndex)
        Assert.AreEqual(expectedSelectedRow.Cells(0).Text, listView.SelectedRows(0).Cells(0).Text)
    End Sub

    ''' <summary>
    ''' Asserts that the list has the expected number of rows
    ''' </summary>
    ''' <param name="wellKnownNameOfList">the name of the list</param>
    ''' <param name="expectedNumberOfRows">the expected number of rows</param>
    ''' <remarks></remarks>
    Protected Sub Assert_NumberOfRowsInList(ByVal wellKnownNameOfList As String, ByVal expectedNumberOfRows As Integer)
        Dim listView = GetControl(Of ListView)(wellKnownNameOfList)
        Assert.AreEqual(expectedNumberOfRows, listView.Rows.Count)
    End Sub

    ''' <summary>
    ''' Asserts that the text for the cell contains the expected text
    ''' </summary>
    ''' <param name="wellKnownListName">the name of the listview</param>
    ''' <param name="rowNumber">the row number to check</param>
    ''' <param name="columnNumber">the column number</param>
    ''' <param name="expectedValue">the expected value</param>
    ''' <remarks></remarks>
    Protected Sub Assert_CellOnRowInListContainsValue(ByVal wellKnownListName As String, ByVal rowNumber As Integer, ByVal columnNumber As Integer, ByVal expectedValue As String)
        Assert.AreEqual(expectedValue, GetCellText(wellKnownListName, rowNumber, columnNumber))
    End Sub

    ''' <summary>
    ''' Asserts that the row number in the list has the expected data
    ''' </summary>
    ''' <param name="wellKnownListName">the wellknown name of the list</param>
    ''' <param name="rowNumber">the number of the row to check</param>
    ''' <param name="expectedData">the expected data</param>
    ''' <remarks></remarks>
    Protected Sub Assert_RowHasTableData(ByVal wellKnownListName As String, ByVal rowNumber As Integer, ByVal expectedData As Table)

        Dim listView = GetControl(Of ListView)(wellKnownListName)
        Dim rowFromGUI = listView.Rows(rowNumber - 1)

        For Each expectedRowFromStep In expectedData.Rows
            For Each columnName In expectedData.Header

                Dim textInGUI = rowFromGUI.Cells(columnName).Text
                Dim expectedTextFromStep = expectedRowFromStep(columnName)

                Assert.AreEqual(expectedTextFromStep, textInGUI)
            Next
        Next
    End Sub

    
End Class