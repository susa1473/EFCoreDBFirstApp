using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreDBFirstApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly EcommerceGamesContext _context;

        public ProductTypeController(EcommerceGamesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductType>> GetAllProductTypeItems()
        {
            return Ok(await _context.ProductTypes.ToListAsync());
        }

        [HttpGet("{ProductTypeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductType>> GetProductTypeWithId([FromRoute] int ProductTypeId)
        {
            var producttypeitem = await this._context.ProductTypes.FirstOrDefaultAsync(x => x.Id == ProductTypeId);
            if (producttypeitem == null)
            {
                return NotFound();
            }

            return Ok(producttypeitem);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductType>> CreateProductType([FromBody] ProductType producttypeitem)
        {
            if (producttypeitem == null)
            {
                return BadRequest("Producttype object is null");
            }

            this._context.ProductTypes.Add(producttypeitem);
            var records = await this._context.SaveChangesAsync();
            if (records > 0)
            {
                return Created(string.Empty, (object)producttypeitem);
            }

            return BadRequest("ProductType was not created");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProductType([FromBody] ProductType producttype)
        {
            if (producttype == null)
            {
                return BadRequest("Producttype object is null");
            }
            this._context.Update(producttype);
            await this._context.SaveChangesAsync();
            return Ok(producttype);
        }


        /*
         * This method should have Authorization, the user should be Administrator  
         */
        [HttpDelete("{ProductTypeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int ProductTypeId)
        {
            var itemToDelete = await this._context.ProductTypes.FirstOrDefaultAsync(x => x.Id == ProductTypeId);
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
