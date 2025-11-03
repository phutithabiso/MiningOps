using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;

namespace MiningOps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersApiController : ControllerBase
    {

        private readonly AppDbContext _context;

        public OrdersApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/orders/{id}/total
        [HttpGet("{id}/total")]
        public async Task<IActionResult> GetTotal(int id)
        {
            var orderItems = await _context.OrderItemsDb
                .Where(i => i.PurchaseOrderId == id)
                .ToListAsync();

            if (orderItems == null || !orderItems.Any())
                return Ok(new { total = 0 });

            decimal total = orderItems.Sum(i => i.Quantity * i.UnitPrice);
            return Ok(new { total });
        }
    }
}
