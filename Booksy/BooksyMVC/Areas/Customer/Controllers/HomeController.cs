using Booksy.Models;
using Booksy.Utitlities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace BooksyMVC.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
			List<Product> productListFromAPI = new();

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/Products");

				if (Res.IsSuccessStatusCode)
				{
					var apiResponse = Res.Content.ReadAsStringAsync().Result;

					productListFromAPI = JsonConvert.DeserializeObject<List<Product>>(apiResponse);

				}
			}
            return View(productListFromAPI);
		}

        public async Task<IActionResult> Details(int productId)
        {
			Product productListFromAPI = new();

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/Products/" + productId);

				if (Res.IsSuccessStatusCode)
				{
					var apiResponse = Res.Content.ReadAsStringAsync().Result;

					productListFromAPI = JsonConvert.DeserializeObject<Product>(apiResponse);

				}
			}
			ShoppingCart cartObj = new()
			{
				Count = 1,
				ProductId = productId,
				Product = productListFromAPI,
			};

			return View(cartObj);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = HttpContext.Session.GetInt32("UserId")??0;
            List<ShoppingCart> ShoppingCarts = new();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/ShoppingCarts");

                if (Res.IsSuccessStatusCode)
                {
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;

                    ShoppingCarts = JsonConvert.DeserializeObject<List<ShoppingCart>>(apiResponse);

                }
            }

            ShoppingCart cartFromDb = ShoppingCarts.FirstOrDefault(
                u => u.ApplicationUserId == shoppingCart.ApplicationUserId && u.ProductId == shoppingCart.ProductId);


            if (cartFromDb == null)
            {

                //Add product to ShoppingCart
                ShoppingCart shoppingCartFromAPI = new();
                using (var httpClient = new HttpClient())
                {
                    StringContent valuesToAdd = new StringContent(JsonConvert.SerializeObject(shoppingCart),
                  Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:7123/api/ShoppingCarts/", valuesToAdd))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        shoppingCartFromAPI = JsonConvert.DeserializeObject<ShoppingCart>(apiResponse);
                    }
                }
                HttpContext.Session.SetInt32("SessionCart",
                    ShoppingCarts.Where(u => u.ApplicationUserId == shoppingCart.ApplicationUserId).ToList().Count);
            }
            else
            {
                //ShoppingCarts.IncrementCount(cartFromDb, shoppingCart.Count);
                shoppingCart.Count += cartFromDb.Count;
                ShoppingCart ShoppingCartFromAPI = new();
                using (var httpClient = new HttpClient())
                {
                    int id = cartFromDb.Id;
                    StringContent valueToUpdate = new StringContent(JsonConvert.SerializeObject(shoppingCart)
             , Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:7123/api/ShoppingCarts/" + id, valueToUpdate))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        ShoppingCartFromAPI = JsonConvert.DeserializeObject<ShoppingCart>(apiResponse);
                    }
                }
            }


            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}