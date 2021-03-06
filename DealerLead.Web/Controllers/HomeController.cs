using System;
using System.Diagnostics;
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
            Guid oid = Authenticate.GetOid(this.User);
            var dealerLeadUser = await _context.DealerLeadUser.FirstOrDefaultAsync(x => x.AzureADId.Equals(oid));
            if (dealerLeadUser == null) return NotFound();
            return View(dealerLeadUser);
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
    }
}
