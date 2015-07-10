using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaloShare.Controllers
{
    [RoutePrefix("profiles")]
    public class ProfileController : BaseController
    {
        // GET: Profile
        [Route("{userName}")]
        public ActionResult Index(string userName)
        {
            ViewModels.Profile profile = UserService.GetProfileByUserName(userName);

            return View(profile);
        }
    }
}