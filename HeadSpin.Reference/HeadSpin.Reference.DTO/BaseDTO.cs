using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadSpin.Reference.DTO
{
    public class BaseDTO
    {
        public BaseDTO()
        {
            this.Id = 0;
            this.LockDate = DateTime.Now;
        }

        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LockDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string CreateUserId { get; set; }

        public string UpdateUserId { get; set; }


    }
}
