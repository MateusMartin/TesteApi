using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TesteApiUnitTest
{
    public class Teste
    {
        private ProductService _productService;
        private Context _context; 
        private ProductGenerator _productGenerator;
        private Random _random;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Context>() 
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new Context(options); 
            _productService = new ProductService(_context);
            _productGenerator = new ProductGenerator();
            _random = new Random();
        }

        [TearDown]
        public void TearDown()
        {
            // Descarte objetos IDisposable aqui
            _context.Dispose();
        }

        [Test]
        public async Task AddProductAsync_ProductAddedSuccessfully()
        {
            // Gerar um produto aleat�rio
            var product = _productGenerator.GenerateRandomProduct();

            // Adicionando o produto usando o servi�o
            await _productService.AddProductAsync(product);

            //Log do produto inserido
            Console.WriteLine("Produto inserido:");
            Console.WriteLine($"ID do Produto: {product.Id}");
            Console.WriteLine($"Nome: {product.Nome}");
            Console.WriteLine($"Descri��o: {product.Descricao}");
            Console.WriteLine($"Valor Unit�rio: {product.ValorUnitario}");

            // Procurar o produto rec�m-adicionado no banco de dados
            var addedProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);

            //Assert para verificar se o produto foi adicionado com sucesso
            Assert.IsNotNull(addedProduct);
            Assert.AreEqual(product.Nome, addedProduct.Nome);
            Assert.AreEqual(product.Descricao, addedProduct.Descricao);
            Assert.AreEqual(product.ValorUnitario, addedProduct.ValorUnitario);
        }


        [Test]
        public async Task UpdateProductAsync_ProductUpdatedSuccessfully()
        {
            // Gerar um produto aleat�rio
            var product = _productGenerator.GenerateRandomProduct();

            // Adicionando o produto usando o servi�o
            await _productService.AddProductAsync(product);

            // Log do produto antes da altera��o
            Console.WriteLine("Produto antes da altera��o:");
            Console.WriteLine($"ID do Produto: {product.Id}");
            Console.WriteLine($"Nome: {product.Nome}");
            Console.WriteLine($"Descri��o: {product.Descricao}");
            Console.WriteLine($"Valor Unit�rio: {product.ValorUnitario}");

            // Alterar o produto
            var novoValorUnitario = Math.Round((decimal)(_random.NextDouble() * 100), 2);
            product.Nome = "Nome Alterado";
            product.Descricao = "Descri��o Alterada";
            product.ValorUnitario = novoValorUnitario;

            // Atualizar o produto usando o servi�o
            await _productService.UpdateProductAsync(product);

            // Procurar o produto atualizado no banco de dados
            var updatedProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);

            // Log do produto depois da altera��o
            Console.WriteLine("Produto depois da altera��o:");
            Console.WriteLine($"ID do Produto: {updatedProduct.Id}");
            Console.WriteLine($"Nome: {updatedProduct.Nome}");
            Console.WriteLine($"Descri��o: {updatedProduct.Descricao}");
            Console.WriteLine($"Valor Unit�rio: {updatedProduct.ValorUnitario}");

            // Assert para verificar se o produto foi atualizado corretamente
            Assert.IsNotNull(updatedProduct);
            Assert.AreEqual(product.Nome, updatedProduct.Nome);
            Assert.AreEqual(product.Descricao, updatedProduct.Descricao);
            Assert.AreEqual(product.ValorUnitario, updatedProduct.ValorUnitario);
        }

        [Test]
        public async Task DisplayProductsInTableFormat()
        {
            // Gerar um n�mero aleat�rio de produtos entre 20 e 100
            int numProducts = _random.Next(20, 101);

            // Adicionar os produtos ao banco de dados
            List<Product> products = new List<Product>();
            for (int i = 0; i < numProducts; i++)
            {
                var product = _productGenerator.GenerateRandomProduct();
                await _productService.AddProductAsync(product);
                products.Add(product);
            }

            // Consultar os produtos no banco de dados
            var allProducts = _context.Products.ToList();

            // Determinar a largura das colunas
            int idWidth = 5;
            int nomeWidth = 20;
            int descricaoWidth = 30;
            int valorWidth = 15;

            // Linha de tra�os superior
            Console.WriteLine(new string('-', idWidth + nomeWidth + descricaoWidth + valorWidth + 13));

            // Exibir os cabe�alhos da tabela
            Console.WriteLine($"| {PadRight("ID", idWidth)} | {PadRight("Nome", nomeWidth)} | {PadRight("Descri��o", descricaoWidth)} | {PadRight("Valor Unit�rio", valorWidth)} |");
            Console.WriteLine($"|{new string('-', idWidth + 2)}|{new string('-', nomeWidth + 2)}|{new string('-', descricaoWidth + 2)}|{new string('-', valorWidth + 2)}|");

            // Exibir os produtos em formato de tabela
            foreach (var product in allProducts)
            {
                Console.WriteLine($"| {PadRight(product.Id.ToString(), idWidth)} | {PadRight(product.Nome, nomeWidth)} | {PadRight(product.Descricao, descricaoWidth)} | {PadRight(product.ValorUnitario.ToString("C"), valorWidth)} |");
            }

            // Linha de tra�os inferior
            Console.WriteLine(new string('-', idWidth + nomeWidth + descricaoWidth + valorWidth + 13));
        }

        private string PadRight(string input, int length)
        {
            return input.PadRight(length).Substring(0, length);
        }


        [Test]
        public async Task DeleteProductAsync_ProductDeletedSuccessfully()
        {
            // Gerar um produto aleat�rio
            var product = _productGenerator.GenerateRandomProduct();

            // Adicionar o produto ao banco de dados
            await _productService.AddProductAsync(product);
            Console.WriteLine($"Produto adicionado: ID = {product.Id}, Nome = {product.Nome}, Descri��o = {product.Descricao}, Valor Unit�rio = {product.ValorUnitario}");

            // Consultar o produto rec�m-adicionado no banco de dados
            var addedProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);
            Assert.IsNotNull(addedProduct, "O produto n�o foi adicionado corretamente ao banco de dados.");

            // Excluir o produto usando o servi�o
            await _productService.DeleteProductAsync(addedProduct);
            Console.WriteLine($"Produto exclu�do: ID = {addedProduct.Id}, Nome = {addedProduct.Nome}, Descri��o = {addedProduct.Descricao}, Valor Unit�rio = {addedProduct.ValorUnitario}");

            // Verificar se o produto foi exclu�do do banco de dados
            var deletedProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);

            Assert.IsNull(deletedProduct, "O produto n�o foi exclu�do corretamente do banco de dados.");
            Console.WriteLine("Nenhum produto retornado ap�s a exclus�o.");
        }

    }


}