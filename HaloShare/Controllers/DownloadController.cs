using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using HaloShare.Models;
using Microsoft.AspNet.Identity;

namespace HaloShare.Controllers
{
    [RoutePrefix("dwnl")]
    public class DownloadController : BaseController
    {
        // GET: Download
        [Route("f/{id}/{*token}")]
        public void Forge(int id, string token)
        {
            if (!TokenService.ValidateDateTimeToken(token))
            {
                Response.StatusCode = 403;
                Response.End();
                return;
            }

            var variant = GameMapService.GetVariant(id);

            variant.File.Downloads.Add(new FileDownload()
            {
                DownloadedOn = DateTime.UtcNow,
                UserId = Request.IsAuthenticated ? (int?)User.Identity.GetUserId<int>() : null,
                UserIP = Request.UserHostAddress
            });
            variant.File.DownloadCount += 1;
            GameMapService.Save();

            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + variant.DownloadName);
            Response.TransmitFile(Path.Combine(Server.MapPath("~/Content/Files/Forge"), variant.File.FileName));
            Response.End();
        }

        [Route("g/{id}/{*token}")]
        public void GameType(int id, string token)
        {
            if (!TokenService.ValidateDateTimeToken(token))
            {
                Response.StatusCode = 403;
                Response.End();
                return;
            }

            var variant = GameTypeService.GetVariant(id);

            variant.File.Downloads.Add(new FileDownload()
            {
                DownloadedOn = DateTime.UtcNow,
                UserId = Request.IsAuthenticated ? (int?)User.Identity.GetUserId<int>() : null,
                UserIP = Request.UserHostAddress
            });
            variant.File.DownloadCount += 1;
            GameTypeService.Save();

            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + variant.DownloadName);
            Response.TransmitFile(Path.Combine(Server.MapPath("~/Content/Files/GameType"), variant.File.FileName));
            Response.End();
        }
    }
}