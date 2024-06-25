using EFCoreDBFirstApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreDBFirstApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly EcommerceGamesContext _context;

        public OrderItemController(EcommerceGamesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderItem>> GetAllOrderItems()
        {
            return Ok(await _context.CartItems.ToListAsync());
        }

        [HttpGet("{OrderItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderItem>> GetOrderItemWithId([FromRoute] int OrderItemId)
        {
            var item = await this._context.OrderItems.FirstOrDefaultAsync(x => x.Id == OrderItemId);
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
        public async Task<ActionResult<OrderItem>> CreateOrderItem([FromBody] OrderItem item)
        {
            if (item == null)
            {
                return BadRequest("OrderItem object is null");
            }

            this._context.OrderItems.Add(item);
            var records = await this._context.SaveChangesAsync();
            if (records > 0)
            {
                return Created(string.Empty, (object)item);
            }

            return BadRequest("OrderItem was not created");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderItem([FromBody] OrderItem orderitem)
        {
            if (orderitem == null)
            {
                return BadRequest("Order item object is null");
            }
            this._context.Update(orderitem);
            await this._context.SaveChangesAsync();
            return Ok(orderitem);
        }


        /*
         * This method should have Authorization, the user should be Administrator  
         */
        [HttpDelete("{OrderItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int OrderItemId)
        {
            var itemToDelete = await this._context.CartItems.FirstOrDefaultAsync(x => x.Id == OrderItemId);
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
