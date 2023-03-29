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
    public class OrderDetailsController : ControllerBase
    {
        /*        private readonly IOrderDetailService<OrderDetail> _service;

                public OrderDetailsController(IOrderDetailService<OrderDetail> service)
                {
                    _service = service;
                }*/
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(OrderDetailsController));
        private readonly IUnitOfWorkRepo _unitOfWork;

        public OrderDetailsController(IUnitOfWorkRepo unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails()
        {
            try
            {
                _log4net.Info("get order detail list is being called");
                //var companies = await _service.GetOrderDetails();
                var companies = await _unitOfWork.OrderDetail.GetOrderDetails();
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
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int id)
        {

            try
            {
                _log4net.Info("get order detail"+id+" is being called");
                //var OrderDetail = await _service.GetById(id);
                var OrderDetail = await _unitOfWork.OrderDetail.GetById(id);

                if (OrderDetail == null)
                {
                    return NotFound();
                }

                return Ok(OrderDetail);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message,e);
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetail OrderDetail)
        {
            if (id != OrderDetail.Id)
            {
                return BadRequest();
            }
            if (!OrderDetailExists(id))
            {
                return NotFound();
            }
            try
            {
                _log4net.Info("put order detail  is being called");
                //await _service.Update(OrderDetail);
                await _unitOfWork.OrderDetail.Update(OrderDetail);
                return Ok(OrderDetail);
            }
            catch (Exception e)
            {_log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> PostOrderDetail(OrderDetail OrderDetail)//Object b)
        {
            /*var a = b.ToString();
            var OrderDetail = JsonConvert.DeserializeObject<OrderDetail>(a);*/
            try
            {
                _log4net.Info("post order detail for "+OrderDetail+" is being called");
                //await _service.Add(OrderDetail);
                await _unitOfWork.OrderDetail.Add(OrderDetail);
                return CreatedAtAction("GetOrderDetail", new { id = OrderDetail.Id }, OrderDetail);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            //var OrderDetail = await _service.OrderDetailExists(id);
            var OrderDetail = await _unitOfWork.OrderDetail.OrderDetailExists(id);
            if (!OrderDetail)
            {
                return NotFound();
            }

            try
            {
                _log4net.Info("post order Details for " +id +" isbeing called");
                //await _service.Delete(id);
await _unitOfWork.OrderDetail.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        private bool OrderDetailExists(int id)
        {
            _log4net.Info("OrderDetail exists for " + id+" is being called");
            //return (_service.GetById(id) != null);
            return (_unitOfWork.OrderDetail.GetById(id) != null);
        }
    }
}
