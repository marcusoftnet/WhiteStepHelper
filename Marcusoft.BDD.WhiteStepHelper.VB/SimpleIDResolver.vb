''' <summary>
''' This is a real simple convention based id resolver
''' The convention is that the ID of the controll is the same as the wellknown name
''' transformed to lowercase and no spaces
''' </summary>
''' <remarks></remarks>
Public Class SimpleIDResolver
    Implements IIdResolver

    Public Function IdFromWellKnownName(ByVal wellKnownName As String, ByVal windowToSearch As White.Core.UIItems.WindowItems.Window) As String Implements IIdResolver.IdFromWellKnownName
        Return wellKnownName.Replace(" ", String.Empty).ToLower()
    End Function
End Class