using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HeadSpin.Core.Utilities.Document
{
    public class PdfGenerator
    {

        private static string GetOpenImageUrl()
        {
            var config = ConfigHelper.GetAppSettingValue("EnvironmentName", "");

            if (string.IsNullOrEmpty(config))
            {
                return "http://whiteboard.prepnow.com/document/openimage";
            }

            return string.Format("http://{0}.whiteboard.prepnow.com/document/openimage", config);

        }

        private static string OnPreConvert(string html)
        {
            XDocument xdoc = XDocument.Parse(html);

            var sb = new StringBuilder(html);
            sb.Replace("\"", "'");
            sb.Replace("<desc style=\"-webkit-tap-highlight-color: rgba(0, 0, 0, 0);\">Created with Raphaël 2.1.2</desc>", "");
            sb.Replace("/document/image", GetOpenImageUrl());
            //sb.Replace("xmlns='http://www.w3.org/2000/svg'", "xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink'");

            //sb.Replace("opacity='1'", "opacity='0'");

            //XDocument xdoc = XDocument.Parse(sb.ToString());
            XName qualifiedName = XName.Get("svg", "http://www.w3.org/2000/svg");

            var svgs = xdoc.Descendants(qualifiedName);
 
            if (svgs !=null )
            {
                foreach (XElement svg in svgs)
                {
                    // get height, width and viewbox attributes
                    XAttribute h = svg.Attribute("height");
                    XAttribute w = svg.Attribute("width");
                    XAttribute vb = svg.Attribute("viewBox");

                    vb.SetValue(string.Format("0 0 {0} {1}", w.Value, h.Value));
                }
            }
                 
            
            return xdoc.ToString() ;
        }
       
        public static byte[] ConvertHtmlToPdf(string html, string tempFolder)
        {   
            // preprocessing
            html = OnPreConvert(html);

            var rootFileName = Guid.NewGuid().ToString();
            var inFile = Path.Combine(tempFolder, rootFileName + ".svg");
            var outFile = Path.Combine(tempFolder, rootFileName + ".pdf");

            //write html to temp local file
            System.IO.File.WriteAllText(@inFile, html);

            string toolPath = ConfigHelper.GetAppSettingValue("pdf-print-tool", "d:\\Program Files\\wkhtmltopdf\\bin\\wkhtmltopdf.exe");

            // run the conversion utility
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.FileName = toolPath;
            psi.CreateNoWindow = true;
            //psi.RedirectStandardInput = true;
            //psi.RedirectStandardOutput = true;
            //psi.RedirectStandardError = true;

            psi.Arguments = " --page-size Letter --orientation Landscape --images \"" + inFile + "\" \"" + outFile + "\"";
            
            var p = Process.Start(psi);
            
            try
            {
               if (p.WaitForExit(15000))
                {
                    return System.IO.File.ReadAllBytes(outFile);
                }
            }
            finally
            {
                p.Close();
                p.Dispose();
                System.IO.File.Delete(inFile);
                System.IO.File.Delete(outFile);
            }

            return null;
        }
    }
}
