using Booksy.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BooksyMVC.Areas.Admin.Controllers
{
	public class CoverTypeController : Controller
	{
		public async Task<IActionResult> Index()
		{
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
				return View(CoverTypeFromAPI);
			}
		}


		// GET: CoverTypeController/Create
		public IActionResult Create()
		{
			return View();
		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CoverType CoverTypeFromMVC)
		{
			CoverType CoverTypeFromApi = new CoverType();
			using (var httpClient = new HttpClient())
			{
				StringContent valuesToAdd = new StringContent(JsonConvert.SerializeObject(CoverTypeFromMVC),
			  Encoding.UTF8, "application/json");

				using (var response = await httpClient.PostAsync("https://localhost:7123/api/CoverTypes/", valuesToAdd))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					CoverTypeFromApi = JsonConvert.DeserializeObject<CoverType>(apiResponse);
				}
			}
			return RedirectToAction("Index");
		}

		// GET: CoverTypeController/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			TempData["CoverTypeId"] = id;
			CoverType CoverTypeFromAPI = new CoverType();
			using (var httpClient = new HttpClient())
			{
				using (var response = await httpClient.GetAsync("https://localhost:7123/api/CoverTypes/" + id))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					CoverTypeFromAPI = JsonConvert.DeserializeObject<CoverType>(apiResponse);
				}
			}
			return View(CoverTypeFromAPI);
		}

		// POST: CoverTypeController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(CoverType obj)
		{
			CoverType CoverTypeFromAPI = new CoverType();
			string foodId = TempData["CoverTypeId"].ToString();
			using (var httpClient = new HttpClient())
			{
				int id = obj.Id;
				StringContent valueToUpdate = new StringContent(JsonConvert.SerializeObject(obj)
		 , Encoding.UTF8, "application/json");
				using (var response = await httpClient.PutAsync("https://localhost:7123/api/CoverTypes/" + id, valueToUpdate))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					CoverTypeFromAPI = JsonConvert.DeserializeObject<CoverType>(apiResponse);
				}
			}
			return RedirectToAction("Index");
		}

		// GET: CoverTypeController/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			TempData["CoverTypeId"] = id;
			CoverType CoverTypeFromAPI = new CoverType();
			using (var httpClient = new HttpClient())
			{
				using (var response = await httpClient.GetAsync("https://localhost:7123/api/CoverTypes/" + id))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					CoverTypeFromAPI = JsonConvert.DeserializeObject<CoverType>(apiResponse);
				}
			}
			return View(CoverTypeFromAPI);
		}

		// POST: CoverTypeController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id, IFormCollection collection)
		{
			string foodId = TempData["CoverTypeId"].ToString();
			using (var httpClient = new HttpClient())
			{
				using (var response = await httpClient.DeleteAsync("https://localhost:7123/api/CoverTypes/" + foodId))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
				}
			}
			return RedirectToAction("Index");
		}
	}
}
