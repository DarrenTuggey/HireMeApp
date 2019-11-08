using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HireMeApp.Data;
using HireMeApp.Models;
using HireMeApp.ViewModels;

namespace HireMeApp.Controllers
{
    public class InfoNamesController : Controller
    {
        HireMeContext _context;

        public InfoNamesController(HireMeContext context)
        {
            _context = context;
        }

        // GET: InfoNames
        public async Task<IActionResult> Index()
        {
            return View(await _context.InfoName.ToListAsync());
        }

        // GET: InfoNames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoName = await _context.InfoName
                .FirstOrDefaultAsync(m => m.Id == id);
            if (infoName == null)
            {
                return NotFound();
            }

            return View(infoName);
        }

        // GET: InfoNames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InfoNames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LanguageId,Info_Name")] InfoName infoName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(infoName);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(infoName);
        }

        // GET: InfoNames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoName = await _context.InfoName.FindAsync(id);
            if (infoName == null)
            {
                return NotFound();
            }
            return View(infoName);
        }

        // POST: InfoNames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LanguageId,Info_Name")] InfoName infoName)
        {
            if (id != infoName.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(infoName);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InfoNameExists(infoName.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(infoName);
        }

        // GET: InfoNames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoName = await _context.InfoName
                .FirstOrDefaultAsync(m => m.Id == id);
            if (infoName == null)
            {
                return NotFound();
            }

            return View(infoName);
        }

        // POST: InfoNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var infoName = await _context.InfoName.FindAsync(id);
            _context.InfoName.Remove(infoName);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InfoNameExists(int id)
        {
            return _context.InfoName.Any(e => e.Id == id);
        }

        public ActionResult LoadNameView(int id)
        {
            var infoName = _context.InfoName.Where(s => s.Id == id).ToList();
            var textBlock = _context.TextBlock.Where(a => a.InfoId == id).ToList();
            var picture = _context.Picture.Where(b => b.InfoId == id).ToList();

            var InfoVM = new InfoViewModel
            {
                InfoNameVM = infoName,
                TextBlockVM = textBlock,
                PictureVM = picture
            };

            return PartialView("_IndexName", InfoVM);
        }
    }
}
