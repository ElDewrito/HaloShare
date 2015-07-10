using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaloShare.Controllers
{
    [RoutePrefix("Api")]
    public class ApiController : BaseController
    {
        [Route("Forge/{id}")]
        public ActionResult Forge(int id)
        {
            var variant = GameMapService.GetVariant(id);
            if (variant == null || variant.IsDeleted)
                return HttpNotFound();

            return Json(new
            {
                Type = "Forge",
                TypeName = variant.GameMap.Name,
                Id = variant.Id,
                Name = variant.Title,
                Author = variant.Author.UserName,
                Description = variant.ShortDescription,
                Icon = @"/content/images/maps/thumbs/" + variant.GameMap.InternalName + ".jpg",
                Download = Url.Action("Forge", "Download", new { id = variant.Id, token = TokenService.CreateDateTimeToken() }),
                Provider = "HaloShare",
                ProviderIcon = "/content/images/logo/white.png"
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("GameType/{id}")]
        public ActionResult GameType(int id)
        {
            var variant = GameTypeService.GetVariant(id);
            if (variant == null || variant.IsDeleted)
                return HttpNotFound();

            return Json(new
            {
                Type = "GameType",
                TypeName = variant.GameType.Name,
                Id = variant.Id,
                Name = variant.Title,
                Author = variant.Author.UserName,
                Description = variant.ShortDescription,
                Icon = @"/content/images/gametypes/thumbs/" + variant.GameType.InternalName + ".png",
                Download = Url.Action("Forge", "Download", new { id = variant.Id, token = TokenService.CreateDateTimeToken() }),
                Provider = "HaloShare",
                ProviderIcon = "/content/images/logo/white.png"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}