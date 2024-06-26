using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreDBFirstApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly EcommerceGamesContext _context;

        public CartItemController(EcommerceGamesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CartItem>> GetAllCartItems()
        {
            return Ok(await _context.CartItems.ToListAsync());
        }

        [HttpGet("{AddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartItem>> GetCartItemsWithId([FromRoute] int CartItemId)
        {
            var cartitem = await this._context.Addresses.FirstOrDefaultAsync(x => x.Id == CartItemId);
            if (cartitem == null)
            {
                return NotFound();
            }

            return Ok(cartitem);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CartItem>> CreateCartitem([FromBody] CartItem cartitem)
        {
            if (cartitem == null)
            {
                return BadRequest("Cartitem object is null");
            }

            this._context.CartItems.Add(cartitem);
            var records = await this._context.SaveChangesAsync();
            if (records > 0)
            {
                return Created(string.Empty, (object)cartitem);
            }

            return BadRequest("Cartitem was not created");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCartItem([FromBody] CartItem cartitem)
        {
            if (cartitem == null)
            {
                return BadRequest("Cartitem object is null");
            }
            this._context.Update(cartitem);
            await this._context.SaveChangesAsync();
            return Ok(cartitem);
        }


        /*
         * This method should have Authorization, the user should be Administrator  
         */
        [HttpDelete("{CartItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int CartItemId)
        {
            var itemToDelete = await this._context.CartItems.FirstOrDefaultAsync(x => x.Id == CartItemId);
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
