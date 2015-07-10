using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloShare.Services
{
    public class BaseService
    {
        protected Models.ApplicationDbContext db;

        public BaseService()
        {
            db = new Models.ApplicationDbContext();
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}