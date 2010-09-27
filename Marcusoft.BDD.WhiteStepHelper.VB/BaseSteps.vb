Imports System.IO
Imports White.Core.UIItems.ListBoxItems
Imports TechTalk.SpecFlow
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports White.Core.UIItems
Imports White.Core.UIItems.TreeItems
Imports White.Core.WindowsAPI


<Binding()>
Public Class BaseSteps
    Inherits WhiteStepBase

    Public Sub New()
        MyBase.New(New AutomationNameResolver())
    End Sub

    ''' <summary>
    ''' Asserts that the application is started
    ''' </summary>
    ''' <remarks></remarks>
    <Given("that the application is started")>
        <[Then]("the application should be started")>
    Public Sub ApplicationIsStarted()
        Assert.AreEqual(True, IsApplicationUnderTestRunning)
    End Sub

    ''' <summary>
    ''' Asserts that the window under test has the given title
    ''' </summary>
    ''' <param name="expectedTitle">the expected title of the window</param>
    ''' <remarks></remarks>
    <Given("that the '(.*)' window is shown")>
        <[Then]("the '(.*)' window is shown")>
    Public Sub WindowUnderTestHaveTitle(ByVal expectedTitle As String)
        SetWindowUnderTest(expectedTitle)
        Assert.IsTrue(CurrentWindowUnderTest.Visible)
        Assert.AreEqual(expectedTitle, CurrentWindowUnderTest.Title)
    End Sub

    ''' <summary>
    ''' Asserts that the window under test has the given title
    ''' </summary>
    ''' <param name="expectedTitle">the expected title of the window</param>
    ''' <remarks></remarks>
    <Given("that the window has the title '(.*)'")>
        <[Then]("the window should have the title '(.*)'")>
    Public Sub WindowIsShown(ByVal expectedTitle As String)
        SetWindowUnderTest(expectedTitle)
        Assert.IsTrue(CurrentWindowUnderTest.Visible)
        Assert.AreEqual(expectedTitle, CurrentWindowUnderTest.Title)
    End Sub

    ''' <summary>
    ''' Clicks the control with the given name
    ''' </summary>
    ''' <param name="wellKnownName">the wellknown name of the control</param>
    ''' <remarks></remarks>
    <Given("that I click on '(.*)'")>
        <[When]("I click on '(.*)'")>
    Public Sub ClickControl(ByVal wellKnownName As String)
        GetControl(wellKnownName).Click()
    End Sub

    ''' <summary>
    ''' Asserts that the textbox text is the expected text
    ''' </summary>
    ''' <param name="wellKnownName">the wellknown name of the textbox</param>
    ''' <param name="expectedText">the text to expect</param>
    ''' <remarks></remarks>
    <[Then]("the text in textbox '(.*)' should be '(.*)'")>
        <Given("that the text in textbox '(.*)' should be '(.*)'")>
    Public Sub TextBoxShouldHaveText(ByVal wellKnownName As String, ByVal expectedText As String)
        Dim txt = GetControl(Of TextBox)(wellKnownName)
        Assert.AreEqual(expectedText, txt.Text)
    End Sub

    ''' <summary>
    ''' Asserts that the textbox text starts with the expected text
    ''' </summary>
    ''' <param name="wellKnownName">the wellknown name of the textbox</param>
    ''' <param name="expectedText">the text to expect</param>
    ''' <remarks></remarks>
    <[Then]("the textbox '(.*)' starts with the text '(.*)'")>
        <Given("that the textbox '(.*)' starts with the text '(.*)'")>
    Public Sub TextBoxTextStartsWith(ByVal wellKnownName As String, ByVal expectedText As String)
        Dim txt = GetControl(Of TextBox)(wellKnownName)
        Assert.IsTrue(txt.Text.StartsWith(expectedText))
    End Sub

    ''' <summary>
    ''' Asserts that the label text contains the expected text
    ''' </summary>
    ''' <param name="wellKnownLabelName">the wellknown name of the label</param>
    ''' <param name="expectedText">the text to expect</param>
    ''' <remarks></remarks>
    <[Then]("the label '(.*)' should contain the text '(.*)'")>
    Protected Sub Assert_LabelContainsText(ByVal wellKnownLabelName As String, ByVal expectedText As String)
        Dim lbl = GetControl(Of Label)(wellKnownLabelName)
        Assert.AreEqual(expectedText, lbl.Text)
    End Sub

    ''' <summary>
    ''' Writes the text into the textbox with the well known name
    ''' </summary>
    ''' <param name="wellKnownName">the name of the textbox</param>
    ''' <param name="textToWrite">the text to write</param>
    ''' <remarks></remarks>
    <Given("that I write '(.*)' in the textbox '(.*)'")>
        <[When]("I write '(.*)' in the textbox '(.*)'")>
    Public Sub WriteInTextBox(ByVal textToWrite As String, ByVal wellKnownName As String)
        GetControl(Of TextBox)(wellKnownName).BulkText = textToWrite
    End Sub

    ''' <summary>
    ''' Asserts that the control of the name is present on the 
    ''' current window under test
    ''' </summary>
    ''' <param name="wellKnownName">the well known name of the control</param>
    ''' <remarks></remarks>
    <Given("that the control '(.*)' is shown")>
    Public Sub ControlIsPresentAndEnabled(ByVal wellKnownName As String)
        Dim c = GetControl(wellKnownName)
        Assert.IsNotNull(c)
        Assert.IsTrue(c.Enabled)
    End Sub


    ''' <summary>
    ''' Asserts that the expected item text is the text of
    ''' the selected item of the combo-box
    ''' </summary>
    ''' <param name="expectedItemText"></param>
    ''' <param name="wellKnownSelectBoxName"></param>
    ''' <remarks></remarks>
    <[Given]("that '(.*)' is selected in selectbox '(.*)'")>
    <[Then]("'(.*)' should be selected in selectbox '(.*)'")>
    Public Sub ItemShouldBeSelectedInSelectBox(ByVal expectedItemText As String, ByVal wellKnownSelectBoxName As String)
        Dim sel = GetControl(Of ComboBox)(wellKnownSelectBoxName)
        Assert.AreEqual(expectedItemText, sel.SelectedItemText)
    End Sub

    ''' <summary>
    ''' Selects a value by text in the selectbox
    ''' </summary>
    ''' <param name="itemTextToSelect">the text of the item to select</param>
    ''' <param name="wellKnownSelectBoxName">the wellknown name of the selectbox</param>
    ''' <remarks></remarks>
    <[When]("I select '(.*)' in the selectbox '(.*)'")>
    Public Sub WhenISelectItem2InTheSelectboxTestSelectbox(ByVal itemTextToSelect As String, ByVal wellKnownSelectBoxName As String)
        GetControl(Of ComboBox)(wellKnownSelectBoxName).Select(itemTextToSelect)
    End Sub
    
    ''' <summary>
    ''' Double clicks the control with the given name
    ''' </summary>
    ''' <param name="wellKnownName">the wellknown name of the control</param>
    ''' <remarks></remarks>
    <[When]("I doubleclick on '(.*)'")>
    Public Sub DoubleClickControl(ByVal wellKnownName As String)
        GetControl(wellKnownName).DoubleClick()
    End Sub

    ''' <summary>
    ''' The dialog wit the a given title is shown
    ''' </summary>
    ''' <param name="dialogTitle">the title of the dialog</param>
    ''' <remarks></remarks>
    <Given("that the '(.*)' dialog is shown")>
    <[Then]("the '(.*)' dialog is shown")>
    Public Sub DialogWindowIsShown(ByVal dialogTitle As String)
        Dim d = GetModalDialogByTitle(dialogTitle)
        Assert.AreEqual(True, d.IsFocussed)

    End Sub

    <Given("att trädet '(.*)' har följande yttersta noder: (.*)")>
    Public Sub TrädKontrollHarFöljandeTopNivåNoder(ByVal välkäntNamnPåTräd As String, ByVal nodLista As String)
        Dim nodNamnLista = nodLista.Trim().Split(","c).ToList()
        Assert_TreeHasTopLevelNodes(välkäntNamnPåTräd, nodNamnLista)
    End Sub

    <Given("att noden '(.*)' i '(.*)' är utfälld")>
        <[Then]("ska noden '(.*)' i '(.*)' vara utfälld")>
    Public Sub NodITrädÄrUtfälld(ByVal nodText As String, ByVal trädNamn As String)
        Assert.AreEqual(True, NodeInTree(trädNamn, nodText).IsExpanded)
    End Sub

    <[When]("jag fäller ihop noden '(.*)' i '(.*)'")>
    Public Sub FällIhopNod(ByVal nodText As String, ByVal trädNamn As String)
        NodeInTree(trädNamn, nodText).Collapse()
    End Sub

    <[When]("jag fäller ut noden '(.*)' i '(.*)'")>
        <Given("att jag fäller ut noden '(.*)' i '(.*)'")>
    Public Sub FällUtNod(ByVal nodText As String, ByVal trädNamn As String)
        NodeInTree(trädNamn, nodText).Expand()
    End Sub

    <[Then]("att noden '(.*)' i '(.*)' är ihopfälld")>
        <[Then]("ska noden '(.*)' i '(.*)' vara ihopfälld")>
    Public Sub NodSkaVaraIhopfälld(ByVal nodText As String, ByVal trädNamn As String)
        Assert.IsFalse(NodeInTree(trädNamn, nodText).IsExpanded)
    End Sub

    <[When]("att jag väljer noden '(.*)' i '(.*)'")>
        <Given("att jag väljer noden '(.*)' i '(.*)'")>
    Public Sub VäljerNodITrädet(ByVal nodText As String, ByVal trädNamn As String)
        NodeInTree(trädNamn, nodText).Select()
    End Sub

    <Given("att trädet '(.*)' har (\d+) st noder")>
        <[Then]("ska trädet '(.*)' ha (\d+) st noder")>
    Public Sub TrädHarAntalNoder(ByVal trädNamn As String, ByVal antalNoder As Integer)
        Assert_NumberOfNodesInTree(antalNoder, trädNamn)
    End Sub

    <[Then]("den sista noden i trädet '(.*)' ska ha namnet '(.*)'")>
    Public Sub SistaNodenSkaHaNamnet(ByVal trädNamn As String, ByVal nodeText As String)
        Dim nod = NodeInTree(trädNamn, nodeText)
        Assert.IsNotNull(nod)

        Dim träd = GetControl(Of Tree)(trädNamn)
        Dim sistaNoden = träd.Nodes(träd.Nodes.Count - 1)

        Assert.AreEqual(sistaNoden, nod)
    End Sub

    <Given("att noden '(.*)' i '(.*)' ligger på nivå (\d+)")>
        <[Then]("ska noden '(.*)' i '(.*)' ligga på nivå (\d+)")>
    Public Sub NodLiggerPåNivå(ByVal nodText As String, ByVal trädNamn As String, ByVal nivåNummer As Integer)
        Dim level = NodeLevelForNodeInTree(trädNamn, nodText)

        Assert.AreEqual(nivåNummer, level)
    End Sub



    <[Then]("ska noden '(.*)' ska inte finnas i trädet '(.*)'")>
    Public Sub NodenSkaInteFinnasITräd(ByVal nodText As String, ByVal trädetsNamn As String)
        Assert_NodeNotInTree(trädetsNamn, nodText)
    End Sub

    <[When]("jag trycker kortkommando 'CTRL+(.*)'")>
    Public Sub TryckerKortKommando(ByVal kommando As String)
        CurrentWindowUnderTest.Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL)
        CurrentWindowUnderTest.Keyboard.Enter(kommando)
        CurrentWindowUnderTest.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL)
    End Sub


    <[Then]("ska noden '(.*)' vara den valda noden i trädet '(.*)'")>
    Public Sub ValdNodSkaBara(ByVal nodText As String, ByVal trädNamn As String)
        Assert_SelectedNodeHasNodeText(trädNamn, nodText)
    End Sub

    <[When]("jag väljer rad (\d+) i listan '(.*)'")>
        <Given("att jag väljer rad (\d+) i listan '(.*)'")>
    Public Sub VäljerRadILista(ByVal radNr As Integer, ByVal listansNamn As String)
        SelectRowInListView(listansNamn, radNr - 1)
    End Sub

    <[Then]("ska vald rad (\d+) vara den valda raden i listan '(.*)'")>
    Public Sub RadNrSkaVaraValdRad(ByVal radIndex As Integer, ByVal listansNamn As String)
        Assert_RowIsSelectedRow(listansNamn, radIndex)
    End Sub

    <Given("att det finns (\d+) rader i listan '(.*)'")>
        <[Then]("ska det finnas (\d+) rader i listan '(.*)'")>
    Public Sub DetFinnsAntalRaderILista(ByVal förväntatAntalRader As Integer, ByVal namnPåListan As String)
        Assert_NumberOfRowsInList(namnPåListan, förväntatAntalRader)
    End Sub

    <[Then]("ska kolumn (\d+) på rad (\d+) i listan '(.*)' innehålla '(.*)'")>
    Public Sub TextICellPåLista(ByVal kolumnNr As Integer, ByVal radNr As Integer, ByVal namnPåListan As String, ByVal förväntatVärde As String)
        Assert_CellOnRowInListContainsValue(namnPåListan, radNr, kolumnNr, förväntatVärde)
    End Sub

    <[Then]("ska rad (\d+) i listan '(.*)' ha följande data:")>
    Public Sub RadIListaSkaHaData(ByVal radNummer As Integer, ByVal namnPåListan As String, ByVal tabellDataFrånSteg As Table)
        Assert_RowHasTableData(namnPåListan, radNummer, tabellDataFrånSteg)
    End Sub

    <Given("att dialog '(.*)' visas")>
    <[Then]("ska dialog '(.*)' visas")>
    Public Sub DiaglogrutaVisas(ByVal namnPåDialogRuta As String)
        CurrentWindowUnderTest = GetModalDialogByTitle(namnPåDialogRuta)
    End Sub

    <Given("att fönster '(.*)' visas")>
    <[Then]("ska fönster '(.*)' visas")>
    Public Sub FönsterSkaVaraAktivtFönster(ByVal fönsterNamn As String)
        Dim windows = ApplicationUnderTest.GetWindows()

        Assert.AreEqual(1, windows.Count)
        Assert.AreEqual(fönsterNamn, windows(0).Title)
        Assert.AreEqual(True, windows(0).IsCurrentlyActive)

        SetWindowUnderTest(fönsterNamn)
    End Sub

End Class