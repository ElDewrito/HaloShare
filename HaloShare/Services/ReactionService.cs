using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloShare.Services
{
    public class ReactionService : BaseService
    {
        public int Count()
        {
            return db.Reactions.Count();
        }
    }
}