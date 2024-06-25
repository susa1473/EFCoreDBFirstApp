using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreDBFirstApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantsController : ControllerBase
    {
        private readonly EcommerceGamesContext _context;

        public ProductVariantsController(EcommerceGamesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductVariant>> GetAllProductVariantItems()
        {
            return Ok(await _context.ProductVariants.ToListAsync());
        }

        [HttpGet("{ProductVariantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductVariant>> GetProductVariantWithId([FromRoute] int ProductVariantId)
        {
            var item = await this._context.ProductVariants.FirstOrDefaultAsync(x => x.Id == ProductVariantId);
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
        public async Task<ActionResult<ProductVariant>> CreateProductVariant([FromBody] ProductVariant productvariantitem)
        {
            if (productvariantitem == null)
            {
                return BadRequest("Productvariant object is null");
            }

            this._context.ProductVariants.Add(productvariantitem);
            var records = await this._context.SaveChangesAsync();
            if (records > 0)
            {
                return Created(string.Empty, (object)productvariantitem);
            }

            return BadRequest("Productvariant was not created");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProductVariant([FromBody] ProductVariant productvariant)
        {
            if (productvariant == null)
            {
                return BadRequest("ProductVariant object is null");
            }
            this._context.Update(productvariant);
            await this._context.SaveChangesAsync();
            return Ok(productvariant);
        }


        /*
         * This method should have Authorization, the user should be Administrator  
         */
        [HttpDelete("{ProductVariantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int ProductVariantId)
        {
            var itemToDelete = await this._context.ProductVariants.FirstOrDefaultAsync(x => x.Id == ProductVariantId);
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
