using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaloShare.Services
{
    public class GameTypeService : BaseService
    {
        public IEnumerable<Models.GameTypeVariant> SearchVariants(string q, string orderBy, bool asc, int? typeId, bool? staffPick, int? authorId, bool hideDeleted = true)
        {
            IEnumerable<Models.GameTypeVariant> variants = db.GameTypeVariants;

            if(hideDeleted)
                variants = variants.Where(n => n.IsDeleted == false);

            if (typeId.HasValue)
            {
                variants = variants.Where(n => n.GameTypeId == typeId);
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


        public IEnumerable<SelectListItem> GetTypeSelectlist(int? typeId)
        {
            return db.GameTypes.Select(n =>
                new SelectListItem()
                {
                    Value = n.Id.ToString(),
                    Text = n.Name,
                    Selected = typeId.HasValue && typeId == n.Id
                });
        }

        public Models.GameType GetType(int id)
        {
            return db.GameTypes.Find(id);
        }

        public IEnumerable<Models.GameType> GetTypes()
        {
            return db.GameTypes;
        }

        public IEnumerable<Models.GameTypeVariant> GetVariants(int gameTypeId)
        {
            return db.GameTypeVariants.Where(v => v.GameTypeId == gameTypeId);
        }

        public Models.GameTypeVariant GetVariant(int id)
        {
            return db.GameTypeVariants.Find(id);
        }

        public Models.GameTypeVariant ValidateHash(string md5Hash)
        {
            return db.GameTypeVariants.Where(n => n.IsDeleted == false)
                .FirstOrDefault(n => n.File.MD5 == md5Hash);
        }

        public int Reply(ViewModels.ReplyRequest model, int userId)
        {
            var reaction = new Models.Reaction
            {
                Comment = model.Comment,
                AuthorId = userId,
                GameTypeVariantId = model.Id,
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

        public Models.GameType GetByInternalId(int id)
        {
            return db.GameTypes.FirstOrDefault(n => n.InternalId == id);
        }

        public void AddVariant(Models.GameTypeVariant variant)
        {
            db.GameTypeVariants.Add(variant);
        }
    }
}