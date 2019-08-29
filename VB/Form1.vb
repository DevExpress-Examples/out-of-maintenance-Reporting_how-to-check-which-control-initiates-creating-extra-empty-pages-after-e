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
Imports System.Linq

Namespace WindowsApplication1
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
			Dim r As New XtraReport1()
			r.CreateDocument()
			ShowControlsOnPage(r, 2)
		End Sub


		Private Sub ShowControlsOnPage(ByVal report As XtraReport, ByVal pageNumber As Integer)
			If report.PrintingSystem.Document.PageCount >= pageNumber - 1 Then
'INSTANT VB NOTE: The variable controls was renamed since Visual Basic does not handle local variables named the same as class members well:
				Dim controls_Renamed As List(Of ReportControlDescriptor) = GetControlsOnPage(report, pageNumber)

				Dim r As New XtraReport()
				r.Bands.Add(New PageHeaderBand())
				r.Bands(BandKind.PageHeader).Controls.Add(New XRLabel() With {
					.LocationF = New PointF(0, 0F),
					.SizeF = New SizeF(600F, 40F),
					.Text = String.Format("Controls Printed on Page {0}", pageNumber),
					.Font = New Font("Arial", 36F),
					.TextAlignment = TextAlignment.MiddleCenter
				})


				r.DataSource = controls_Renamed
				Dim db As New DetailBand()
				db.HeightF = 20F
				r.Bands.Add(db)
				Dim l As New XRLabel() With {
					.LocationF = New PointF(0, 0F),
					.SizeF = New SizeF(600F, 20F)
				}
				l.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "[FullName]"))

				db.Controls.Add(l)
				Dim tool As New ReportPrintTool(r)
				tool.ShowPreviewDialog()
			End If
		End Sub



		Private Shared Function GetControlsOnPage(ByVal r As XtraReport, ByVal pageNumber As Integer) As List(Of ReportControlDescriptor)
'INSTANT VB NOTE: The variable controls was renamed since Visual Basic does not handle local variables named the same as class members well:
			Dim controls_Renamed As New Dictionary(Of String, ReportControlDescriptor)()
			Dim p As Page = r.PrintingSystem.Document.Pages(pageNumber - 1)
			Dim be As BrickEnumerator = p.GetEnumerator()
			Do While be.MoveNext()
				Dim vb As VisualBrick = TryCast(be.Current, VisualBrick)
				Dim mo As New ReportControlDescriptor(CType(vb.BrickOwner, XRControl).Name, CType(vb.BrickOwner, XRControl).Report.ToString())
				If Not controls_Renamed.ContainsKey(mo.FullName) Then
					controls_Renamed.Add(mo.FullName, mo)
				End If
			Loop
			Return controls_Renamed.Values.ToList()
		End Function

		Private Sub button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button2.Click
			Using report As XtraReport = New XtraReport1()
				Dim tool As New ReportPrintTool(report)
				tool.ShowPreviewDialog()
			End Using
		End Sub
	End Class
End Namespace

