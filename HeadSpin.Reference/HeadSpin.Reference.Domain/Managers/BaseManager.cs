using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadSpin.Reference.Persistence.Repositories;

namespace HeadSpin.Reference.Domain.Managers
{
  
    internal class BaseManager
    {   
        public void SetUserInfo(DTO.BaseDTO dto, string userId)
        {
            if (dto.Id <= 0)
            {
                dto.CreateUserId = userId;
            }
            else
            {
                dto.UpdateUserId = userId;
            }
        }
 
    }
}
