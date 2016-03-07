using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;
using System.IO;
using System.Windows.Controls;

namespace MProjectWPF.Controller
{
    class Documents
    {
        public Documents()
        {
            XpsDocument doc = new XpsDocument("C:/Users/David E/Desktop/ANTEPROYECTO.xps", FileAccess.ReadWrite);
            DocumentViewer dc = new DocumentViewer();
            dc.Document = doc.GetFixedDocumentSequence();
                                    
        }
    }
}
