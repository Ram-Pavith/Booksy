using Booksy.Models;
using BooksyMVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
namespace BooksyMVC.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IWebHostEnvironment hostEnvironment){
            _hostEnvironment = hostEnvironment;
        }
        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //GET
        public async Task<IActionResult> Upsert(int? id)
        {//Category list
            List<Category> categoryFromAPI = new List<Category>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/Categories");

                if (Res.IsSuccessStatusCode)
                {
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;

                    categoryFromAPI = JsonConvert.DeserializeObject<List<Category>>(apiResponse);

                }
            }
            //CoverType List
            List<CoverType> CoverTypeFromAPI = new List<CoverType>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/CoverTypes");

                if (Res.IsSuccessStatusCode)
                {
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;

                    CoverTypeFromAPI = JsonConvert.DeserializeObject<List<CoverType>>(apiResponse);

                }
            }
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = categoryFromAPI.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = CoverTypeFromAPI.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                Product ProductFromAPI = new Product();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/Products/" + id);

                    if (Res.IsSuccessStatusCode)
                    {
                        var apiResponse = Res.Content.ReadAsStringAsync().Result;

                        ProductFromAPI = JsonConvert.DeserializeObject<Product>(apiResponse);
                    }
                    ProductVM productVMGet = new();
                    productVMGet.Product = ProductFromAPI ;
                    productVM.Product = ProductFromAPI;
                    return View(productVM);
                }
            }


        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }
                if (obj.Product.Id == 0)
                {
                    //Add Product
                    Product ProductFromApi = new Product();
                    using (var httpClient = new HttpClient())
                    {
                        StringContent valuesToAdd = new StringContent(JsonConvert.SerializeObject(obj.Product),
                      Encoding.UTF8, "application/json");

                        using (var response = await httpClient.PostAsync("https://localhost:7123/api/Products/", valuesToAdd))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            ProductFromApi = JsonConvert.DeserializeObject<Product>(apiResponse);
                        }
                    }
                }
                else
                {
                    //update product
                    Product ProductFromAPI = new();
                    //string productId = TempData["ProductId"].ToString();
                    using (var httpClient = new HttpClient())
                    {
                        int id = obj.Product.Id;
                        StringContent valueToUpdate = new StringContent(JsonConvert.SerializeObject(obj.Product)
                 , Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PutAsync("https://localhost:7123/api/Products/" + id, valueToUpdate))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            ProductFromAPI = JsonConvert.DeserializeObject<Product>(apiResponse);
                        }
                    }
                }
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
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
            return Json(new { data = productListFromAPI });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {

            Product productListFromAPI = new();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/Products/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;

                    productListFromAPI = JsonConvert.DeserializeObject<Product>(apiResponse);

                }
            }
            var obj = productListFromAPI;
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            //Remove Product
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7123/api/Products/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return Json(new { success = true, message = "Delete Successful" });

        }

    }
}
