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
    public class TextBlocksController : Controller
    {
        private readonly HireMeContext _context;

        public TextBlocksController(HireMeContext context)
        {
            _context = context;
        }

        // GET: TextBlocks/Create
        public IActionResult CreateP()
        {
            ViewData["InfoId"] = new SelectList(_context.InfoName, "Id", "Id");
            return PartialView("_CreateTextBlock");
        }

        // POST: TextBlocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateP([Bind("Id,InfoId,Textblock1")] TextBlock textBlock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(textBlock);
                await _context.SaveChangesAsync();
                return PartialView("_IndexName", new InfoViewModel()
                {
                    InfoNameVM = _context.InfoName.Where(s => s.Id == textBlock.InfoId).ToList(),
                    TextBlockVM = _context.TextBlock.Where(s => s.InfoId == textBlock.InfoId).ToList(),
                    PictureVM = _context.Picture.Where(s => s.InfoId == textBlock.InfoId).ToList()
                });
            }
            ViewData["InfoId"] = new SelectList(_context.InfoName, "Id", "Id", textBlock.InfoId);
            return PartialView("_IndexName", new InfoViewModel()
            {
                InfoNameVM = _context.InfoName.Where(s => s.Id == textBlock.InfoId).ToList(),
                TextBlockVM = _context.TextBlock.Where(s => s.InfoId == textBlock.InfoId).ToList(),
                PictureVM = _context.Picture.Where(s => s.InfoId == textBlock.InfoId).ToList()
            });
        }

        // GET: TextBlocks/Edit/5
        public async Task<IActionResult> EditP(int? id)
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
            return PartialView("_EditTextBlock", textBlock);
        }

        // POST: TextBlocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditP(int id, [Bind("Id,InfoId,Textblock1")] TextBlock textBlock)
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
                    await _context.SaveChangesAsync().ConfigureAwait(true);
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

                return PartialView("_IndexName", new InfoViewModel()
                {
                    InfoNameVM = _context.InfoName.Where(s => s.Id == textBlock.InfoId).ToList(),
                    TextBlockVM = _context.TextBlock.Where(s => s.InfoId == textBlock.InfoId).ToList(),
                    PictureVM = _context.Picture.Where(s => s.InfoId == textBlock.InfoId).ToList()
                });
            }
            ViewData["InfoId"] = new SelectList(_context.InfoName, "Id", "Id", textBlock.InfoId);
            return View("Error");
        }

        // GET: TextBlocks/Delete/5
        public async Task<IActionResult> DeleteP(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var textBlock = await _context.TextBlock
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(true);
            if (textBlock == null)
            {
                return NotFound();
            }

            return PartialView("_DeleteTextBlock", textBlock);
        }

        // POST: TextBlocks/Delete/5
        [HttpPost, ActionName("DeleteP")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedP(int id)
        {
            var textBlock = await _context.TextBlock.FindAsync(id);
            _context.TextBlock.Remove(textBlock);
            await _context.SaveChangesAsync().ConfigureAwait(true);
            return PartialView("_IndexName", new InfoViewModel()
            {
                InfoNameVM = _context.InfoName.Where(s => s.Id == textBlock.InfoId).ToList(),
                TextBlockVM = _context.TextBlock.Where(s => s.InfoId == textBlock.InfoId).ToList(),
                PictureVM = _context.Picture.Where(s => s.InfoId == textBlock.InfoId).ToList()
            });
        }

        private bool TextBlockExists(int id)
        {
            return _context.TextBlock.Any(e => e.Id == id);
        }
    }
}
