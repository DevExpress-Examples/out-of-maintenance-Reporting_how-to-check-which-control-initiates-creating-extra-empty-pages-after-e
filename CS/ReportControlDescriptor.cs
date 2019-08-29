using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsApplication1 {
    class ReportControlDescriptor {
        public ReportControlDescriptor(string n, string r) { Name = n; ReportName = r; }

        public string Name {
            get;
            set;
        }

        public string ReportName {
            get;
            set;
        }
        public string FullName {
            get { return ReportName + "." + Name; }
        }

    }
}
