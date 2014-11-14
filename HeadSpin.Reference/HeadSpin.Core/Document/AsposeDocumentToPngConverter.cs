using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HeadSpin.Core.Utilities.Document
{
    internal class AsposeDocumentToPngConverter : AsposeBase, IDocumentConverter
    {
       
        public AsposeDocumentToPngConverter(): base() { }
         
                
        private IList<string> XLSConvertPagesToPng(string filename)
        {

            string folder = string.Format("{0}/{2}/{1}", this.BucketName, System.IO.Path.GetFileNameWithoutExtension(filename), this.Environment);

            Aspose.Cloud.Cells.Workbook d = new Aspose.Cloud.Cells.Workbook(System.IO.Path.GetFileName(filename));

            var results = d.SplitDocument(Aspose.Cloud.Cells.SplitDocumentFormats.png, folder, this.AsposePreConfigStorageName);

            if (results != null && results.Count() > 0)
            {
                List<string> realResults = null;

                foreach(string r in results)
                {
                    // it's an xls so let's make sure the pages actually have a length...
                    
                    string storedName = string.Format("{0}/{1}", Path.GetFileNameWithoutExtension(filename), Path.GetFileName(r));

                    byte[] page = DocumentSvc.DownloadFromStorage(storedName);

                    if (page != null && page.LongLength > 0)
                    {
                        if(realResults==null)
                        {
                            realResults = new List<string>();
                        }

                        realResults.Add(r);
                    }
                }

                return realResults;
            }

            return null;

        }

        private IList<string> PPTConvertPagesToPng(string filename)
        {
            
            string folder = string.Format("{0}/{2}/{1}", this.BucketName, System.IO.Path.GetFileNameWithoutExtension(filename), this.Environment);

            Aspose.Cloud.Slides.Document d = new Aspose.Cloud.Slides.Document(System.IO.Path.GetFileName(filename));

            var results = d.SplitDocument(Aspose.Cloud.Slides.SplitDocumentFormats.png, folder, this.AsposePreConfigStorageName);

            if (results != null)
            {
                return (from x in results select x.Href).ToList();
            }

            return null;
        }
         

        private IList<string> DOCConvertPagesToPng(string filename)
        {
            string folder = string.Format("{0}/{2}/{1}", this.BucketName, System.IO.Path.GetFileNameWithoutExtension(filename), this.Environment);

            Aspose.Cloud.Words.Document d = new Aspose.Cloud.Words.Document(System.IO.Path.GetFileName(filename));

            var results = d.SplitDocument(Aspose.Cloud.Words.SplitDocumentFormats.png, folder, this.AsposePreConfigStorageName);

            if (results != null)
            {
                return (from x in results select x.Href).ToList();
            }

            return null;
        }

        private IList<string> PDFConvertPagesToPng(string filename)
        {
            string folder = string.Format("{0}/{2}/{1}", this.BucketName, System.IO.Path.GetFileNameWithoutExtension(filename), this.Environment);

            Aspose.Cloud.Pdf.Document d = new Aspose.Cloud.Pdf.Document(System.IO.Path.GetFileName(filename));
            var results = d.SplitDocument(Aspose.Cloud.Pdf.SplitDocumentFormat.png, folder, this.AsposePreConfigStorageName);

            if (results != null)
            {
                return (from x in results select x.Href).ToList();
            }

            return null;
        }

        public IList<string> ConvertPagesToImages(string filename)
        {
            string pathOnStorage = string.Format("{0}/{2}/{1}", this.BucketName, filename, this.Environment);

            string extension = System.IO.Path.GetExtension(filename);

            if (extension.Contains(".doc") ||
                extension.Contains(".docx") ||
                extension.Contains(".txt") ||
                extension.Contains(".rtf"))
            {
                return DOCConvertPagesToPng(pathOnStorage);
            }
            else if (extension.Contains(".pdf"))
            {
                return PDFConvertPagesToPng(pathOnStorage);
            }
            else if (extension.Contains(".xls") || extension.Contains(".xlsx"))
            {
                return XLSConvertPagesToPng(pathOnStorage);
            }
            else if (extension.Contains(".ppt") || extension.Contains(".pptx"))
            {
                return PPTConvertPagesToPng(pathOnStorage);
            }
            else
            {
                throw new NotSupportedException(
                    string.Format("File extension {0} isn't supported.", extension));
            }

        }
    }
}
