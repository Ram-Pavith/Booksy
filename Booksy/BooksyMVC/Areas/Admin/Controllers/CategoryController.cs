using Booksy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BooksyMVC.Areas.Admin.Controllers
{
	public class CategoryController : Controller
	{
		// GET: CategoryController
		public async Task<IActionResult> Index()
		{
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
				return View(categoryFromAPI);
			}
		}


		// GET: CategoryController/Create
		public IActionResult Create()
		{
			return View();
		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Category categoryFromMVC)
		{
			Category categoryFromApi = new Category();
			using (var httpClient = new HttpClient())
			{
				StringContent valuesToAdd = new StringContent(JsonConvert.SerializeObject(categoryFromMVC),
			  Encoding.UTF8, "application/json");

				using (var response = await httpClient.PostAsync("https://localhost:7123/api/Categories/", valuesToAdd))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					categoryFromApi = JsonConvert.DeserializeObject<Category>(apiResponse);
				}
			}
			return RedirectToAction("Index");
		}

		// GET: CategoryController/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			TempData["CategoryId"] = id;
			Category CategoryFromAPI = new Category();
			using (var httpClient = new HttpClient())
			{
				using (var response = await httpClient.GetAsync("https://localhost:7123/api/Categories/" + id))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					CategoryFromAPI = JsonConvert.DeserializeObject<Category>(apiResponse);
				}
			}
			return View(CategoryFromAPI);
		}

		// POST: CategoryController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Category obj)
		{
			Category CategoryFromAPI = new Category();
			string foodId = TempData["CategoryId"].ToString();
			using (var httpClient = new HttpClient())
			{
				int id = obj.Id;
				StringContent valueToUpdate = new StringContent(JsonConvert.SerializeObject(obj)
		 , Encoding.UTF8, "application/json");
				using (var response = await httpClient.PutAsync("https://localhost:7123/api/Categories/" + id, valueToUpdate))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					CategoryFromAPI = JsonConvert.DeserializeObject<Category>(apiResponse);
				}
			}
			return RedirectToAction("Index");
		}

		// GET: CategoryController/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			TempData["CategoryId"] = id;
			Category CategoryFromAPI = new Category();
			using (var httpClient = new HttpClient())
			{
				using (var response = await httpClient.GetAsync("https://localhost:7123/api/Categories/" + id))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					CategoryFromAPI = JsonConvert.DeserializeObject<Category>(apiResponse);
				}
			}
			return View(CategoryFromAPI);
		}

		// POST: CategoryController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id, IFormCollection collection)
		{
			string foodId = TempData["CategoryId"].ToString();
			using (var httpClient = new HttpClient())
			{
				using (var response = await httpClient.DeleteAsync("https://localhost:7123/api/Categories/" + foodId))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
				}
			}
			return RedirectToAction("Index");
		}
	}
}
