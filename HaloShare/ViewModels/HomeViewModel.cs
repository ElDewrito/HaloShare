using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloShare.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Models.GameMapVariant> Maps { get; set; }
        public IEnumerable<Models.GameTypeVariant> Types { get; set; }
    }
}