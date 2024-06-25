using Microsoft.AspNetCore.Mvc;

namespace EFCoreDBFirstApp.Controllers
{
    public class ImageController : Controller
    {
        private readonly EcommerceGamesContext _context;

        public ImageController(EcommerceGamesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Image>> GetAllImages()
        {
            return Ok(await _context.Images.ToListAsync());
        }

        [HttpGet("{ImageId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Image>> GetImagesWithId([FromRoute] int ImageId)
        {
            var image = await this._context.Images.FirstOrDefaultAsync(x => x.Id == ImageId);
            if (image == null)
            {
                return NotFound();
            }

            return Ok(image);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Image>> CreateImage([FromBody] Image imageitem)
        {
            if (imageitem == null)
            {
                return BadRequest("Image object is null");
            }

            this._context.Images.Add(imageitem);
            var records = await this._context.SaveChangesAsync();
            if (records > 0)
            {
                return Created(string.Empty, (object)imageitem);
            }

            return BadRequest("Image was not created");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateImage([FromBody] Image imageitem)
        {
            if (imageitem == null)
            {
                return BadRequest("Image object is null");
            }
            this._context.Update(imageitem);
            await this._context.SaveChangesAsync();
            return Ok(imageitem);
        }


        /*
         * This method should have Authorization, the user should be Administrator  
         */
        [HttpDelete("{ImageId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int ImageId)
        {
            var itemToDelete = await this._context.Images.FirstOrDefaultAsync(x => x.Id == ImageId);
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
