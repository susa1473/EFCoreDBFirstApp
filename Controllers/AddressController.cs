using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreDBFirstApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly EcommerceGamesContext _context;

        public AddressController(EcommerceGamesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Address>> GetAllAddresses()
        {
            return Ok(await _context.Addresses.ToListAsync());
        }

        [HttpGet("{AddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Address>> GetAdressWithId([FromRoute] int AddressId)
        {
            var address = await this._context.Addresses.FirstOrDefaultAsync(x => x.Id == AddressId);
            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> CreateAddress([FromBody] Address address)
        {
            if (address == null)
            {
                return BadRequest("Address object is null");
            }

            this._context.Addresses.Add(address);
            var records = await this._context.SaveChangesAsync();
            if (records > 0)
            {
                return Created(string.Empty, (object)address);
            }

            return BadRequest("Address was not created");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAddress([FromBody] Address address)
        {
            if (address == null)
            {
                return BadRequest("Address object is null");
            }
            this._context.Update(address);
            await this._context.SaveChangesAsync();
            return Ok(address);
        }


        /*
         * This method should have Authorization, the user should be Administrator  
         */
        [HttpDelete("{AddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int AddressId)
        {
            var itemToDelete = await this._context.Addresses.FirstOrDefaultAsync(x => x.Id == AddressId);
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
