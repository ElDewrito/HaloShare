using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloShare.Services
{
    public class ReportService : BaseService
    {
        public IEnumerable<Models.Report> GetReports(bool? handled)
        {
            IEnumerable<Models.Report> data = db.Reports.Include("Author");

            if (handled.HasValue)
            {
                data = data.Where(n => n.IsHandled == handled);
            }


            return data.OrderBy(n => n.IsHandled).ThenByDescending(n => n.ReportedOn);
        }

        public Models.Report Get(int id)
        {
            return db.Reports.Find(id);
        }

        public void AddReport(Models.Report model)
        {
            db.Reports.Add(model);
        }
    }
}