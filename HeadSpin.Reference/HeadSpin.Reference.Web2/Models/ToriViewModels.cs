using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeadSpin.Reference.Web2.Models
{
    // Models returned by ToriController actions.
    public class ToriInfoViewModel
    {
        public string Name { get; set; }
        public DateTime Bday { get; set; }
    }

   
}