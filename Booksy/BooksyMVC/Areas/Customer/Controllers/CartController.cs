using Booksy.Models;
using Booksy.Utitlities;
using BooksyMVC.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
using Stripe.Issuing;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace BooksyMVC.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        //Email sender injected in constructor
        /*public CartController( IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }*/
        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("UserId");
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


            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = ShoppingCarts,
                OrderHeader = new()
            };
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }

        public async Task<IActionResult> Summary()
        {
            var id = HttpContext.Session.GetInt32("UserId");
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

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = ShoppingCarts,
                OrderHeader = new()
            };


            //Getting ApplicationUser
            ApplicationUser user = new();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/ApplicationUsers/"+id);

                if (Res.IsSuccessStatusCode)
                {
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;

                    user = JsonConvert.DeserializeObject<ApplicationUser>(apiResponse);

                }
            }
            ShoppingCartVM.OrderHeader.ApplicationUser = user;

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;



            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SummaryPOST()
        {
            var id = HttpContext.Session.GetInt32("UserId");


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
            ShoppingCarts = ShoppingCarts.Where(c=>c.ApplicationUserId==id).ToList();

            ShoppingCartVM.ListCart = ShoppingCarts;


            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = id??0;


            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            ApplicationUser applicationUser = new();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/ApplicationUsers/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;

                    applicationUser = JsonConvert.DeserializeObject<ApplicationUser>(apiResponse);

                }
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 3)
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }


            //Add to OrderHeader
            OrderHeader orderHeaderFromApi = new();
            OrderHeader orderheadertemp = new();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                OrderHeader orderHeader = new()
                {
                    ApplicationUser = ShoppingCartVM.OrderHeader.ApplicationUser,
                    ApplicationUserId = ShoppingCartVM.OrderHeader.ApplicationUserId,
                    OrderDate = ShoppingCartVM.OrderHeader.OrderDate,
                    OrderTotal = ShoppingCartVM.OrderHeader.OrderTotal,
                    PaymentStatus = ShoppingCartVM.OrderHeader.PaymentStatus??"",
                    OrderStatus = ShoppingCartVM.OrderHeader.OrderStatus??"",
                    Name = ShoppingCartVM.OrderHeader.Name ?? "",
                    PhoneNumber = ShoppingCartVM.OrderHeader.PhoneNumber??"",
                    StreetAddress = ShoppingCartVM.OrderHeader.StreetAddress ?? "",
                    City = ShoppingCartVM.OrderHeader.City ?? "",
                    State = ShoppingCartVM.OrderHeader.State ?? "",
                    PostalCode = ShoppingCartVM.OrderHeader.PostalCode ?? ""
                };
                orderheadertemp = orderHeader;
                StringContent valuesToAdd = new StringContent(JsonConvert.SerializeObject(orderHeader),
                      Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7123/api/OrderHeaders/", valuesToAdd))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    orderHeaderFromApi = JsonConvert.DeserializeObject<OrderHeader>(apiResponse);
                }
            }
            foreach (var cart in ShoppingCartVM.ListCart)
            {

                OrderDetail orderDetail = new()
                {
                    //Product=cart.Product,
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    OrderHeader=orderheadertemp,
                    Price = cart.Price,
                    Count = cart.Count
                };
                OrderDetail orderDetailFromAPI = new();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StringContent valuesToAdd = new StringContent(JsonConvert.SerializeObject(orderDetail),
                      Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:7123/api/OrderDetails/", valuesToAdd))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        orderDetailFromAPI = JsonConvert.DeserializeObject<OrderDetail>(apiResponse);
                    }
                }
            }
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
            int MaxId = orderHeaders.Max(o => o.Id);
            HttpContext.Session.SetInt32("OrderId", MaxId);

            //Stripe Functionality
            if (applicationUser.CompanyId.GetValueOrDefault() != 0)
            {
                //stripe settings 
                var domain = "https://localhost:7233/";
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={MaxId}",
                    CancelUrl = domain + $"Customer/Cart/Index",
                };

                foreach (var item in ShoppingCartVM.ListCart)
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
                    StringContent valueToUpdate = new StringContent(JsonConvert.SerializeObject(orderheadertemp)
                     , Encoding.UTF8, "application/json");

                    var sessionId = session.Id??"";
                    var paymentIntentId = session.PaymentIntentId ?? "";
                    using (var response = await client.PutAsync("https://localhost:7123/api/ShoppingCarts/update_stripe_id/" + MaxId + "/" + sessionId+"/"+paymentIntentId, valueToUpdate))
                    {
                        var apiResponse = response.Content.ReadAsStringAsync().Result;

                        orderheaderstripe = JsonConvert.DeserializeObject<OrderHeader>(apiResponse);

                    }
                }

                //ShoppingCartVM.OrderHeader.
                //_unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            else
            {
                return RedirectToAction("OrderConfirmation", "Cart", new { id = MaxId });
            }
        }

        public async Task<IActionResult> OrderConfirmation(int id)
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
            OrderHeader orderHeader = orderHeaders.FirstOrDefault(u => u.Id == id);
           /* if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
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

                        //HttpResponseMessage Res = await client.PutAsync("https://localhost:7123/api/ShoppingCarts/increment_count/" + cartId +"/" + 1);

                        using (var response = await client.PutAsync("https://localhost:7123/api/ShoppingCarts/update_stripe_id/" + id + "/" + session.Id + "/" + session.PaymentIntentId, valueToUpdate))
                        {
                            var apiResponse = response.Content.ReadAsStringAsync().Result;

                            orderheaderstripe = JsonConvert.DeserializeObject<OrderHeader>(apiResponse);

                        }
                    }
                }
            }*/
            //_emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - Bulky Book", "<p>New Order Created</p>");
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
            //Remove Booked Products from Cart
            List<ShoppingCart> shoppingCarts = ShoppingCarts.Where(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            foreach (var item in shoppingCarts)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:7123/api/ShoppingCarts/" + item.Id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return View(id);
         }

        public async Task<IActionResult> Plus(int cartId)
        {

            ShoppingCart shoppingCart = new();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/ShoppingCarts/"+cartId);

                if (Res.IsSuccessStatusCode)
                {
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;

                    shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(apiResponse);

                }
            }



            ShoppingCart ShoppingCart = new();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent valueToUpdate = new StringContent(JsonConvert.SerializeObject(shoppingCart)
                 , Encoding.UTF8, "application/json");

                //HttpResponseMessage Res = await client.PutAsync("https://localhost:7123/api/ShoppingCarts/increment_count/" + cartId +"/" + 1);

                using (var response = await client.PutAsync("https://localhost:7123/api/ShoppingCarts/increment_count/" + cartId + "/" + 1,valueToUpdate))
                {
                    var apiResponse = response.Content.ReadAsStringAsync().Result;

                    ShoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(apiResponse);

                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {

            ShoppingCart shoppingCart = new();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("https://localhost:7123/api/ShoppingCarts/" + cartId);

                if (Res.IsSuccessStatusCode)
                {
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;

                    shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(apiResponse);

                }
            }



            ShoppingCart ShoppingCart = new();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent valueToUpdate = new StringContent(JsonConvert.SerializeObject(shoppingCart)
                 , Encoding.UTF8, "application/json");

                //HttpResponseMessage Res = await client.PutAsync("https://localhost:7123/api/ShoppingCarts/increment_count/" + cartId +"/" + 1);

                using (var response = await client.PutAsync("https://localhost:7123/api/ShoppingCarts/decrement_count/" + cartId + "/" + 1, valueToUpdate))
                {
                    var apiResponse = response.Content.ReadAsStringAsync().Result;

                    ShoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(apiResponse);

                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7123/api/ShoppingCarts/" + cartId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

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
            HttpContext.Session.SetInt32("SessionCart",
                ShoppingCarts.Where(u => u.ApplicationUserId == cartId).ToList().Count);
            return RedirectToAction(nameof(Index));
        }





        private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity <= 50)
            {
                return price;
            }
            else
            {
                if (quantity <= 100)
                {
                    return price50;
                }
                return price100;
            }
        }
    }
}
