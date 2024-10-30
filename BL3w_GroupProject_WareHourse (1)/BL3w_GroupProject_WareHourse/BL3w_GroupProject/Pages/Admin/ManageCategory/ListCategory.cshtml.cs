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

namespace BL3w_GroupProject.Pages.Admin.ManageCategory
{
    public class ListCategoryModel : PageModel
    {
        private readonly ICategoryService categoryService;

        public ListCategoryModel()
        {
            categoryService = new CategoryService();
        }

        public IList<Category> Category { get;set; } = default!;


        [BindProperty(SupportsGet = true)]
        public int curentPage { get; set; } = 1;
        public int pageSize { get; set; } = 4;
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
                Category = categoryService.GetCategories()
                    .Where(a => a.CategoryCode.ToUpper().Contains(SearchText.Trim().ToUpper()) || a.Name.ToLower().Contains(SearchText.Trim().ToLower()))
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }
            else
            {
                count = categoryService.GetCategories().Count();
                Category = categoryService.GetCategories()
                            .Skip((curentPage - 1) * pageSize).Take(pageSize)
                            .ToList();
            }
            return Page();
        }
    }
}
