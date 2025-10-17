using ApplicationService.Dtos.ProductDtos;
using ApplicationService.ProductServices.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region [- Filds -]
        readonly private IProductApplicationService _productApplicationService;
        #endregion

        #region [- Constructor -]
        public ProductController(IProductApplicationService productApplicationService)
        {
            _productApplicationService = productApplicationService;
        }

        #endregion

        #region [- Create() -]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PostProductDto productCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(productCreate);

            var response = await _productApplicationService.Post(productCreate);
            if (!response.IsSuccessful)
                return BadRequest(response.ErrorMessage);

            return new JsonResult(response.Result);
        }

        #endregion

        #region [- Edit() -]
        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] PutProductDto productEdit)
        {
            if (!ModelState.IsValid)
                return BadRequest(productEdit);

            var response = await _productApplicationService.Put(productEdit);
            if (!response.IsSuccessful)
                return BadRequest(response.ErrorMessage);

            return new JsonResult(response.Result);
        }
        #endregion

        #region [- Delete() -]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteProductDto deleteProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(deleteProductDto);

            var response = await _productApplicationService.Delete(deleteProductDto);
            if (!response.IsSuccessful)
                return BadRequest(response.ErrorMessage);

            return new JsonResult(response.Result);
        }
        #endregion

        #region [- Index() -]
        public IActionResult Index()
        {
            return new JsonResult(new GetAllProductDto { GetByIdProductDtos = new List<GetByIdProductDto>() });
        }
        #endregion

        #region [- GetAll() -]
        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _productApplicationService.GetAll();
            if (!response.IsSuccessful)
                return new JsonResult(new GetAllProductDto { GetByIdProductDtos = new List<GetByIdProductDto>() });

            return new JsonResult(response.Result);
        }
        #endregion

    }
}
