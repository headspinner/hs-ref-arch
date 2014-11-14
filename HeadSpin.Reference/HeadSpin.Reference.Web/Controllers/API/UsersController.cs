using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
 using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;

using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

using PrepNow.Classroom.Web.Models;
using PrepNow.Classroom.Web.Models.API;
using PrepNow.Classroom.Web.API;


namespace PrepNow.Classroom.Web.Controllers.API
{
    [System.Web.Http.AuthorizeAttribute( Roles="API")]
    public class UsersController : ApiController
    {

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        //@@@ revisit
        //TODO
        //private const string API_USER = "api";

        private ApplicationUserManager _userManager;

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

        
        // POST: api/Users/5
        [HttpPost]
        [Route("api/users")]
        [ValidateModel]
        public HttpResponseMessage UpdateAccount(UpdateAccount a)
        {
            try
            {
                var preppie = Domain.Services.UserService.GetPrepNowUserByCrmId(a.CrmId);

                if (preppie==null)
                {
                    return this.Request.CreateErrorResponse(HttpStatusCode.NotFound,
                          "No account found by Crm Id " + a.CrmId.ToString());
                }

                var u = UserManager.FindById(preppie.UserId);

                if (u == null)
                {
                    return this.Request.CreateErrorResponse(HttpStatusCode.NotFound,
                          "No identity account found by Id " + preppie.UserId);
                }

                if (!string.IsNullOrWhiteSpace(a.Email))
                {
                    u.Email = a.Email;
                    u.UserName = a.Email;
                }

                if (!string.IsNullOrWhiteSpace(a.FirstName))
                {
                    preppie.FirstName = a.FirstName;
                }

                if (!string.IsNullOrWhiteSpace(a.LastName))
                {
                    preppie.LastName = a.LastName;
                }

                if (!string.IsNullOrWhiteSpace(a.PhoneNo))
                {
                    preppie.PhoneNo = a.PhoneNo;
                }

                if(string.IsNullOrWhiteSpace(preppie.TimeZoneId))
                {
                    // call the utility
                    preppie.TimeZoneId = PrepNow.Utilities.Address.AddressSvc.GetTimeZone(a.Address);
                }

                UserManager.Update(u);

                //Domain.Services.UserService.SavePrepNowUser(preppie, API_USER);
                Domain.Services.UserService.SavePrepNowUser(preppie, this.User.Identity.GetUserId());

                // wireup the response...
                var response = AutoMapper.Mapper.Map< AccountResponse>(preppie);
                response.Email = u.Email;

                return this.Request.CreateResponse(HttpStatusCode.OK, response);
                
            }
            catch(Exception e)
            {
                return this.Request.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, e.Message);
            }
        }


        [HttpGet]
        [Route("api/users/{crmId}")]
        public IHttpActionResult GetAccount(int crmId)
        {
            // @@@ helper... probably not used by crm land....
            var preppie = Domain.Services.UserService.GetPrepNowUserByCrmId(crmId);

            if (null == preppie)
            {
                return NotFound();
            }

            var u = UserManager.FindById(preppie.UserId);

            // wireup the response...
            var response = AutoMapper.Mapper.Map<AccountResponse>(preppie);
            response.Email = u.Email;

            return Ok(response);
        }


        //[HttpPost]        
        //[Route("api/users/relationship/new")]
        //[ValidateModel]
        //public IHttpActionResult CreateTutorStudentRelationship(TutorToStudent ts)
        //{   
        //    Domain.Services.UserService.CreateTutorStudentRelationship(ts.TutorCrmId, ts.StudentCrmId, API_USER);

        //    return Ok();
        //}

        [HttpPost]
        [Route("api/users/relationship/tutor/{tutorCrmId}/student/{studentCrmId}")]
        [ValidateModel]
        public IHttpActionResult CreateTutorStudentRelationship(int tutorCrmId, int studentCrmId)
        {
            //Domain.Services.UserService.CreateTutorStudentRelationship(tutorCrmId, studentCrmId, API_USER);
            Domain.Services.UserService.CreateTutorStudentRelationship(tutorCrmId, studentCrmId, this.User.Identity.GetUserId());

            return Ok();
        }


        // POST: api/Users
        [HttpPost]
        [Route("api/users/{identityId}/resetpassword/{newPassword}")]
        [ValidateModel]
        public HttpResponseMessage ResetPassword(string newPassword, string identityId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                        "Must specify a new password");
                }

                if (string.IsNullOrWhiteSpace(identityId))
                {
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                        "Must have an identity id to lookup the user");
                }

                var u = UserManager.FindById(identityId);

                if (u == null)
                {
                    return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, 
                        "No identity account found by id " + identityId);
                }
                
                var token = UserManager.GeneratePasswordResetToken(identityId);

                UserManager.ResetPassword(identityId, token, newPassword);

                return this.Request.CreateResponse(HttpStatusCode.OK, "Password changed");

            }
            catch (Exception e)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }

        }

         
        // POST: api/Users
        [HttpPost]
        [Route("api/users/new")]
        [ValidateModel]
        public HttpResponseMessage CreateAccount( NewAccount a)
        {   
            var user = new ApplicationUser() { UserName = a.Email, Email = a.Email };

            IdentityResult result = UserManager.Create(user, a.Password);
            
            if (!result.Succeeded)
            {
                var errmsg = new System.Text.StringBuilder();

                foreach (var error in result.Errors)
                {
                    errmsg.AppendLine(error);
                }

                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, errmsg.ToString());
            }
             
            try
            {
                var dto = AutoMapper.Mapper.Map<DTO.PrepNowUser>(a);

                // custom mapping :)
                dto.UserId = user.Id;
                dto.TimeZoneId = PrepNow.Utilities.Address.AddressSvc.GetTimeZone(a.Address);

                //Domain.Services.UserService.CreateAccount(dto, API_USER);
                Domain.Services.UserService.CreateAccount(dto, this.User.Identity.GetUserId());

                // wireup the response...
                var response = AutoMapper.Mapper.Map< AccountResponse>(dto);
                response.Email = user.Email;

                return this.Request.CreateResponse(HttpStatusCode.Created, response);
            }
            catch (Exception ex)
            {
                if (user != null)
                {
                    UserManager.Delete(user);
                }
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
