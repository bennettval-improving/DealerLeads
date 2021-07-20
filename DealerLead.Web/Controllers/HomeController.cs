using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DealerLead.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DealerLead.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DealerLeadDbContext _context;

        public HomeController(ILogger<HomeController> logger, DealerLeadDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            Guid oid = GetOid();
            var dealerLeadUser = await _context.DealerLeadUser.FirstOrDefaultAsync(x => x.AzureADId.Equals(oid));
            return View(dealerLeadUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create()
        {
            var oid = GetOid();
            if (oid.Equals(Guid.Empty)) return RedirectToAction(nameof(Index));

            _context.Add(new DealerLeadUser { AzureADId = oid });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private Guid GetOid()
        {
            var user = this.User;
            if (user != null && user.Claims != null && user.Claims.Count() > 0)
            {
                var oid = user.Claims.FirstOrDefault(x => x.Type.Equals("http://schemas.microsoft.com/identity/claims/objectidentifier")).Value;
                return Guid.Parse(oid);
            }
            else
                return Guid.Empty;
        }
    }
}
