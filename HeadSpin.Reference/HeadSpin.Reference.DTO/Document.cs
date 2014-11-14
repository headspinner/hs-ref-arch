using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadSpin.Reference.DTO
{
    public class Document : BaseDTO
    {
        public Document(): base()
        {
            this.DeleteDate = null;
            this.SessionId = null;
        }

        public DateTime? DeleteDate { get; set; }

        public string MimeType { get; set; }

        public string Category { get; set; }

        public string FileName  { get; set; }

        public int? SessionId { get; set; }

        public string StoredFilePathAndName
        {
            get 
            {
                return string.Format("{0}/{0}{1}", this.Id, System.IO.Path.GetExtension(this.FileName));
            }
        }

        public IList<DocumentPage> DocumentPages { get; set; }
    }
}
