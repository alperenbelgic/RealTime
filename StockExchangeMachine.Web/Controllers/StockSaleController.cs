using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockExchangeMachine.Web.Models;

namespace StockExchangeMachine.Web.Controllers
{
    public class StockSaleController : Controller
    {
        private readonly TheContext _context;

        public StockSaleController(TheContext context)
        {
            //_context = context;
        }



        public IActionResult Operate()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy([Bind("Id,Count,Price")] StockSaleViewModel stockSaleViewModel)
        {
            if (ModelState.IsValid)
            {
                TempStatic.TempStockProduct.Bid(stockSaleViewModel.Count, stockSaleViewModel.Price, "customer");
            }
            return View("Create", stockSaleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sell([Bind("Id,Count,Price")] StockSaleViewModel stockSaleViewModel)
        {
            if (ModelState.IsValid)
            {
                TempStatic.TempStockProduct.Offer(stockSaleViewModel.Count, stockSaleViewModel.Price, "customer");
            }
            return View("Create", stockSaleViewModel);
        }



        // GET: StockSale
        public async Task<IActionResult> Index()
        {
            return View(await _context.StockSaleViewModel.ToListAsync());
        }

        // GET: StockSale/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockSaleViewModel = await _context.StockSaleViewModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (stockSaleViewModel == null)
            {
                return NotFound();
            }

            return View(stockSaleViewModel);
        }

        // GET: StockSale/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StockSale/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Count,Price")] StockSaleViewModel stockSaleViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockSaleViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stockSaleViewModel);
        }



        // GET: StockSale/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockSaleViewModel = await _context.StockSaleViewModel.SingleOrDefaultAsync(m => m.Id == id);
            if (stockSaleViewModel == null)
            {
                return NotFound();
            }
            return View(stockSaleViewModel);
        }

        // POST: StockSale/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Count,Price")] StockSaleViewModel stockSaleViewModel)
        {
            if (id != stockSaleViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockSaleViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockSaleViewModelExists(stockSaleViewModel.Id))
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
            return View(stockSaleViewModel);
        }

        // GET: StockSale/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockSaleViewModel = await _context.StockSaleViewModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (stockSaleViewModel == null)
            {
                return NotFound();
            }

            return View(stockSaleViewModel);
        }

        // POST: StockSale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockSaleViewModel = await _context.StockSaleViewModel.SingleOrDefaultAsync(m => m.Id == id);
            _context.StockSaleViewModel.Remove(stockSaleViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockSaleViewModelExists(int id)
        {
            return _context.StockSaleViewModel.Any(e => e.Id == id);
        }
    }
}
