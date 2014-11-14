using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadSpin.Reference.DTO
{
    public class DocumentPage : BaseDTO
    {   
        public string ImageFileExt  { get; set; }

        public int DocumentId { get; set; }

        public int PageNo { get; set; }

        public int HeightPx { get; set; }

        public int WidthPx { get; set; }

        public string StoredFilename { get; set; }

        public string StoredFilePath
        {
            get
            {
                return string.Format("{0}/{1}", this.DocumentId, this.StoredFilename);
            }
        }
    }
}
