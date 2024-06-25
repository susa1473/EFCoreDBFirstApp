using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreDBFirstApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly EcommerceGamesContext _context;

        public OrderController(EcommerceGamesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Order>> GetAllOrders()
        {
            return Ok(await _context.Orders.ToListAsync());
        }

        [HttpGet("{OrderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartItem>> GetOrderWithId([FromRoute] int OrdersId)
        {
            var item = await this._context.Orders.FirstOrDefaultAsync(x => x.Id == OrdersId);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order object is null");
            }

            this._context.Orders.Add(order);
            var records = await this._context.SaveChangesAsync();
            if (records > 0)
            {
                return Created(string.Empty, (object)order);
            }

            return BadRequest("Order was not created");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order object is null");
            }
            this._context.Update(order);
            await this._context.SaveChangesAsync();
            return Ok(order);
        }


        /*
         * This method should have Authorization, the user should be Administrator  
         */
        [HttpDelete("{OrderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int OrderId)
        {
            var itemToDelete = await this._context.CartItems.FirstOrDefaultAsync(x => x.Id == OrderId);
            if (itemToDelete != null)
            {
                this._context.RemoveRange(itemToDelete);
                await this._context.SaveChangesAsync();
                return Ok(itemToDelete);
            }

            return NotFound();
        }

    }
}
