using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteApi.Models;
using TesteApi.Service;

namespace TesteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IEnumerable<Product> GetAllProducts(string orderBy = "Id", string orderDirection = "Asc")
        {
            var products = _productService.GetAllProducts();

            switch (orderBy.ToLower())
            {
                case "nome":
                    products = orderDirection.ToLower() == "desc" ? products.OrderByDescending(p => p.Nome) : products.OrderBy(p => p.Nome);
                    break;
                case "valorunitario":
                    products = orderDirection.ToLower() == "desc" ? products.OrderByDescending(p => p.ValorUnitario) : products.OrderBy(p => p.ValorUnitario);
                    break;
                default:
                    products = orderDirection.ToLower() == "desc" ? products.OrderByDescending(p => p.Id) : products.OrderBy(p => p.Id);
                    break;
            }

            return products;
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (product.ValorUnitario < 0)
            {
                return BadRequest("O valor do produto não pode ser negativo.");
            }

            _productService.AddProduct(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            if (product.ValorUnitario < 0)
            {
                return BadRequest("O valor do produto não pode ser negativo.");
            }

            _productService.UpdateProduct(product);
            return Ok("Produto alterado -> "+ product.Nome);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);
            return Ok("Produto removido -> " + id); ;
        }

        [HttpGet("search")]
        public IEnumerable<Product> SearchProducts(string searchTerm)
        {
            return _productService.GetAllProducts().Where(p => p.Nome.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }




    }
}
