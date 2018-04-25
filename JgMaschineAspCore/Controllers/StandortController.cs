using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JgLibDataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JgMaschineAspCore.Controllers
{
    public class StandortController : Controller
    {
        private readonly JgMaschineDb db;
        private readonly ILogger _logger;

        public StandortController(IConfiguration configuration,
            ILogger<AccountController> logger)
        {
            _logger = logger;
            db = new JgMaschineDb() { SqlVerbindung = configuration.GetConnectionString("ConnectionData") };
        }
        
        public async Task<IActionResult> Index()
        {
            var standort = db.TabStandortSet.OrderBy(o => o.StandortName);
            return View(await standort.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.FormHelper = new TFormHelper() { IsCreate = true };
            return View(new TabStandort() { Id = Guid.NewGuid() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TabStandort standort)
        {
            if (ModelState.IsValid)
            {
                await db.TabStandortSet.AddAsync(standort);
                await db.DbSave(User, standort);

                return RedirectToAction("Index");
            }

            return View(standort);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var standort = await db.TabStandortSet.FindAsync(id);
            if (standort != null)
            {
                ViewBag.FormHelper = new TFormHelper() { IsCreate = false };
                return View("Create", standort);
            }
            else
                return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit1(Guid id)
        {
            var standort = await db.TabStandortSet.FindAsync(id);
            await TryUpdateModelAsync(standort);

            if (ModelState.IsValid)
            {
                await db.DbSave(User, standort);
                return RedirectToAction("Index");
            }

            return View("Create", standort);
        }
    }
}