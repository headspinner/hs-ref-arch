using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadSpin.Reference.Persistence.Repositories;

namespace HeadSpin.Reference.Domain.Services
{
   
    public class UserService
    {
        public static TimeZoneInfo GetUserTimeZoneInfo(DTO.HeadSpinUser user)
        {
            return (new Managers.UserManager(new UserRepostitory())).GetUserTimeZoneInfo(user);
        }
       

     

        public static DTO.HeadSpinUser GetHeadSpinUserByUserId(string identityUserId, bool doNotFoundThrow)
        {
            return (new Managers.UserManager(new UserRepostitory())).GetHeadSpinUserByUserId(identityUserId, doNotFoundThrow);
        }

        public static DTO.HeadSpinUser GetHeadSpinUserByUserId(string identityUserId)
        {
            return GetHeadSpinUserByUserId(identityUserId, false);
        }


  

        public static void SaveHeadSpinUser(DTO.HeadSpinUser user, string userId)
        {
            (new Managers.UserManager(new UserRepostitory())).SaveHeadSpinUser(user, userId);
        }
       
    }
}
