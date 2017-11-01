using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealTime.Models;

namespace RealTime.Controllers
{
    public class MyWorkController : Controller
    {
        private readonly RealTimeContext _context;

        public MyWorkController(RealTimeContext context)
        {

           


            _context = context;
        }

        // GET: MyWork
        public async Task<IActionResult> Index()
        {
            return View(await _context.MyModel.ToListAsync());
        }

        // GET: MyWork/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myModel = await _context.MyModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (myModel == null)
            {
                return NotFound();
            }

            return View(myModel);
        }

        // GET: MyWork/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MyWork/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] MyModel myModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(myModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(myModel);
        }

        // GET: MyWork/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myModel = await _context.MyModel.SingleOrDefaultAsync(m => m.Id == id);
            if (myModel == null)
            {
                return NotFound();
            }
            return View(myModel);
        }

        // POST: MyWork/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] MyModel myModel)
        {
            if (id != myModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyModelExists(myModel.Id))
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
            return View(myModel);
        }

        // GET: MyWork/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myModel = await _context.MyModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (myModel == null)
            {
                return NotFound();
            }

            return View(myModel);
        }

        // POST: MyWork/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myModel = await _context.MyModel.SingleOrDefaultAsync(m => m.Id == id);
            _context.MyModel.Remove(myModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyModelExists(int id)
        {
            return _context.MyModel.Any(e => e.Id == id);
        }
    }
}
