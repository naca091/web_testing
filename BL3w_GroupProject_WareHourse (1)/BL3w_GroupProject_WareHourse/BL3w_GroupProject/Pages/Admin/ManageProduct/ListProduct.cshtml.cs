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

namespace BL3w_GroupProject.Pages.Admin.ManageProduct
{
    public class ListProductModel : PageModel
    {
        private readonly IProductService productService;
        public ListProductModel()
        {
            productService = new ProductService();
        }

        public IList<Product> Product { get;set; } = default!;


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
                count = productService.GetProducts()
                        .Where(a => a.ProductCode.ToUpper().Contains(SearchText.Trim().ToUpper()))
                        .Count();
                Product = productService
                    .GetProducts().Where(a => a.ProductCode.ToUpper().Contains(SearchText.Trim().ToUpper()) || a.Name.ToLower().Contains(SearchText.Trim().ToLower()))
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            } else
            {
                count = productService.GetProducts().Count();
                Product = productService.GetProducts()
                        .Skip((curentPage - 1) * pageSize).Take(pageSize)
                        .ToList();
            }
            return Page();
        }
    }
}
