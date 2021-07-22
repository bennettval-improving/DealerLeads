using System;
using System.Linq;
using System.Threading.Tasks;
using DealerLead.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DealerLead.Web.Controllers
{
    public class DealershipsController : Controller
    {
        private readonly DealerLeadDbContext _context;

        public DealershipsController(DealerLeadDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            var dealerships = await _context.Dealership.Where(x => x.CreatingUserId == id).ToListAsync();
            return View(dealerships);
        }

        public IActionResult Create()
        {
            ViewData["State"] = new SelectList(_context.SupportedState, "Abbreviation", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dealership dealership)
        {
            var oid = Authenticate.GetOid(this.User);
            var user = await _context.DealerLeadUser.FirstOrDefaultAsync(x => x.AzureADId.Equals(oid));
            if (user == null || user.Id < 0 || !ModelState.IsValid) // probably shouldn't handle these scenarios the same way
            {
                ViewData["State"] = new SelectList(_context.SupportedState, "Abbreviation", "Name");
                return View(dealership);
            }

            dealership.CreatingUserId = user.Id;
            _context.Dealership.Add(dealership);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { id = dealership.CreatingUserId }); 
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue) return NotFound();

            var dealership = await _context.Dealership.FindAsync(id);
            if (dealership == null) return NotFound();

            ViewData["State"] = new SelectList(_context.SupportedState, "Abbreviation", "Name");
            return View(dealership);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dealership dealership)
        {
            if (dealership == null) return BadRequest($"{nameof(dealership)} cannot be null.");
            if (id != dealership.Id) return BadRequest($"{nameof(id)} does not match {nameof(dealership)} id.");
            if (id < 1) return BadRequest("Invalid dealership Id.");

            if (ModelState.IsValid)
            {
                dealership.ModifyDate = DateTime.Now;
                _context.Dealership.Update(dealership);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { id = dealership.CreatingUserId });
            }
            return View(dealership);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue || id.Value < 1) return NotFound();

            var dealership = await _context.Dealership.FindAsync(id.Value);
            if (dealership == null) return NotFound();

            return View(dealership);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dealership = await _context.Dealership.FindAsync(id);
            return View(dealership);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id < 1) return BadRequest("Invalid Dealership Id.");

            var dealership = await _context.Dealership.FindAsync(id);
            if (dealership == null) return NotFound();

            var userId = dealership.CreatingUserId;

            _context.Dealership.Remove(dealership);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { id = userId });
        }
    }
}
