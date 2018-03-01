using JgLibDataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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
            try
            {
                if (Id == null)
                    throw new Exception("Id ist null");

                var report = await db.TabReportSet.FindAsync(Id);
                if (report == null)
                    throw new Exception("Report nicht gefunden.");

                return View(report);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReportEdit(Guid Id, IFormFile DateiName)
        {
            ModelState.AddModelError("DateiName", "Error! Falscher Dateityp.");

            if (ModelState.IsValid)
            {

                var report = await db.TabReportSet.FindAsync(Id);
                await TryUpdateModelAsync(report);

                using (var reader = new StreamReader(DateiName.OpenReadStream()))
                {
                    var fileContent = reader.ReadToEnd();
                    var parsedContentDisposition = ContentDispositionHeaderValue.Parse(DateiName.ContentDisposition);
                    var fileName = parsedContentDisposition.FileName;
                }

                //if (DateiName.ContentLength > 0)
                //{
                //    var mem = new MemoryStream();
                //    await DateiName.InputStream.CopyToAsync(mem);
                //    report.ReportDaten = mem.ToArray();
                //}

                report.Aenderung = DateTime.Now;
                await db.SaveChangesAsync();

                return RedirectToAction("ReportIndex");
            }

            var rep = new TabReport();
            await TryUpdateModelAsync(rep);
            return View(rep);
        }
    }
}
