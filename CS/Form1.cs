using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System.Collections;
using System.Linq;

namespace WindowsApplication1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            XtraReport1 r = new XtraReport1();
            r.CreateDocument();
            ShowControlsOnPage(r, 2);
        }


        private void ShowControlsOnPage(XtraReport report, int pageNumber) {
            if (report.PrintingSystem.Document.PageCount >= pageNumber - 1) {
                List<ReportControlDescriptor> controls = GetControlsOnPage(report, pageNumber);                

                XtraReport r = new XtraReport();
                r.Bands.Add(new PageHeaderBand());
                r.Bands[BandKind.PageHeader].Controls.Add(new XRLabel() {
                    LocationF = new PointF(0, 0f),
                    SizeF = new SizeF(600f, 40f),
                    Text = String.Format("Controls Printed on Page {0}", pageNumber),
                    Font = new Font("Arial", 36f),
                    TextAlignment = TextAlignment.MiddleCenter
                });


                r.DataSource = controls;
                DetailBand db = new DetailBand();
                db.HeightF = 20f;
                r.Bands.Add(db);
                XRLabel l = new XRLabel() {
                    LocationF = new PointF(0, 0f),
                    SizeF = new SizeF(600f, 20f)
                };
                l.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[FullName]"));

                db.Controls.Add(l);
                ReportPrintTool tool = new ReportPrintTool(r);
                tool.ShowPreviewDialog();
            }
        }



        private static List<ReportControlDescriptor> GetControlsOnPage(XtraReport r, int pageNumber) {
            Dictionary<string, ReportControlDescriptor> controls = new Dictionary<string, ReportControlDescriptor>();
            Page p = r.PrintingSystem.Document.Pages[pageNumber - 1];
            BrickEnumerator be = p.GetEnumerator();
            while (be.MoveNext()) {
                VisualBrick vb = be.Current as VisualBrick;
                ReportControlDescriptor mo = new ReportControlDescriptor(((XRControl)vb.BrickOwner).Name, ((XRControl)vb.BrickOwner).Report.ToString());
                if (!controls.ContainsKey(mo.FullName))
                    controls.Add(mo.FullName, mo);
            }
            return controls.Values.ToList();
        }

        private void button2_Click(object sender, EventArgs e) {
            using (XtraReport report = new XtraReport1()) {
                ReportPrintTool tool = new ReportPrintTool(report);
                tool.ShowPreviewDialog();
            }
        }
    }
}

