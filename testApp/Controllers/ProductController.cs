using ElasticSearchSharp.Services.Services.Elastic;
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
    }
}
