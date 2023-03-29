using Booksy.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksyMVC.ViewModels
{
    public class UserVM
    {
        public ApplicationUser ApplicationUser { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CompanyList { get; set; }
    }
}
