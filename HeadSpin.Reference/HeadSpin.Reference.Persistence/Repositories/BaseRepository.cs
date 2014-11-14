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
   
    public class BaseRepository 
    {
        protected static readonly string ConnectionString =
          ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    }
     
}
