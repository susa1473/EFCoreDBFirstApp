using EFCoreDBFirstApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace EFCoreDBFirstApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly EcommerceGamesContext _context;

        public ProductsController(EcommerceGamesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Product>> GetAllProducts()
        {
            return Ok(await _context.Products.OrderBy(p => p.Title).Include(p=> p.ProductVariants).ToListAsync());
        }

        [HttpGet("{ProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductWithId([FromRoute] int ProductId) 
        {
            var product = await this._context.Products.FirstOrDefaultAsync(x => x.Id == ProductId);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product object is null");
            }
                       
            this._context.Products.Add(product);
            var records = await this._context.SaveChangesAsync();
            if(records > 0)
            {               
                return Created(string.Empty, (object)product);
            }
            
            return BadRequest("Product was not created");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product) 
        {
            if (product == null)
            {
                return BadRequest("Product object is null");
            }
            this._context.Update(product);
            await this._context.SaveChangesAsync();
            return Ok(product);           
        }


        /*
         * This method should have Authorization, the user should be Administrator  
         */
        [HttpDelete("{ProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int ProductId)
        {
            var productToDelete = await this._context.Products.FirstOrDefaultAsync(x => x.Id == ProductId);
            if (productToDelete != null)
            {
                this._context.RemoveRange(productToDelete);
                await this._context.SaveChangesAsync();
                return Ok(productToDelete);
            }
 
            return NotFound();
        }
    }
}
