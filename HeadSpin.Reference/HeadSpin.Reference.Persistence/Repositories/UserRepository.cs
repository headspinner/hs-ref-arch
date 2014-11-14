using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insight.Database;
using System.Data.SqlClient;
using System.Configuration;

namespace HeadSpin.Reference.Persistence.Repositories
{
    public class UserRepostitory : BaseRepository, IUserRepository  
    {

     


       

        public void SaveHeadSpinUser(DTO.HeadSpinUser user)
        {
            var c = new OptimisticConnection(new SqlConnection(ConnectionString));

            try
            {
                IUserRepository repo = c.As<IUserRepository>();

                repo.SaveHeadSpinUser(user);
            }
            catch (OptimisticConcurrencyException ox)
            {
                throw new Exception("The information you're trying to update was changed whilst you were in the process of updating. Concurrency Exception", ox);
            }
        }


        public DTO.HeadSpinUser GetHeadSpinUserByUserId(string userId)
        {
            var c = new SqlConnection(ConnectionString);

            IUserRepository repo = c.As<IUserRepository>();

            return repo.GetHeadSpinUserByUserId(userId);
        }

        public DTO.HeadSpinUser GetHeadSpinUserById(int id)
        {
            var c = new SqlConnection(ConnectionString);

            IUserRepository repo = c.As<IUserRepository>();

            return repo.GetHeadSpinUserById(id);
        }

    }
     
}
