using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;
using HaloShare.VariantLib;
using HaloShare.Core;
using System.Security.Cryptography;

namespace HaloShare.Controllers
{
    [Authorize]
    [RoutePrefix("upload")]
    public class UploadController : BaseController
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View(new ViewModels.UploadIndex
            {
                JsonGameMaps = Newtonsoft.Json.JsonConvert.SerializeObject(GameMapService.GetMaps()),
                JsonGameTypes = Newtonsoft.Json.JsonConvert.SerializeObject(GameTypeService.GetTypes())
            });
        }

        [HttpPost]
        [Route("gametype")]
        public ActionResult GameType(ViewModels.UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                Models.GameTypeVariant variant = new Models.GameTypeVariant();
                variant.Title = model.Title.Replace("\0", "");
                variant.ShortDescription = model.Description;
                variant.Description = model.Content;
                variant.CreatedOn = DateTime.UtcNow;
                variant.AuthorId = User.Identity.GetUserId<int>();
                variant.File = new Models.File()
                {
                    FileSize = model.File.ContentLength,
                    FileName = Guid.NewGuid().ToString() + ".gtv",
                    UploadedOn = variant.CreatedOn,
                    MD5 = model.File.InputStream.ToMD5()
                };

                var validateGame = GameTypeService.ValidateHash(variant.File.MD5);

                if(validateGame != null)
                {
                    Response.StatusCode = 400;
                    return Content(string.Format(
                            "<b>Keep it Clean!</b> The game variant has already been uploaded: <a target=\"_blank\" href=\"{0}\">{1}</a>.",
                            Url.Action("Details", "GameType", new { slug = validateGame.Slug }),
                            validateGame.Title));
                }

                /* Read the map type from the uploaded file.
                 * This is also a validation message to make sure
                 * that the file uploaded is an actual map.
                 */

                var detectType = VariantDetector.Detect(model.File.InputStream);
                if (detectType == VariantType.Invalid)
                {
                    // The given map doesn't exist.
                    model.File.InputStream.Close();

                    Response.StatusCode = 400;
                    return Content("<b>Keep it Clean!</b> The file uploaded is invalid. Please make sure it's a valid game variant.");
                }
                else if (detectType == VariantType.ForgeVariant)
                {
                    // The given map doesn't exist.
                    model.File.InputStream.Close();

                    Response.StatusCode = 400;
                    return Content("<strong>PARDON OUR DUST!</strong> Can't upload forge variant as game variant.");
                }

                string path = System.IO.Path.Combine(Server.MapPath("~/Content/Files/GameType/"), variant.File.FileName);
                using (var stream = new System.IO.MemoryStream())
                {
                    model.File.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
                    model.File.InputStream.CopyTo(stream);

                    GameVariant variantItem = new GameVariant(stream);

                    var type = GameTypeService.GetByInternalId(variantItem.TypeId);

                    if (type != null)
                    {
                        variant.GameTypeId = type.Id;

                        variantItem.VariantName = model.Title;
                        variantItem.VariantAuthor = User.Identity.GetUserName();
                        variantItem.VariantDescription = variant.ShortDescription;
                        variantItem.Save();


                        // Save the file.
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.Seek(0, System.IO.SeekOrigin.Begin);
                            stream.CopyTo(fileStream);
                        }
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        return Content("<strong>PARDON OUR DUST!</strong> We currently do not support the uploaded gametype.");
                    }
                }

                GameTypeService.AddVariant(variant);
                GameTypeService.Save();

                return Content(Url.Action("Details", "GameType", new { slug = variant.Slug }));
            }

            Response.StatusCode = 400;
            return View("~/Views/Shared/_ModelState.cshtml");
        }


        [HttpPost]
        [Route("forge")]
        public ActionResult Forge(ViewModels.UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                Models.GameMapVariant variant = new Models.GameMapVariant();
                variant.Title = model.Title.Replace("\0", "");
                variant.ShortDescription = model.Description;
                variant.Description = model.Content;
                variant.CreatedOn = DateTime.UtcNow;
                variant.AuthorId = User.Identity.GetUserId<int>();
                variant.File = new Models.File()
                {
                    FileSize = model.File.ContentLength,
                    FileName = Guid.NewGuid().ToString() + ".vrt",
                    UploadedOn = variant.CreatedOn,
                    MD5 = model.File.InputStream.ToMD5()
                };
                // Validate the variant to see if it's not a duplicate.

          
                var validateMap = GameMapService.ValidateHash(variant.File.MD5);

                if (validateMap != null)
                {
                    Response.StatusCode = 400;
                    return Content(string.Format(
                            "<b>Keep it Clean!</b> The forge variant has already been uploaded: <a target=\"_blank\" href=\"{0}\">{1}</a>.",
                            Url.Action("Details", "Forge", new { slug = validateMap.Slug }),
                            validateMap.Title));
                }

                /* Read the map type from the uploaded file.
                 * This is also a validation message to make sure
                 * that the file uploaded is an actual map.
                 */

                var detectType = VariantDetector.Detect(model.File.InputStream);
                if (detectType == VariantType.Invalid)
                { 
                    // The given map doesn't exist.
                    model.File.InputStream.Close();

                    Response.StatusCode = 400;
                    return Content("<b>Keep it Clean!</b> The file uploaded is invalid. Please make sure the map is in the new format.");
                }
                else if (detectType == VariantType.GameVariant)
                {
                    // The given map doesn't exist.
                    model.File.InputStream.Close();

                    Response.StatusCode = 400;
                    return Content("<strong>PARDON OUR DUST!</strong> Can't upload game variant as forge variant.");
                }

                string path = System.IO.Path.Combine(Server.MapPath("~/Content/Files/Forge/"), variant.File.FileName);
                using (var stream = new System.IO.MemoryStream())
                {
                    model.File.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
                    model.File.InputStream.CopyTo(stream);

                    ForgeVariant variantItem = new ForgeVariant(stream);

                    var map = GameMapService.GetMapByInternalId(variantItem.MapId);

                    if (map != null)
                    {
                        variant.GameMapId = map.Id;

                        variantItem.VariantName = model.Title;
                        variantItem.VariantAuthor = User.Identity.GetUserName();
                        variantItem.VariantDescription = variant.ShortDescription;
                        variantItem.Save();


                        // Save the file.
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.Seek(0, System.IO.SeekOrigin.Begin);
                            stream.CopyTo(fileStream);
                        }
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        return Content("<strong>PARDON OUR DUST!</strong> We currently do not support the uploaded map.");
                    }
                }

                GameMapService.AddVariant(variant);
                GameMapService.Save();

                return Content(Url.Action("Details", "Forge", new { slug = variant.Slug }));
            }

            Response.StatusCode = 400;
            return View("~/Views/Shared/_ModelState.cshtml");
        }

        public ActionResult GameType()
        {

            return View();
        }
    }
}