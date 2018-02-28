using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JgMaschineAspWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            using (var db = new JgLibDataModel.JgMaschineDb())
            {
                var ben = db.TabBedienerSet.FirstOrDefault();


            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[Authorize]
        //public async Task<ActionResult> ReportBearbeiten(Guid? Id)
        //{
        //    if (Id == null)
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        //    var report = await db.TabReportSet.Include(i => i.EMaschine)
        //        .Include(f => f.EMaschine.EStandort).Include(b => b.EBediener)
        //        .FirstOrDefaultAsync(f => f.Id == Id);

        //    if (meldung == null)
        //        return HttpNotFound();

        //}
    }
}