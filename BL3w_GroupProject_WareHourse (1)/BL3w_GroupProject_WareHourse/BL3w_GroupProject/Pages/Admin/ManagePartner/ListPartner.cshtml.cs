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

namespace BL3w_GroupProject.Pages.Admin.ManagePartner
{
    public class ListPartnerModel : PageModel
    {
        private readonly IPartnerService partnerService;
        private const int PageSize = 5;
        public ListPartnerModel()
        {
            partnerService = new PartnerService();
        }

        public IList<Partner> Partner { get;set; } = default!;

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
                count = partnerService.GetPartners()
                    .Where(a => a.PartnerCode.ToUpper().Equals(SearchText.ToUpper().Trim()) || a.Name.ToLower().Contains(SearchText.ToLower().Trim()))
                    .Count();
                Partner = partnerService.GetPartners()
                    .Where(a => a.PartnerCode.ToUpper().Equals(SearchText.ToUpper().Trim()) || a.Name.ToLower().Contains(SearchText.ToLower().Trim()))
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            } else
            {
                count = partnerService.GetPartners().Count();
                Partner = partnerService.GetPartners()
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }

            return Page();
        }
    }
}
