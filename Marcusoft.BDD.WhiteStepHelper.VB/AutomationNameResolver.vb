Imports System.Windows.Automation
Imports White.Core.UIItems.WindowItems
Imports White.Core.UIItems


''' <summary>
''' Assumes that the wellknown name of a control is stored in the AutomationProperties.Name property
''' </summary>
''' <example>
''' AutomationProperties.Name="TestTräd"
''' </example>
''' <remarks></remarks>
Public Class AutomationNameResolver
    Implements IIdResolver

    Private _lookedUpIds As Dictionary(Of String, String)

    ''' <summary>
    ''' Constructs a new name resolver that looks for controls by AutomationProperties.Name
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _lookedUpIds = New Dictionary(Of String, String)
    End Sub

    ''' <summary>
    ''' Returns the id from the already looked up ids
    ''' or
    ''' empty string if not present
    ''' </summary>
    ''' <param name="wellKnownName">the wellknown name to look for</param>
    ''' <param name="windowTitle">the window title</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IDFromSavedResult(ByVal wellKnownName As String, ByVal windowTitle As String) As String
        Dim id = String.Empty
        Dim key = KeyForWellKnownName(windowTitle, wellKnownName)

        If (_lookedUpIds.ContainsKey(key)) Then
            id = _lookedUpIds(key)
        End If

        Return id
    End Function

    Private Function KeyForWellKnownName(ByVal windowTitle As String, ByVal wellKnownName As String) As String
        Return windowTitle + "_" + wellKnownName
    End Function

    ''' <summary>
    ''' Returns the ID for an controls well-known name in a certain window
    ''' </summary>
    ''' <param name="wellKnownName">This is the name that is used in the Gehrkin-specs, typically the name a user is user when referring to the control</param>
    ''' <param name="windowToSearch">the window to search in</param>
    ''' <returns>the id of the control</returns>
    ''' <remarks></remarks>
    Public Function IdFromWellKnownName(ByVal wellKnownName As String, ByVal windowToSearch As White.Core.UIItems.WindowItems.Window) As String Implements IIdResolver.IdFromWellKnownName
        Dim id = IDFromSavedResult(wellKnownName, windowToSearch.Title)

        If String.IsNullOrEmpty(id) Then
            id = SearchForIDOnWindow(wellKnownName, windowToSearch)

            Dim key = KeyForWellKnownName(windowToSearch.Title, wellKnownName)
            _lookedUpIds.Add(key, id)
        End If


        Return id
    End Function

    Private Function SearchForIDOnWindow(ByVal wellKnownName As String, ByVal windowToSearch As White.Core.UIItems.WindowItems.Window) As String
        Dim id = String.Empty

        Dim crit = Finders.SearchCriteria.ByNativeProperty(AutomationElement.NameProperty, wellKnownName)

        Try
            id = windowToSearch.Get(crit).PrimaryIdentification
        Catch ex As Exception
            Dim message = String.Format("Could not find any control with AutomationProperties.Name=""{0}"" in Window '{1}'", wellKnownName, windowToSearch.Title)
            Throw New ArgumentException(message)
        End Try
        Return id
    End Function
End Class