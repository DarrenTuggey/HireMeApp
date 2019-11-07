using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HireMeApp.Data;
using HireMeApp.Models;
using Microsoft.AspNetCore.Http;

namespace HireMeApp.Controllers
{
    public class PicturesController : Controller
    {
        private readonly HireMeContext _context;

        public PicturesController(HireMeContext context)
        {
            _context = context;
        }

        // GET: Pictures
        public async Task<IActionResult> Index()
        {
            var hireMeContext = _context.Picture.Include(p => p.Info);
            return View(await hireMeContext.ToListAsync());
        }

        // GET: Pictures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Picture
                .Include(p => p.Info)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (picture == null)
            {
                return NotFound();
            }

            return View(picture);
        }

        // GET: Pictures/Create
        public IActionResult Create()
        {
            ViewData["InfoId"] = new SelectList(_context.InfoName, "Id", "Id");
            return View();
        }

        //Modified create to use IFormFile to convert image to byte stream.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(include: "InfoId,Pic,ImageMimeType")] Picture picture, IFormFile image, int infoId)
        {
            await using (var fs1 = image.OpenReadStream())
            {
                await using var ms1 = new MemoryStream();
                fs1.CopyTo(ms1);
                picture.Pic = ms1.ToArray();
            }
            picture.ImageMimeType = image.ContentType;
            picture.InfoId = infoId;
            if (ModelState.IsValid)
            {
                _context.Picture.Add(picture);
                await _context.SaveChangesAsync().ConfigureAwait(true);
                return RedirectToAction(nameof(Index));
            }
            return View(picture);
        }

        // GET: Pictures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Picture.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }
            ViewData["InfoId"] = new SelectList(_context.InfoName, "Id", "Id", picture.InfoId);
            return View(picture);
        }

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InfoId,Pic,ImageMimeType")] Picture picture)
        {
            if (id != picture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(picture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PictureExists(picture.Id))
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
            ViewData["InfoId"] = new SelectList(_context.InfoName, "Id", "Id", picture.InfoId);
            return View(picture);
        }

        // GET: Pictures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Picture
                .Include(p => p.Info)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (picture == null)
            {
                return NotFound();
            }

            return View(picture);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var picture = await _context.Picture.FindAsync(id);
            _context.Picture.Remove(picture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PictureExists(int id)
        {
            return _context.Picture.Any(e => e.Id == id);
        }

        public FileContentResult GetImage(int id)
        {

            Picture photo = _context.FindPhotoById(id);

            if (photo != null)
            {
                return File(photo.Pic, photo.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}
