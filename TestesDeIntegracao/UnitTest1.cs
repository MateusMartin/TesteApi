using Newtonsoft.Json;
using System.Net;
using System.Text;


namespace TestesDeIntegracao
{

    [TestFixture]
    public class Tests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            // Configurar o cliente HTTP para fazer solicitações à API
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost");
        }

  
        [Test]
        [Category("Integration")]
        public async Task GetAllProductsAPI()
        {
            // Fazer uma solicitação GET para uma rota da API
            HttpResponseMessage response = await _client.GetAsync("/api/Products?orderBy=Id&orderDirection=Asc");

            // Verificar se a resposta foi bem-sucedida (código de status 200)
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(responseBody);

            // Imprimir o resultado do teste na saída do console
            Console.WriteLine(responseBody);
        }

        [Test]
        [Category("Integration")]
        public async Task GetProduct_ReturnsSuccess()
        {
            // Arrange
            int id = 1;

            // Act
            var response = await _client.GetAsync($"/api/products/{id}");
            
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(responseBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // Imprimir o resultado do teste na saída do console
            Console.WriteLine(responseBody);
        }

        [Test]
        [Category("Integration")]
        public async Task GetProductById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int id = 999;

            // Act
            var response = await _client.GetAsync($"/api/products/{id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        [Category("Integration")]
        public async Task UpdateProduct_NegativeValue_ReturnsBadRequest()
        {
            // Arrange
            int id = 1;

            // Pega os detalhes do produto com o ID 1
            var getProductResponse = await _client.GetAsync($"/api/products/{id}");
            getProductResponse.EnsureSuccessStatusCode();
            var getProductResponseBody = await getProductResponse.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(getProductResponseBody);

            // Modifica o produto pra um valor negativo
            product.ValorUnitario = -10;

            // Converte o produto pra uma JSON string
            var jsonProduct = JsonConvert.SerializeObject(product);

            // Cria content com o json do produto e especifica media type
            var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

            // Act
            var putResponse = await _client.PutAsync($"/api/products/{id}", content);

            //Verifica se a response tem conteudo
            string responsePring = await putResponse.Content.ReadAsStringAsync();
            Assert.IsNotNull(responsePring);

            // Imprimir o resultado do teste na saída do console
            Console.WriteLine(responsePring);


            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, putResponse.StatusCode);
            string responseBody = await putResponse.Content.ReadAsStringAsync();
            StringAssert.Contains("O valor do produto não pode ser negativo.", responseBody);
        }

        [Test]
        [Category("Integration")]
        public async Task UpdateProduct_PositiveValue()
        {
            // Arrange
            int id = 1;

            // Pega os detalhes do produto com o ID 1
            var getProductResponse = await _client.GetAsync($"/api/products/{id}");
            getProductResponse.EnsureSuccessStatusCode();
            var getProductResponseBody = await getProductResponse.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(getProductResponseBody);

            // Modifica o produto pra um valor Positivo
            product.ValorUnitario = 20;

            // Converte o produto pra uma JSON string
            var jsonProduct = JsonConvert.SerializeObject(product);

            // Cria content com o json do produto e especifica media type
            var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

            // Act
            var putResponse = await _client.PutAsync($"/api/products/{id}", content);

            //Verifica se a response tem conteudo
            string responsePring = await putResponse.Content.ReadAsStringAsync();
            Assert.IsNotNull(responsePring);

            // Imprimir o resultado do teste na saída do console
            Console.WriteLine(responsePring);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode);
        }


        [Test]
        [Category("Integration")]
        public async Task SearchProducts_RetornaProdutosFiltrados()
        {
            // Arrange
            int id = 1;

            // Obtém os detalhes do produto com ID 1
            var getProductResponse = await _client.GetAsync($"/api/products/{id}");
            getProductResponse.EnsureSuccessStatusCode();
            var getProductResponseBody = await getProductResponse.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(getProductResponseBody);

            // Obtém as três primeiras letras do nome do produto
            string searchTerm = product.Nome.Substring(0, Math.Min(3, product.Nome.Length));

            // Act: Faz a requisição para buscar os produtos filtrados
            var response = await _client.GetAsync($"/api/products/search?searchTerm={searchTerm}");

            // Assert: Verifica se a requisição foi bem-sucedida
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // Converte a resposta para uma lista de produtos
            var responseBody = await response.Content.ReadAsStringAsync();
            var filteredProducts = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseBody);

            // Imprime os produtos filtrados
            Console.WriteLine("Produtos filtrados:");
            foreach (var filteredProduct in filteredProducts)
            {
                Console.WriteLine($"ID: {filteredProduct.Id}, Nome: {filteredProduct.Nome}, Valor Unitário: {filteredProduct.ValorUnitario}");
            }
        }

        [Test]
        [Category("Integration")]
        public async Task DeleteProduct_DeletesProductAndReturnsOk()
        {
            // Arrange
            int id = 2;

            // Consulta o produto antes de deletar
            var responseBeforeDeletion = await _client.GetAsync($"/api/products/{id}");

            // Verifica se o produto existe antes de deletar
            if (responseBeforeDeletion.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Produto com ID {id} não encontrado. Nada para deletar.");
                return;
            }

            string responseBodyBeforeDeletion = await responseBeforeDeletion.Content.ReadAsStringAsync();
            Console.WriteLine("Detalhes do produto antes da exclusão:");
            Console.WriteLine(responseBodyBeforeDeletion);

            // Act: Deleta o produto
            var responseDelete = await _client.DeleteAsync($"/api/products/{id}");

            // Assert: Verifica se a exclusão foi bem-sucedida (código de status 200)
            Assert.AreEqual(HttpStatusCode.OK, responseDelete.StatusCode);

            // Consulta o produto após a exclusão
            var responseAfterDeletion = await _client.GetAsync($"/api/products/{id}");

            // Assert: Verifica se o produto foi removido (status de resposta 404)
            Assert.AreEqual(HttpStatusCode.NotFound, responseAfterDeletion.StatusCode, "O produto foi removido com sucesso.");
        }


        [Test]
        [Category("Integration")]
        public async Task CreateProduct_ReturnsCreated()
        {
            // Arrange
            var newProduct = new Product
            {
                Nome = "Novo Produto",
                Descricao = "Descrição do Novo Produto",
                Estoque = 10,
                ValorUnitario = 50
            };

            // Converte o novo produto para uma JSON string
            var jsonProduct = JsonConvert.SerializeObject(newProduct);

            // Cria content com o JSON do novo produto e especifica media type
            var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

            // Act: Envia uma solicitação POST para criar o novo produto
            var postResponse = await _client.PostAsync("/api/products", content);

            // Assert: Verifica se a resposta é um sucesso (status 201 Created)
            Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);

            // Obtém o URI do novo recurso criado
            var createdProductUri = postResponse.Headers.Location;

            // Assert: Verifica se o URI do recurso criado não é nulo
            Assert.IsNotNull(createdProductUri);

            // Optional: If needed, you can retrieve the newly created product to further assert its properties or existence
            // Exemplo: Obtém os detalhes do novo produto criado
            var getCreatedProductResponse = await _client.GetAsync(createdProductUri);
            getCreatedProductResponse.EnsureSuccessStatusCode();
            var createdProductResponseBody = await getCreatedProductResponse.Content.ReadAsStringAsync();
            var createdProduct = JsonConvert.DeserializeObject<Product>(createdProductResponseBody);

            // Assert: Verifica se as propriedades do produto criado correspondem às propriedades esperadas
            Assert.AreEqual(newProduct.Nome, createdProduct.Nome);
            Assert.AreEqual(newProduct.Descricao, createdProduct.Descricao);
            Assert.AreEqual(newProduct.Estoque, createdProduct.Estoque);
            Assert.AreEqual(newProduct.ValorUnitario, createdProduct.ValorUnitario);
        }

    }

}
