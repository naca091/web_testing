using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;
using System.Drawing.Printing;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace BL3w_GroupProject.Pages.Admin.ManageStockOut
{
    public class ListStockOutModel : PageModel
    {
        private readonly IStockOutService stockOutService;
        private const int PageSize = 5;
        public ListStockOutModel()
        {
            stockOutService = new StockOutService();
        }

        public IList<StockOut> StockOut { get;set; } = default!;


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
            var stockOutList = stockOutService.GetStockOuts();

            if(SearchText == null)
            {
                count = stockOutService.GetStockOuts().Count();
                StockOut = stockOutService.GetStockOuts()
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            } else
            {
                count = stockOutService.GetStockOuts()
                    .Where(a => a.Account.AccountCode.ToUpper().Contains(SearchText.Trim().ToUpper()) || a.Partner.Name.ToLower().Contains(SearchText.Trim().ToLower()))
                    .Count();
                StockOut = stockOutService.GetStockOuts()
                    .Where(a => a.Account.AccountCode.ToUpper().Contains(SearchText.Trim().ToUpper()) || a.Partner.Name.ToLower().Contains(SearchText.Trim().ToLower()))
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }
            return Page();
        }
    }
}
