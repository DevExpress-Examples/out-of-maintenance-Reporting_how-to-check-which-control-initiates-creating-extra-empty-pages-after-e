Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace WindowsApplication1
	Friend Class ReportControlDescriptor
		Public Sub New(ByVal n As String, ByVal r As String)
			Name = n
			ReportName = r
		End Sub

		Public Property Name() As String

		Public Property ReportName() As String
		Public ReadOnly Property FullName() As String
			Get
				Return ReportName & "." & Name
			End Get
		End Property

	End Class
End Namespace
