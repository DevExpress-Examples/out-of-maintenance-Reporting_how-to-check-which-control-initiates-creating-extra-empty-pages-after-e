Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports System.Collections

Namespace WindowsApplication1
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
			If Not Me.button1.IsHandleCreated Then Return

			Dim r As New XtraReport1()
			r.CreateDocument()
			ShowControlsOnPage(r, 2)
			'r.ShowPreviewDialog();
		End Sub


		Private Sub ShowControlsOnPage(ByVal report As XtraReport1, ByVal pageNumber As Integer)
			If report.PrintingSystem.Document.PageCount >= pageNumber - 1 Then
'INSTANT VB NOTE: The variable controls was renamed since Visual Basic does not handle local variables named the same as class members well:
				Dim controls_Conflict As IDictionary = GetControlsOnPage(report, pageNumber)
				Dim al As ArrayList = CopyToArrayList(controls_Conflict)

				Dim r As New XtraReport()
				r.DataSource = al
				Dim db As New DetailBand()
				db.Height = 20
				r.Bands.Add(db)
				Dim l As New XRLabel()
				l.DataBindings.Add("Text", Nothing, "FullName")
				l.Location = New Point(0, 0)
				l.Size = New Size(500, 20)
				db.Controls.Add(l)
				Dim tool As New ReportPrintTool(r)
				tool.ShowPreviewDialog()
			End If
		End Sub

		Private Function CopyToArrayList(ByVal controls As IDictionary) As ArrayList
			Dim al As New ArrayList()
			For Each obj As DictionaryEntry In controls
				al.Add(obj.Value)
			Next obj
			Return al
		End Function

		Private Shared Function GetControlsOnPage(ByVal r As XtraReport1, ByVal pageNumber As Integer) As IDictionary
'INSTANT VB NOTE: The variable controls was renamed since Visual Basic does not handle local variables named the same as class members well:
			Dim controls_Conflict As IDictionary = New Hashtable()
			Dim p As Page = r.PrintingSystem.Document.Pages(pageNumber - 1)
			Dim be As BrickEnumerator = p.GetEnumerator()
			Do While be.MoveNext()
				Dim vb As VisualBrick = TryCast(be.Current, VisualBrick)
				Dim mo As New MObject(CType(vb.BrickOwner, XRControl).Name, CType(vb.BrickOwner, XRControl).Report.ToString())
				If Not controls_Conflict.Contains(mo.FullName) Then
					controls_Conflict.Add(mo.FullName, mo)
				End If
			Loop
			Return controls_Conflict
		End Function
	End Class

	Friend Class MObject
		Public Sub New(ByVal n As String, ByVal r As String)
			objname_Conflict = n
			reportName_Conflict = r
		End Sub

'INSTANT VB NOTE: The field objname was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private objname_Conflict As String
		Public Property ObjName() As String
			Get
				Return objname_Conflict
			End Get
			Set(ByVal value As String)
				objname_Conflict = value
			End Set
		End Property
'INSTANT VB NOTE: The field reportName was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private reportName_Conflict As String

		Public Property ReportName() As String
			Get
				Return reportName_Conflict
			End Get
			Set(ByVal value As String)
				reportName_Conflict = value
			End Set
		End Property
		Public ReadOnly Property FullName() As String
			Get
				Return reportName_Conflict & "." & objname_Conflict
			End Get
		End Property

	End Class
End Namespace