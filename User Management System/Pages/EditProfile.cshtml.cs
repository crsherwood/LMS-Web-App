using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lightaplusplus.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Lightaplusplus.Pages
{
    public class EditProfileModel : PageModel
    {
        private readonly Lightaplusplus.Data.Lightaplusplus_SystemContext _context;

        public EditProfileModel(Lightaplusplus.Data.Lightaplusplus_SystemContext context)
        {
            _context = context;
        }

        public Users Users { get; set; }

        [BindProperty]
        public int id { get; set; }

        [BindProperty, Required]
        public string Firstname { get; set; }

        [BindProperty, Required]
        public string Lastname { get; set; }

        [MinimumAge(18)]
        [BindProperty, Required(ErrorMessage = "A birthday is required")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [BindProperty]
        public string Phonenumber { get; set; }

        [BindProperty]
        public string Addressline1 { get; set; }

        [BindProperty]
        public string Addressline2 { get; set; }

        [BindProperty]
        public string Addresscity { get; set; }

        [BindProperty]
        public string Addressstate { get; set; }

        [BindProperty]
        public int Addresszip { get; set; }

        [BindProperty]
        public string phoneErrorMessage { get; set; }

        [BindProperty]
        public string zipErrorMessage { get; set; }

        [BindProperty]
        public string Bio { get; set; }

        [BindProperty]
        public List<UserLinks> Links { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Users = await _context.Users.FirstOrDefaultAsync(m => m.ID == id);

            if (Users == null)
            {
                return NotFound();
            }

            Firstname = Users.firstname ;
            Lastname = Users.lastname;
            Birthday = Users.birthday;
            Phonenumber = Users.phonenumber;
            Addressline1 = Users.addressline1;
            Addressline2 = Users.addressline2;
            Addresscity = Users.addresscity;
            Addressstate = Users.addressstate;
            Addresszip = Users.addresszip;
            Bio = Users.bio;

            Links = _context.UserLinks.Where(u => u.UserId == (int)id).ToList();

            while(Links.Count < 3)
            {
                var link = new UserLinks();
                link.UserId = (int)id;
                Links.Add(link);
            }

            this.id = (int)id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            bool notValid = false;
            Users = await _context.Users.FirstOrDefaultAsync(m => m.ID == id);
            Users.firstname = Firstname;
            Users.lastname = Lastname;
            Users.birthday = Birthday;
            Users.phonenumber = Phonenumber;
            Users.addressline1 = Addressline1;
            Users.addressline2 = Addressline2;
            Users.addresscity = Addresscity;
            Users.addressstate = Addressstate;
            Users.addresszip = Addresszip;
            Users.bio = Bio;

            foreach (var link in Links)
            {
                if (link.link != null)
                {
                    link.UserId = Users.ID;
                    link.User = Users;
                    Users.Links.Add(link);
                }
            }

            if (!Regex.IsMatch(Addresszip.ToString(), "[\\d-]{5,}") && Addresszip.ToString() != "" && Addresszip.ToString() != "0")
            {
                zipErrorMessage = "Invalid Zipcode";
                notValid = true;
            }
            else
            {
                // if they do match set message to empty
                zipErrorMessage = string.Empty;
            }

            if (Phonenumber != null && !Regex.IsMatch(Phonenumber, "^(\\d{10,}|[0 - 9 -]{10,}|[0 - 9\\.]{10,}|[0 - 9\\s]{10,}$)"))
            {
 
                phoneErrorMessage = "Invalid Phone Number.";
                notValid = true;
            }
            else
            {
                // if they do match set message to empty
                phoneErrorMessage = string.Empty;
            }

            if (notValid)
            {
                notValid = false;
                return Page();
            }
            

            _context.Attach(Users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(Users.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Profile", new { id = id });
        }

        //public async Task OnPostAddLink()
        //{

        //}

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}
