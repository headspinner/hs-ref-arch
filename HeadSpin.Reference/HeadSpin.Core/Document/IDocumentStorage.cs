using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadSpin.Core.Utilities.Document
{
    internal interface IDocumentStorage
    {
        void UploadToStorage(byte[] fileBytes, string storedFilePathAndName);

        byte[] DownloadFromStorage(string storedFilePathAndName);
    }
}
