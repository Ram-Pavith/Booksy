using Booksy.Models;
using Booksy.Utitlities;
using BooksyMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace BooksyMVC.Areas.Admin.Controllers
{
	public class OrderController : Controller
	{
		public OrderVM OrderVM { get; set; }
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Details(int orderId)
		{
			List<OrderHeader> orderHeaderListFromAPI = new();

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/OrderHeaders");

				if (Res.IsSuccessStatusCode)
				{

					var apiResponse = Res.Content.ReadAsStringAsync().Result;

					orderHeaderListFromAPI = JsonConvert.DeserializeObject<List<OrderHeader>>(apiResponse);

				}
			}
			List<OrderDetail> orderDetailListFromAPI = new();
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/OrderDetails");

				if (Res.IsSuccessStatusCode)
				{

					var apiResponse = Res.Content.ReadAsStringAsync().Result;

					orderDetailListFromAPI = JsonConvert.DeserializeObject<List<OrderDetail>>(apiResponse);

				}
			}
			OrderVM = new OrderVM()
			{
				OrderHeader = orderHeaderListFromAPI.FirstOrDefault(u => u.Id == orderId),
				OrderDetail = orderDetailListFromAPI.Where(u => u.OrderId == orderId).Select(u=>u).ToList(),
			};
			return View(OrderVM);
		}

		[ActionName("Details")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Details_PAY_NOW()
		{
			List<OrderHeader> orderHeaderListFromAPI = new();

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/OrderHeaders");

				if (Res.IsSuccessStatusCode)
				{

					var apiResponse = Res.Content.ReadAsStringAsync().Result;

					orderHeaderListFromAPI = JsonConvert.DeserializeObject<List<OrderHeader>>(apiResponse);

				}
			}
			List<OrderDetail> orderDetailListFromAPI = new();
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/OrderDetails");

				if (Res.IsSuccessStatusCode)
				{

					var apiResponse = Res.Content.ReadAsStringAsync().Result;

					orderDetailListFromAPI = JsonConvert.DeserializeObject<List<OrderDetail>>(apiResponse);

				}
			}
			OrderVM.OrderHeader = orderHeaderListFromAPI.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
			OrderVM.OrderDetail = orderDetailListFromAPI.Where(u => u.OrderId == OrderVM.OrderHeader.Id).Select(u=>u).ToList();

			//int MaxId = orderHeaderListFromAPI.Max(o => o.Id);
			//stripe settings 
			var domain = "https://localhost:44300/";
			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string>
				{
				  "card",
				},
				LineItems = new List<SessionLineItemOptions>(),
				Mode = "payment",
				SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderid={OrderVM.OrderHeader.Id}",
				CancelUrl = domain + $"admin/order/details?orderId={OrderVM.OrderHeader.Id}",
			};

			foreach (var item in OrderVM.OrderDetail)
			{

				var sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(item.Price * 100),//20.00 -> 2000
						Currency = "inr",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Product.Title
						},

					},
					Quantity = item.Count,
				};
				options.LineItems.Add(sessionLineItem);

			}

			var service = new SessionService();
			Session session = service.Create(options);
			OrderHeader orderheaderstripe = new();
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				StringContent valueToUpdate = new StringContent(JsonConvert.SerializeObject(OrderVM.OrderHeader)
				 , Encoding.UTF8, "application/json");

				var sessionId = session.Id ?? "";
				var paymentIntentId = session.PaymentIntentId ?? "";
				using (var response = await client.PutAsync("https://localhost:7123/api/ShoppingCarts/update_stripe_id/" + OrderVM.OrderHeader.Id + "/" + sessionId + "/" + paymentIntentId, valueToUpdate))
				{
					var apiResponse = response.Content.ReadAsStringAsync().Result;

					orderheaderstripe = JsonConvert.DeserializeObject<OrderHeader>(apiResponse);

				}
			}
			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303);
		}

		public async Task<IActionResult> PaymentConfirmation(int orderHeaderid)
		{
			List<OrderHeader> orderHeaders = new();
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/OrderHeaders");

				if (Res.IsSuccessStatusCode)
				{
					var apiResponse = Res.Content.ReadAsStringAsync().Result;

					orderHeaders = JsonConvert.DeserializeObject<List<OrderHeader>>(apiResponse);

				}
			}
			OrderHeader orderHeader = orderHeaders.FirstOrDefault(u => u.Id == orderHeaderid);

			if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
			{
				var service = new SessionService();
				Session session = service.Get(orderHeader.SessionId);
				//check the stripe status
				if (session.PaymentStatus.ToLower() == "paid")
				{
					orderHeader.SessionId = SD.StatusApproved;
					orderHeader.PaymentIntentId = SD.PaymentStatusApproved;
					OrderHeader orderheaderstripe = new();
					using (var client = new HttpClient())
					{
						client.DefaultRequestHeaders.Clear();
						client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						StringContent valueToUpdate = new StringContent(JsonConvert.SerializeObject(orderHeader)
						 , Encoding.UTF8, "application/json");

						using (var response = await client.PutAsync("https://localhost:7123/api/ShoppingCarts/update_stripe_id/" + orderHeaderid + "/" + session.Id + "/" + session.PaymentIntentId, valueToUpdate))
						{
							var apiResponse = response.Content.ReadAsStringAsync().Result;

							orderheaderstripe = JsonConvert.DeserializeObject<OrderHeader>(apiResponse);

						}
					}
				}
			}
			return View(orderHeaderid);
		}
