using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadSpin.Reference.DTO
{
    public class HeadSpinUser : BaseDTO
    {
        // transient from aspnet identity user... loaded by the db :)
        public string Email { get; set; }

        public byte[] Avatar { get; set; }

        public string AvatarMimeType { get; set; }

        public string PracticeWhiteboardSessionRefNo { get; set; }

        public string UserId  { get; set; }

        public string SkypeId { get; set; }

        public string TimeZoneId { get; set; }

        public string FirstName  { get; set; }

        public string LastName { get; set; }

        public string PhoneNo { get; set; }

        public Enums.UserType TypeCode { get; set; }

        public int CrmId { get; set; }

        public string Fullname
        {
            get 
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

       
    }
}
