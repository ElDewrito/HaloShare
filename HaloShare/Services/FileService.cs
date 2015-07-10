using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloShare.Services
{
    public class FileService : BaseService
    {
        public int Count()
        {
            return db.Files.Count();
        }

        public int DownloadCount()
        {
            return db.FileDownloads.Count();
        }
    }
}