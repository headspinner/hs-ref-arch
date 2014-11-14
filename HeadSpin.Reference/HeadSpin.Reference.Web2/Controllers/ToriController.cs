using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using HeadSpin.Reference.Web2.Models;

namespace HeadSpin.Reference.Web2.Controllers
{
    [Authorize]
    public class ToriController : ApiController
    {
        private ApplicationUserManager _userManager;

        public ToriController()
        {
        }

        public ToriController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET api/Tori
        public ToriInfoViewModel Get()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return new ToriInfoViewModel() { Name = user.UserName, Bday=DateTime.UtcNow };
        }

        public bool Post(ToriInfoViewModel tori)
        {
            if (tori==null)
            {
                return false;
            }
            if (!string.IsNullOrWhiteSpace(tori.Name))
            {
                return true;
            }
            return false;

        }
    }
}