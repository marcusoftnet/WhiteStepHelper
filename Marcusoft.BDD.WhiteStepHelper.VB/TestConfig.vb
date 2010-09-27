Imports System.Configuration

''' <summary>
''' Reads configuration properties from .config
''' </summary>
''' <remarks></remarks>
Public Class TestConfig
    Public Shared ReadOnly Property ApplicationUnderTestPath As String
        Get
            Return ConfigurationManager.AppSettings("ApplicationUnderTestPath")
        End Get
    End Property

    Public Shared ReadOnly Property MainWindowName As String
        Get
            Return ConfigurationManager.AppSettings("MainWindowName")
        End Get
    End Property
End Class
