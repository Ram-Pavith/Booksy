using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksyAPI.Data;
using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

namespace BooksyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHeadersController : ControllerBase
    {
/*        private readonly IOrderHeaderService<OrderHeader> _service;

        public OrderHeadersController(IOrderHeaderService<OrderHeader> service)
        {
            _service = service;
        }
*/
        private readonly IUnitOfWorkRepo _unitOfWork;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(OrderHeadersController));


        public OrderHeadersController(IUnitOfWorkRepo unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/OrderHeaders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderHeader>>> GetOrderHeaders()
        {
            try
            {
                _log4net.Info("get order header is being called");
                //var companies = await _service.GetOrderHeaders();
                var companies = await _unitOfWork.OrderHeader.GetOrderHeaders();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _log4net.Error(ex.Message,ex);
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderHeader>> GetOrderHeader(int id)
        {

            try
            {
                _log4net.Info("get order header for "+id+" is being called");
                //var OrderHeader = await _service.GetById(id);
                var OrderHeader = await _unitOfWork.OrderHeader.GetById(id);

                if (OrderHeader == null)
                {
                    return NotFound();
                }

                return Ok(OrderHeader);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderHeader(int id, OrderHeader OrderHeader)
        {
            if (id != OrderHeader.Id)
            {
                return BadRequest();
            }
            if (!OrderHeaderExists(id))
            {
                return NotFound();
            }
            try
            {
                _log4net.Info("put order header for"+id+" is being called");
                //await _service.Update(OrderHeader);
                await _unitOfWork.OrderHeader.Update(OrderHeader);
                return Ok(OrderHeader);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message,e);
                return BadRequest(e.Message);
            }
        }
        [HttpPut("update_stripe_id/{id}/{sessionId}/{paymentIntentId}")]
        public async Task<IActionResult> PutOrderHeader(int id,string sessionId,string paymentIntentId, OrderHeader orderHeader)
        {
            Task<OrderHeader> b = _unitOfWork.OrderHeader.GetById(id);
            OrderHeader OrderHeader = await b;
            if (id != OrderHeader.Id)
            {
                return BadRequest();
            }
            if (!OrderHeaderExists(id))
            {
                return NotFound();
            }
            try
            {
                _log4net.Info("update stripe payment is being called");
                //await _service.Update(OrderHeader);
                _unitOfWork.OrderHeader.UpdateStripePaymentID(id,sessionId,paymentIntentId);
                return Ok(OrderHeader);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message,e);
                return BadRequest(e.Message);
            }
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderHeader>> PostOrderHeader(Object b)
        {

            try
            {
                var a = b.ToString();
                var OrderHeader = JsonConvert.DeserializeObject<OrderHeader>(a);
                _log4net.Info("post order header for " +OrderHeader+" is being called");
                //await _service.Add(OrderHeader);
await _unitOfWork.OrderHeader.Add(OrderHeader);
                return CreatedAtAction("GetOrderHeader", new { id = OrderHeader.Id }, OrderHeader);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.InnerException.Message);
            }
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderHeader(int id)
        {
            //var OrderHeader = await _service.OrderHeaderExists(id);
            var OrderHeader = await _unitOfWork.OrderHeader.OrderHeaderExists(id);
            if (!OrderHeader)
            {
                return NotFound();
            }

            try
            {
                _log4net.Info("delete orderheader"+id+" is being called");
                // await _service.Delete(id);
                await _unitOfWork.OrderHeader.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        private bool OrderHeaderExists(int id)
        {
            _log4net.Info("OrderHeader exists for"+id+" is being called");
            // return (_service.GetById(id) != null);
            return (_unitOfWork.OrderHeader.GetById(id) != null);
        }
    }
}
