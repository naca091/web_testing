using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;

namespace BL3w_GroupProject.Pages.Admin.ManageStockOutDetail
{
    public class ListStockOutDetailModel : PageModel
    {
        private readonly IStockOutService stockOutService;
        private const int PageSize = 5;

        public ListStockOutDetailModel()
        {
            stockOutService = new StockOutService();
        }

        public IList<StockOutDetail> StockOutDetail { get; set; } = default!;


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


            if (StockOutDetail == null)
            {
                count = stockOutService.GetStockOutsDetail().Count();
                StockOutDetail = stockOutService.GetStockOutsDetail()
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }
            else
            {
                count = stockOutService.GetStockOutsDetail()
                    .Where(a => a.Product.ProductCode.Equals(SearchText))
                    .Count();
                StockOutDetail = stockOutService.GetStockOutsDetail()
                    .Where(a => a.Product.ProductCode.Equals(SearchText))
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }

            return Page();
        }
    }
}
