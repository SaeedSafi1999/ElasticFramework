using ElasticFramework.Services.Elastic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using testApp.Dependency;

namespace testApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductController(IProductRepository productRepository, IElasticContext context) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> SetToElastic()
        {
            var prod = new Product { Id = Guid.NewGuid().ToString(), Name = "PS5 slim 1TR", Price = 1000.0, Category = "Game Console" };
            await context.CreateIndexAsync("Products");
            return Ok(await context.IndexDocumentAsync("Products", prod));
        }

        [HttpPost]
        public async Task<IActionResult> AddToElastic()
        {
            var res = await productRepository.Insert(new Product
            {
                Category = "Game Console",
                Id = "2",
                Name = "PS5 slim 6T",
                Price = 2500
            });
            return Ok(res);
        }


        [HttpGet]
        public async Task<IActionResult> SearchInElastic()
        {
            return Ok(await productRepository.SearchByCategoryAsync("Game Console"));
        }


        [HttpGet]
        public async Task<IActionResult> SearchPriceRangeInElastic()
        {
            return Ok(await productRepository.SearchByPriceRangeAsync(1000,1800));
        }
    }
}
