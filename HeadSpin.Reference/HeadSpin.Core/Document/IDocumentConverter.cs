using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadSpin.Core.Utilities.Document
{
    internal interface IDocumentConverter
    {  
        IList<string> ConvertPagesToImages(string filename);
    }
}
