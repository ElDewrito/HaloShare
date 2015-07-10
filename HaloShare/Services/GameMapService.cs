using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaloShare.Services
{
    public class GameMapService : BaseService
    {
        public IEnumerable<Models.GameMapVariant> SearchVariants(string q, string orderBy, bool asc, int? mapId, bool? staffPick, int? authorId, bool hideDeleted = true)
        {
            IEnumerable<Models.GameMapVariant> variants = db.GameMapVariants;

            if (hideDeleted)
                variants = variants.Where(n => n.IsDeleted == false);

            if (mapId.HasValue)
            {
                variants = variants.Where(n => n.GameMapId == mapId);
            }


            if (staffPick.HasValue && staffPick.Value == true)
            {
                variants = variants.Where(n => n.IsStaffPick == true);
            }

            if (authorId.HasValue)
            {
                variants = variants.Where(n => n.AuthorId == authorId);
            }

            if (!string.IsNullOrWhiteSpace(q))
            {
                string[] query = q.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                variants = variants.Where(v => query.Any(x => v.Title.ToLower().Contains(x)));
            }

           

            if (!string.IsNullOrEmpty(orderBy))
            {
                if (asc)
                {
                    switch (orderBy.ToLower())
                    {
                        case "name":
                            variants = variants.OrderBy(n => n.Title);
                            break;
                        case "release":
                            variants = variants.OrderBy(n => n.CreatedOn).ThenBy(n => n.Title);
                            break;
                        case "rating":
                            variants = variants.OrderBy(n => n.Rating).ThenBy(n => n.Title);
                            break;
                        case "downloads":
                            variants = variants.OrderBy(n => n.File.DownloadCount).ThenBy(n => n.Title);
                            break;
                    }
                }
                else
                {
                    switch (orderBy.ToLower())
                    {
                        case "name":
                            variants = variants.OrderByDescending(n => n.Title);
                            break;
                        case "release":
                            variants = variants.OrderByDescending(n => n.CreatedOn).ThenBy(n => n.Title);
                            break;
                        case "rating":
                            variants = variants.OrderByDescending(n => n.Rating).ThenBy(n => n.Title);
                            break;
                        case "downloads":
                            variants = variants.OrderByDescending(n => n.File.DownloadCount).ThenBy(n => n.Title);
                            break;
                    }
                }
               
            }


            


            return variants;
        }

        public IEnumerable<Models.GameMap> GetMaps()
        {
            return db.GameMaps;
        }

        public Models.GameMap GetMap(int id)
        {
            return db.GameMaps.Find(id);
        }

        public IEnumerable<SelectListItem> GetMapSelectlist(int? mapId)
        {
            return db.GameMaps.Select(n =>
                new SelectListItem()
                {
                    Value = n.Id.ToString(),
                    Text = n.Name,
                    Selected = mapId.HasValue && mapId == n.Id
                });
        }

        public Models.GameMap GetMapByInternalName(string name)
        {
            return db.GameMaps.FirstOrDefault(n => n.InternalName == name);
        }

        public Models.GameMap GetMapByInternalId(int id)
        {
            return db.GameMaps.FirstOrDefault(n => n.InternalId == id);
        }

        public IEnumerable<Models.GameMapVariant> GetVariants(int mapId)
        {
            return db.GameMapVariants.Where(v => v.GameMapId == mapId);
        }

        public Models.GameMapVariant GetVariant(int id)
        {
            return db.GameMapVariants.Find(id);
        }

        public void AddVariant(Models.GameMapVariant model)
        {
            db.GameMapVariants.Add(model);
        }

        public void UpdateVariant(Models.GameMapVariant model)
        {
            db.GameMapVariants.Attach(model);
            var entry = db.Entry(model);
            entry.State = System.Data.Entity.EntityState.Modified;
        }

        public Models.GameMapVariant ValidateHash(string md5Hash)
        {
            return db.GameMapVariants.Where(n => n.IsDeleted == false)
                .FirstOrDefault(n => n.File.MD5 == md5Hash);
        }

        public int Reply(ViewModels.ReplyRequest model, int userId)
        {
            var reaction = new Models.Reaction
            {
                Comment = model.Comment,
                AuthorId = userId,
                GameMapVariantId = model.Id,
                ParentReactionId = model.ParentId,
                IsDeleted = false,
                PostedOn = DateTime.UtcNow
            };

            db.Reactions.Add(reaction);

            db.SaveChanges();

            return reaction.Id;
        }

        public ViewModels.RateResponse Rate(int id, int userId, int value)
        {
            var type = GetVariant(id);

            var userRated = type.Ratings.FirstOrDefault(_ => _.AuthorId == userId);

            if (userRated == null)
            {
                type.Ratings.Add(new Models.Rating
                {
                    AuthorId = userId,
                    Rate = value,
                    RatedOn = DateTime.UtcNow
                });

                type.RatingCount += 1;
                type.Rating = type.Ratings.Average(_ => _.Rate);

                db.SaveChanges();
            }
            else
            {
                userRated.Rate = value;
                userRated.RatedOn = DateTime.UtcNow;
                type.Rating = type.Ratings.Average(_ => _.Rate);
                db.SaveChanges();
            }

            return new ViewModels.RateResponse
            {
                Count = type.RatingCount,
                Rating = Math.Round(type.Rating)
            };
        }
    }
}