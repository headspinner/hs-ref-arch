using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using HeadSpin.Core.Utilities.Logging;

namespace HeadSpin.Core.Utilities.Document
{
    public static class DocumentSvc
    {
        public static  byte[] ResizePageImage(byte[] pageImage, int h, int w)
        {
            using (MemoryStream saved = new MemoryStream())
            {
                Image i = Image.FromStream(new MemoryStream(pageImage));
                
                Image i2 = ImageHelper.ResizeImage(i, new Size(w, h));

                i2.Save(saved, System.Drawing.Imaging.ImageFormat.Png);

                return saved.ToArray();
            }
        }

        private static bool IsValidFilename(string filename)
        {
            return (!string.IsNullOrWhiteSpace(System.IO.Path.GetExtension(filename)));
        }


        public static string CalculateValidFilename(string filename, string mimetype)
        {
            if (IsValidFilename(filename))
            {
                return filename;
            }

            return string.Format("{0}{1}", filename, CalculateFileExtension(mimetype));
        }
         
        private static string CalculateFileExtension(string mimetype)
        {
            if (mimetype.Contains("wordprocessing"))
            {
                return ".docx";
            }
            else if (mimetype.Contains("spreadsheet"))
            {
                return ".xlsx";
            }
            else if (mimetype.Contains("presentation"))
            {
                return ".pptx";
            }
            else
            {
                try
                {
                    string ext = AskRegistryForDefaultExtension(mimetype);

                    if (!string.IsNullOrWhiteSpace(ext))
                    {
                        return ext;
                    }
                }
                catch(Exception err)
                {
                    // eat it...
                    (new FileLog(ConfigHelper.GetAppSettingValue("file-log"))).Log(err.Message);
                }
                
                throw new Exception("Unable to infer file extension from mime type " + mimetype);
            }
            
        }

        public static string AskRegistryForDefaultExtension(string mimeType)
        {
            string result;
             Microsoft.Win32.RegistryKey key;
            object value;

            key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
            value = key != null ? key.GetValue("Extension", null) : null;
            result = value != null ? value.ToString() : string.Empty;

            return result;
        }
        private static bool NeedsConversion(string filename)
        {
            if (!IsValidFilename(filename))
            {
                throw new Exception("Invalid filename " + filename);
            }

            string ext = System.IO.Path.GetExtension(filename);

            switch (ext.ToLower())
            {
                case ".jpg" : 
                case ".jpeg" :
                case ".gif":
                case ".bmp":
                case ".png":
                    return false;
                default:
                    return true;

            }
        }


        public static IList<string> ConvertPagesToImages(string filepath)
        {
            // maybe return list of uri's...
            if (NeedsConversion(filepath))
            {
                IDocumentConverter converter = null;// new AsposeDocumentToPngConverter();

                return converter.ConvertPagesToImages(filepath);
            }

            var ret = new List<string>(1);
            ret.Add(filepath);

            return ret;
        }

        public static void GetImageDimensions(string storedFilePathAndName, out int heightPx, out int widthPx)
        {
            heightPx = 0;
            widthPx = 0;

            try
            {
                byte[] pageImage = null; //@@@ repo call DownloadFromStorage(storedFilePathAndName);

                if (pageImage != null && pageImage.LongLength > 0)
                {
                    using (MemoryStream saved = new MemoryStream())
                    {
                        Image i = Image.FromStream(new MemoryStream(pageImage));

                        heightPx = i.Height;
                        widthPx = i.Width;
                    }
                }
            }
            catch
            {
                // log?
                throw;
            }

        }

        // todo @@@ should be a repo call...
        //public static byte[] DownloadFromStorage(string storedFilePathAndName)
        //{
        //    IDocumentStorage down = new AsposeDocumentToS3Storage();

        //    return down.DownloadFromStorage(storedFilePathAndName);
        //}

        // todo @@@ repo call doooood!
        //public static void UploadToStorage(byte[] fileBytes, string storedFilePathAndName)
        //{ 
        //    IDocumentStorage up = new AsposeDocumentToS3Storage();

        //    up.UploadToStorage(fileBytes, storedFilePathAndName);
        //}


    }
}
