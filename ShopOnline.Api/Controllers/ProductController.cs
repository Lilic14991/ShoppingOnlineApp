using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;
using ShopOnline.Api.Extensions;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;

        public ProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await _productRepo.GetItems();
                var productCategories = await _productRepo.GetCategories();

                if (products == null || productCategories == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto(productCategories);

                    return Ok(productDtos);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await _productRepo.GetItem(id);

                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    var productCategory = await _productRepo.GetCategory(product.CategoryId);

                    var productDto = product.ConvertToDto(productCategory);
                    return Ok(productDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>>  GetProductCategories()
        {
            try
            {
                var productCategories = await _productRepo.GetCategories();
                var productCategoryDtos = productCategories.ConvertToDto();
                return Ok(productCategoryDtos);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var products = await _productRepo.GetItemsByCategory(categoryId);
                var productCategories = await _productRepo.GetCategories();
                var productDtos = products.ConvertToDto(productCategories); 

                return Ok(productDtos);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }


    }
}
