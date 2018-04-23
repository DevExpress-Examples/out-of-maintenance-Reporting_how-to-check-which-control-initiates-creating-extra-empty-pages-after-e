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

namespace WindowsApplication1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            XtraReport1 r = new XtraReport1();
            r.CreateDocument();
            ShowControlsOnPage(r, 2);
            //r.ShowPreviewDialog();
        }


        private void ShowControlsOnPage(XtraReport1 report, int pageNumber) {
            if(report.PrintingSystem.Document.PageCount >= pageNumber - 1) {
                IDictionary controls = GetControlsOnPage(report, pageNumber);
                ArrayList al = CopyToArrayList(controls);

                XtraReport r= new XtraReport();
                r.DataSource = al;
                DetailBand db = new DetailBand();
                db.Height = 20;
                r.Bands.Add(db);
                XRLabel l = new XRLabel();
                l.DataBindings.Add("Text", null, "FullName");
                l.Location = new Point(0, 0);
                l.Size = new Size(500, 20);
                db.Controls.Add(l);
                r.ShowPreviewDialog();
            }
        }

        private ArrayList CopyToArrayList(IDictionary controls) {
            ArrayList al = new ArrayList();
            foreach(DictionaryEntry obj in controls) {
                al.Add(obj.Value);
            }
            return al;
        }

        private static IDictionary GetControlsOnPage(XtraReport1 r, int pageNumber) {
            IDictionary controls = new Hashtable();
            Page p = r.PrintingSystem.Document.Pages[pageNumber - 1];
            BrickEnumerator be = p.GetEnumerator();
            while(be.MoveNext()) {
                VisualBrick vb = be.Current as VisualBrick;
                MObject mo = new MObject(((XRControl)vb.BrickOwner).Name, ((XRControl)vb.BrickOwner).Report.ToString());
                if(!controls.Contains(mo.FullName))
                    controls.Add(mo.FullName, mo);
            }
            return controls;
        }
    }

    class MObject {
        public MObject(string n, string r) { objname = n; reportName = r; }

        string objname;
        public string ObjName {
            get { return objname; }
            set { objname = value; }
        }
        string reportName;

        public string ReportName {
            get { return reportName; }
            set { reportName = value; }
        }
        public string FullName {
            get { return reportName + "." + objname; }
        }

    }
}