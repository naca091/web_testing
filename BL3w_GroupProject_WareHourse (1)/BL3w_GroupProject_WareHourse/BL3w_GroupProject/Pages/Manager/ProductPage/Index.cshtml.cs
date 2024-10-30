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

namespace BL3w_GroupProject.Pages.Manager.ProductPage
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel()
        {
            _productService = new ProductService();
        }

        public IList<Product> Product { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }

        [BindProperty(SupportsGet = true)]
        public int curentPage { get; set; } = 1;
        public int pageSize { get; set; } = 5;
        public int count { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public IActionResult OnGetAsync(string searchText)
        {
            if (HttpContext.Session.GetString("account") is null)
            {
                return RedirectToPage("/Login");
            }

            var role = HttpContext.Session.GetString("account");

            if (role != "manager")
            {
                return RedirectToPage("/Login");
            }
            SearchText = searchText;
            var products = _productService.GetProducts();

            if (!string.IsNullOrEmpty(searchText))
            {
                products = products.Where(p =>
                    p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    p.ProductCode.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            TotalRecords = products.Count();

            Product = products.Skip((curentPage - 1) * pageSize)
                              .Take(pageSize)
            .ToList();

            TotalPages = (int)System.Math.Ceiling((double)TotalRecords / pageSize);
            curentPage = curentPage;
            return Page();
        }
    }
}
