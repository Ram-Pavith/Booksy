using Booksy.Models;
using BooksyMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BooksyMVC.Areas.Customer.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([Bind("EmailAddress,Password")] ApplicationUser applicationUser)
        {
            ApplicationUser user = new();
            List<ApplicationUser> users = new List<ApplicationUser>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/ApplicationUsers");

                if (Res.IsSuccessStatusCode)
                {
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;

                    users = JsonConvert.DeserializeObject<List<ApplicationUser>>(apiResponse);

                }
            }
            user = users.SingleOrDefault(u => u.EmailAddress == applicationUser.EmailAddress && u.Password == applicationUser.Password);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Name);
                HttpContext.Session.SetString("EmailId", user.EmailAddress);
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Admin", user.isAdmin ? "Admin" : "User");
                return RedirectToAction("Index","Home");
            }
            else
                return View();
        }

        public async Task<IActionResult> Register()
        {
            List<Company> companyFromAPI = new List<Company>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/Companies");

                if (Res.IsSuccessStatusCode)
                {
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;

                    companyFromAPI = JsonConvert.DeserializeObject<List<Company>>(apiResponse);

                }
            }

            UserVM userVM = new()
            {
                ApplicationUser = new(),
                CompanyList = companyFromAPI.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            return View(userVM);
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserVM obj)
        {
            if (obj.ApplicationUser.Id == 0)
            {
                //Add Product
                ApplicationUser userFromApi = new();
                using (var httpClient = new HttpClient())
                {
                    StringContent valuesToAdd = new StringContent(JsonConvert.SerializeObject(obj.ApplicationUser),
                  Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:7123/api/ApplicationUsers/", valuesToAdd))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        userFromApi = JsonConvert.DeserializeObject<ApplicationUser>(apiResponse);
                    }
                }
            }
            else
            {
                //update product
                ApplicationUser userFromAPI = new();
                //string productId = TempData["ProductId"].ToString();
                using (var httpClient = new HttpClient())
                {
                    int id = obj.ApplicationUser.Id;
                    StringContent valueToUpdate = new StringContent(JsonConvert.SerializeObject(obj.ApplicationUser)
             , Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:7123/api/ApplicationUsers/" + id, valueToUpdate))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        userFromAPI = JsonConvert.DeserializeObject<ApplicationUser>(apiResponse);
                    }
                }
            }
            TempData["success"] = "Product created successfully";

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");

        }
    }
}
