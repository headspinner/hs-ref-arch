using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadSpin.Reference.Domain
{
    public class Initializer
    {
        public static void Init()
        {
            HeadSpin.Reference.Persistence.Initializer.Init();
        }
    }
}
