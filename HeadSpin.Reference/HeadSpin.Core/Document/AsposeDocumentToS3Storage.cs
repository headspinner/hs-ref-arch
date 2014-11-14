using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace HeadSpin.Core.Utilities.Document
{
    internal class AsposeDocumentToS3Storage : AsposeBase, IDocumentStorage
    {

        public AsposeDocumentToS3Storage() : base() { }

        public byte[] DownloadFromStorage(string storedFilePathAndName)
        {
            var storagePath = System.IO.Path.GetDirectoryName(storedFilePathAndName);
            var fileName = System.IO.Path.GetFileName(storedFilePathAndName);

            string path = string.Format("{0}/{2}/{1}", this.BucketName, storedFilePathAndName, this.Environment);  

            Aspose.Cloud.Storage.Folder f = new Aspose.Cloud.Storage.Folder();
            var file = f.GetFile(path, Aspose.Cloud.Storage.StorageType.AmazonS3, this.AsposePreConfigStorageName);

            using(var memoryStream = new MemoryStream())
            {
              file.CopyTo(memoryStream);
              return memoryStream.ToArray();
            }

        }

        public void UploadToStorage(byte[] fileBytes, string storedFilePathAndName )
        {  
            //
            var storagePath = System.IO.Path.GetDirectoryName(storedFilePathAndName);
            var fileName = System.IO.Path.GetFileName(storedFilePathAndName);
            
            string path = string.Format("{0}/{2}/{1}", this.BucketName, storagePath, this.Environment);  

            Aspose.Cloud.Storage.Folder f = new Aspose.Cloud.Storage.Folder();
            f.UploadFile(fileBytes, fileName, path, Aspose.Cloud.Storage.StorageType.AmazonS3, this.AsposePreConfigStorageName);
        }
    }
}