/*
		[HttpPost]
		[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
		[ValidateAntiForgeryToken]
		public IActionResult UpdateOrderDetail()
		{
			var orderHEaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, tracked: false);
			orderHEaderFromDb.Name = OrderVM.OrderHeader.Name;
			orderHEaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
			orderHEaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
			orderHEaderFromDb.City = OrderVM.OrderHeader.City;
			orderHEaderFromDb.State = OrderVM.OrderHeader.State;
			orderHEaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
			if (OrderVM.OrderHeader.Carrier != null)
			{
				orderHEaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
			}
			if (OrderVM.OrderHeader.TrackingNumber != null)
			{
				orderHEaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
			}
			_unitOfWork.OrderHeader.Update(orderHEaderFromDb);
			_unitOfWork.Save();
			TempData["Success"] = "Order Details Updated Successfully.";
			return RedirectToAction("Details", "Order", new { orderId = orderHEaderFromDb.Id });
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
		[ValidateAntiForgeryToken]
		public IActionResult StartProcessing()
		{
			_unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusInProcess);
			_unitOfWork.Save();
			TempData["Success"] = "Order Status Updated Successfully.";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
		[ValidateAntiForgeryToken]
		public IActionResult ShipOrder()
		{
			var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, tracked: false);
			orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
			orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
			orderHeader.OrderStatus = SD.StatusShipped;
			orderHeader.ShippingDate = DateTime.Now;
			if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
			{
				orderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
			}
			_unitOfWork.OrderHeader.Update(orderHeader);
			_unitOfWork.Save();
			TempData["Success"] = "Order Shipped Successfully.";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CancelOrder()
		{
			List<OrderHeader> orderHeaders = new();
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/OrderHeaders");

				if (Res.IsSuccessStatusCode)
				{
					var apiResponse = Res.Content.ReadAsStringAsync().Result;

					orderHeaders = JsonConvert.DeserializeObject<List<OrderHeader>>(apiResponse);

				}
			}
			var orderHeader = orderHeaders.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
			if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
			{
				var options = new RefundCreateOptions
				{
					Reason = RefundReasons.RequestedByCustomer,
					PaymentIntent = orderHeader.PaymentIntentId
				};

				var service = new RefundService();
				Refund refund = service.Create(options);
				using (var client = new HttpClient())
				{
					client.DefaultRequestHeaders.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					StringContent valueToUpdate = new StringContent(JsonConvert.SerializeObject(orderHeader)
					 , Encoding.UTF8, "application/json");

					//HttpResponseMessage Res = await client.PutAsync("https://localhost:7123/api/ShoppingCarts/increment_count/" + cartId +"/" + 1);

					using (var response = await client.PutAsync("https://localhost:7123/api/ShoppingCarts/update_stripe_id/" + id + "/" + session.Id + "/" + session.PaymentIntentId, valueToUpdate))
					{
						var apiResponse = response.Content.ReadAsStringAsync().Result;

						orderheaderstripe = JsonConvert.DeserializeObject<OrderHeader>(apiResponse);

					}

					_unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
			}
			else
			{
				_unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
			}
			_unitOfWork.Save();

			TempData["Success"] = "Order Cancelled Successfully.";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}
*/
		#region API CALLS
		[HttpGet]
		public async Task<IActionResult> GetAll(string status)
		{
			IEnumerable<OrderHeader> orderHeaders;
			IEnumerable<OrderHeader> orderHeadersFromAPI = new List<OrderHeader>(); ;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/OrderHeaders");

				if (Res.IsSuccessStatusCode)
				{
					var apiResponse = Res.Content.ReadAsStringAsync().Result;

					orderHeadersFromAPI = JsonConvert.DeserializeObject<List<OrderHeader>>(apiResponse);

				}
			}

			if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
			{
				
				orderHeaders = orderHeadersFromAPI;
			}
			else
			{
				var claimsIdentity = (ClaimsIdentity)User.Identity;
				var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
				orderHeaders = orderHeadersFromAPI.Where(u => u.ApplicationUserId == Convert.ToInt32(claim.Value)).ToList();
			}

			switch (status)
			{
				case "pending":
					orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
					break;
				case "inprocess":
					orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
					break;
				case "completed":
					orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
					break;
				case "approved":
					orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
					break;
				default:
					break;
			}


			return Json(new { data = orderHeaders });
		}
		#endregion
	}
}
