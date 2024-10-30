using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace BL3w_GroupProject.Pages.Admin.ManageLot
{
    public class ListLotModel : PageModel
    {
        private readonly ILotService lotService;
        private const int PageSize = 5;

        public ListLotModel()
        {
            lotService = new LotService();
        }

        public IList<Lot> Lot { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int curentPage { get; set; } = 1;
        public int pageSize { get; set; } = 5;
        public int count { get; set; }
        public int totalPages => (int)Math.Ceiling(Decimal.Divide(count, pageSize));


        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }

        public IActionResult OnGet(int? pageIndex)
        {
            if (HttpContext.Session.GetString("account") is null)
            {
                return RedirectToPage("/Login");
            }

            var role = HttpContext.Session.GetString("account");

            if (role != "admin")
            {
                return RedirectToPage("/Login");
            }


            if(SearchText != null)
            {
                count = lotService.GetAllLots()
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                .Count();
                Lot = lotService.GetAllLots()
                    .Where(a => a.LotCode.ToUpper().Contains(SearchText.Trim().ToUpper()))
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            } else
            {
                count = lotService.GetAllLots().Count();
                Lot = lotService.GetAllLots()
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }

            return Page();
        }
    }
}
