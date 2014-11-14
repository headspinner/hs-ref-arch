using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadSpin.Reference.Persistence.Repositories;

namespace HeadSpin.Reference.Domain.Managers
{
  
    internal class UserManager: BaseManager
    {
        private IUserRepository _repo = null;

        public UserManager(IUserRepository argRepository)
        {
            _repo = argRepository;
        }

        public TimeZoneInfo GetUserTimeZoneInfo(DTO.HeadSpinUser user)
        {
            if (!string.IsNullOrWhiteSpace(user.TimeZoneId))
            {
               return TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);
                
            }

            return TimeZoneInfo.Local;
        }

        
        public void SaveHeadSpinUser(DTO.HeadSpinUser user, string userId)
        {
            if (string.IsNullOrEmpty(user.UserId))
            {
                throw new Exception("An identity account must be created first");
            }

            SetUserInfo(user, userId);

            _repo.SaveHeadSpinUser(user);
        }

       
        
        public DTO.HeadSpinUser GetHeadSpinUserById(int id, bool doNotFoundThrow)
        {
            var u = _repo.GetHeadSpinUserById(id);

            if (doNotFoundThrow && u == null)
            {
                throw new Exception("No HeadSpinUser found with Id " + id.ToString()); 
            }

            return u;
        }

       

      
        public DTO.HeadSpinUser GetHeadSpinUserById(int id)
        {
           return  GetHeadSpinUserById(id, false);
        }

        public DTO.HeadSpinUser GetHeadSpinUserByUserId(string userid, bool doNotFoundThrow)
        {
            var u =  _repo.GetHeadSpinUserByUserId(userid);

            if (doNotFoundThrow && u == null)
            {
                throw new Exception("No HeadSpinUser found with Identity Id " + userid);
            }

            return u;
        }

        public DTO.HeadSpinUser GetHeadSpinUserByUserId(string userid)
        {
            return GetHeadSpinUserByUserId(userid, false);
        }

       
    }
}
