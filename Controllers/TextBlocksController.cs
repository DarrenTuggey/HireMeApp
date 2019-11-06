using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HireMeApp.Data;
using HireMeApp.Models;

namespace HireMeApp.Controllers
{
    public class TextBlocksController : Controller
    {
        private readonly HireMeContext _context;

        public TextBlocksController(HireMeContext context)
        {
            _context = context;
        }

        // GET: TextBlocks
        public async Task<IActionResult> Index()
        {
            var hireMeContext = _context.TextBlock.Include(t => t.Info);
            return View(await hireMeContext.ToListAsync());
        }

        // GET: TextBlocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var textBlock = await _context.TextBlock
                .Include(t => t.Info)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (textBlock == null)
            {
                return NotFound();
            }

            return View(textBlock);
        }

        // GET: TextBlocks/Create
        public IActionResult Create()
        {
            ViewData["InfoId"] = new SelectList(_context.InfoName, "Id", "Id");
            return View();
        }

        // POST: TextBlocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InfoId,Textblock1")] TextBlock textBlock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(textBlock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InfoId"] = new SelectList(_context.InfoName, "Id", "Id", textBlock.InfoId);
            return View(textBlock);
        }

        // GET: TextBlocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var textBlock = await _context.TextBlock.FindAsync(id);
            if (textBlock == null)
            {
                return NotFound();
            }
            ViewData["InfoId"] = new SelectList(_context.InfoName, "Id", "Id", textBlock.InfoId);
            return View(textBlock);
        }

        // POST: TextBlocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InfoId,Textblock1")] TextBlock textBlock)
        {
            if (id != textBlock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(textBlock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TextBlockExists(textBlock.Id))
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
            ViewData["InfoId"] = new SelectList(_context.InfoName, "Id", "Id", textBlock.InfoId);
            return View(textBlock);
        }

        // GET: TextBlocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var textBlock = await _context.TextBlock
                .Include(t => t.Info)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (textBlock == null)
            {
                return NotFound();
            }

            return View(textBlock);
        }

        // POST: TextBlocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var textBlock = await _context.TextBlock.FindAsync(id);
            _context.TextBlock.Remove(textBlock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TextBlockExists(int id)
        {
            return _context.TextBlock.Any(e => e.Id == id);
        }
    }
}
