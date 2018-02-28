using JgLibDataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JgMaschineAspWeb.Controllers
{
    public class AuswertungController : Controller
    {
        private JgMaschineDb db = new JgMaschineDb();

        [Authorize]
        public async Task<ActionResult> ReportIndex()
        {
            var lReporte = await db.TabReportSet.ToListAsync();
            return View(lReporte);
        }

        [Authorize]
        public ActionResult ReportCreate()
        {
            return View("ReportEdit", new TabReport() { Id = Guid.Empty });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReportCreate(TabReport Report)
        {
            if (ModelState.IsValid)
            {
                Report.Id = Guid.NewGuid();
                Report.Aenderung = DateTime.Now;
                await db.TabReportSet.AddAsync(Report);
                await db.SaveChangesAsync();

                return RedirectToAction("ReportIndex");
            }

            return View("ReportEdit", Report);
        }

        [Authorize]
        public async Task<ActionResult> ReportEdit(Guid? Id)
        {
            if (Id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var report = await db.TabReportSet.FindAsync(Id);
            if (report == null)
                return HttpNotFound();

            return View(report);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReportEdit(Guid Id, HttpPostedFileBase DateiName)
        {
            ModelState.AddModelError("DateiName", "Error! Falscher Dateityp.");

            if (ModelState.IsValid)
            {

                var report = await db.TabReportSet.FindAsync(Id);
                TryUpdateModel(report);

                if (DateiName.ContentLength > 0)
                {
                    var mem = new MemoryStream();
                    await DateiName.InputStream.CopyToAsync(mem);
                    report.ReportDaten = mem.ToArray();
                }

                report.Aenderung = DateTime.Now;
                await db.SaveChangesAsync();

                return RedirectToAction("ReportIndex");
            }

            var rep = new TabReport();
            TryUpdateModel(rep);
            return View(rep);
        }
    }
}
