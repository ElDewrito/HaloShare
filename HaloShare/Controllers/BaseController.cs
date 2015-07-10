using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using HaloShare.Security;

namespace HaloShare.Controllers
{
    public class BaseController : Controller
    {

        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private Services.TokenService tokenService;
        public Services.TokenService TokenService
        {
            get
            {
                if (tokenService == null)
                    tokenService = new Services.TokenService();
                return tokenService;
            }
        }

        private Services.ReactionService reactionService;
        public Services.ReactionService ReactionService
        {
            get
            {
                if (reactionService == null)
                    reactionService = new Services.ReactionService();
                return reactionService;
            }
        }

        private Services.FileService fileService;
        public Services.FileService FileService
        {
            get
            {
                if (fileService == null)
                    fileService = new Services.FileService();
                return fileService;
            }
        }

        private Services.UserService userService;
        public Services.UserService UserService
        {
            get
            {
                if (userService == null)
                    userService = new Services.UserService();
                return userService;
            }
        }

        private Services.GameMapService gameMapService;
        public Services.GameMapService GameMapService
        {
            get
            {
                if (gameMapService == null)
                    gameMapService = new Services.GameMapService();
                return gameMapService;
            }
        }

        private Services.GameTypeService gameTypeService;
        public Services.GameTypeService GameTypeService
        {
            get
            {
                if (gameTypeService == null)
                    gameTypeService = new Services.GameTypeService();
                return gameTypeService;
            }
        }

        private Services.ReportService reportService;
        public Services.ReportService ReportService
        {
            get
            {
                if (reportService == null)
                    reportService = new Services.ReportService();
                return reportService;
            }
        }

        public void SetAlert(string message, AlertType type)
        {
            TempData["AlertType"] = type.ToString().ToLower();
            TempData["AlertMessage"] = message;
        }


        public enum AlertType {
            Info,
            Danger,
            Warning,
            Success
        }
    }
}