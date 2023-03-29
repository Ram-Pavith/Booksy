using Booksy.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BooksyMVC.Areas.Admin.Controllers
{
	public class CompanyController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		//GET
		public async Task<IActionResult> Upsert(int? id)
		{
			Company company = new();

			if (id == null || id == 0)
			{
				return View(company);
			}
			else
			{
				Company companyFromAPI = new();

				using (var client = new HttpClient())
				{
					client.DefaultRequestHeaders.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/Companies/" + id);

					if (Res.IsSuccessStatusCode)
					{
						var apiResponse = Res.Content.ReadAsStringAsync().Result;

						companyFromAPI = JsonConvert.DeserializeObject<Company>(apiResponse);

					}
				}
				return View(companyFromAPI);
			}
		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Upsert(Company obj, IFormFile? file)
		{

			if (ModelState.IsValid)
			{

				if (obj.Id == 0)
				{
					Company CompanyFromApi = new Company();
					using (var httpClient = new HttpClient())
					{
						StringContent valuesToAdd = new StringContent(JsonConvert.SerializeObject(obj),
					  Encoding.UTF8, "application/json");

						using (var response = await httpClient.PostAsync("https://localhost:7123/api/Companies/", valuesToAdd))
						{
							string apiResponse = await response.Content.ReadAsStringAsync();
							CompanyFromApi = JsonConvert.DeserializeObject<Company>(apiResponse);
						}
					}
					TempData["success"] = "Company created successfully";
				}
				else
				{
					Company CompanyFromAPI = new();
					//string productId = TempData["ProductId"].ToString();
					using (var httpClient = new HttpClient())
					{
						int id = obj.Id;
						StringContent valueToUpdate = new StringContent(JsonConvert.SerializeObject(obj)
				 , Encoding.UTF8, "application/json");
						using (var response = await httpClient.PutAsync("https://localhost:7123/api/Companies/" + id, valueToUpdate))
						{
							string apiResponse = await response.Content.ReadAsStringAsync();
							CompanyFromAPI = JsonConvert.DeserializeObject<Company>(apiResponse);
						}
					}
					TempData["success"] = "Company updated successfully";
				}
				return RedirectToAction("Index");
			}
			return View(obj);
		}



		#region API CALLS
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			List<Company> companyListFromAPI = new();

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/Companies");

				if (Res.IsSuccessStatusCode)
				{

					var apiResponse = Res.Content.ReadAsStringAsync().Result;

					companyListFromAPI = JsonConvert.DeserializeObject<List<Company>>(apiResponse);

				}
			}
			return Json(new { data = companyListFromAPI });
		}

		//POST
		[HttpDelete]
		public async Task<IActionResult> Delete(int? id)
		{
			Company companyFromAPI = new();

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/Companies/" + id);

				if (Res.IsSuccessStatusCode)
				{
					var apiResponse = Res.Content.ReadAsStringAsync().Result;

					companyFromAPI = JsonConvert.DeserializeObject<Company>(apiResponse);

				}
			}
			var obj = companyFromAPI;
			if (obj == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

			//Remove Product
			using (var httpClient = new HttpClient())
			{
				using (var response = await httpClient.DeleteAsync("https://localhost:7123/api/Companies/" + id))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
				}
			}
			return Json(new { success = true, message = "Delete Successful" });

		}
		#endregion
	}
}
