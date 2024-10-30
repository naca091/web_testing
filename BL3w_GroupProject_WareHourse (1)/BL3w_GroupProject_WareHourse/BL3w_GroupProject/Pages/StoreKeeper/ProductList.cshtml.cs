using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;

namespace BL3w_GroupProject.Pages.StoreKeeper
{
    public class ProductListModel : PageModel
    {
        private readonly IProductService _productService;

        public ProductListModel()
        {
            _productService = new ProductService();
        }

        public IList<Product> Product { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }

        [BindProperty(SupportsGet = true)]
        public int curentPage { get; set; } = 1;
        public int pageSize { get; set; } = 5;
        public int count { get; set; }
        public int totalPages => (int)Math.Ceiling(Decimal.Divide(count, pageSize));

        public IActionResult OnGetAsync()
        {
            if (HttpContext.Session.GetString("account") is null)
            {
                return RedirectToPage("/Login");
            }

            var role = HttpContext.Session.GetString("account");

            if (role != "storekeeper")
            {
                return RedirectToPage("/Login");
            }

            if (SearchText != null)
            {
                count = _productService.GetProducts()
                    .Where(P => P.Name.ToLower().Contains(SearchText.ToLower()) || P.ProductCode.ToLower().Contains(SearchText.ToLower()))
                    .Count();

                Product = _productService.GetProducts()
                    .Where(P => P.Name.ToLower().Contains(SearchText.ToLower()) || P.ProductCode.ToLower().Contains(SearchText.ToLower()))
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            } else
            {
                count = _productService.GetProducts().Count();
                Product = _productService.GetProducts()
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }
            return Page();
        }
    }
}
