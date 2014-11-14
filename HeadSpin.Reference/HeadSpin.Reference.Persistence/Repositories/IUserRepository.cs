using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadSpin.Reference.Persistence.Repositories
{
    public interface IUserRepository
    {
        //void CreateAccount(DTO.HeadSpinUser user, DTO.WhiteboardSession practice);

        void SaveHeadSpinUser(DTO.HeadSpinUser user);

        //void SaveTutorToStudent(DTO.TutorToStudent relationship);

        DTO.HeadSpinUser GetHeadSpinUserByUserId(string userId);
        
        DTO.HeadSpinUser GetHeadSpinUserById(int id);
      
    }

}
