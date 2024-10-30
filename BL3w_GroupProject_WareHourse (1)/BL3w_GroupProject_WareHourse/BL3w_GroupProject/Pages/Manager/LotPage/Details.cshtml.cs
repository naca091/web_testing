﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;

namespace BL3w_GroupProject.Pages.Manager.LotPage
{
    public class DetailsModel : PageModel
    {
        private readonly ILotService _context;

        public DetailsModel(ILotService context)
        {
            _context = context;
        }

      public Lot Lot { get; set; } = default!;
        public List<LotDetail> LotDetails { get; set; } = new List<LotDetail>();

        public IActionResult OnGet(int id)
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
            if (id == null)
            {
                return NotFound();
            }
            if (id <= 0)
            {
                return NotFound();
            }

            Lot = _context.GetLotById(id);
            if (Lot == null)
            {
                return NotFound();
            }
            else
            {
                var lotDetails = _context.GetListLotDetailByLotID(id);

                if (lotDetails != null)
                {
                    LotDetails.AddRange(lotDetails);
                }
            }
            return Page();
        }
    }
}
