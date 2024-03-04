<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
</head>
<body>
  <h1>Projeto de Exemplo com Entity Framework e Testes Unitários</h1>
  <p>Este é um projeto de exemplo que demonstra o uso do Entity Framework com abordagem code-first em uma aplicação ASP.NET Core, juntamente com testes unitários.</p>
  <h2>Resumo do Projeto</h2>
  <p>O projeto consiste em um CRUD básico para gerenciamento de produtos, onde os dados são gerados automaticamente ao iniciar a aplicação. A aplicação permite criar, alterar, deletar e visualizar produtos.</p>
  <h2>Projetos Incluídos</h2>
  <ul>
    <li>TesteApi: Projeto principal da aplicação ASP.NET Core que inclui a lógica de negócios e a API REST para manipulação de produtos.</li>
    <li>TesteApiUnitTest: Projeto de testes unitários que testa a lógica de negócios da aplicação.</li>
  </ul>
  <h2>Sobre o Code-First</h2>
  <p>O Entity Framework suporta duas abordagens principais para modelagem de dados: code-first e database-first. Optei por utilizar a abordagem code-first neste projeto devido à sua facilidade de manutenção, controle total e portabilidade.</p>
  <h3>Controle Total e Facilidade de Manutenção</h3>
  <p>Com a abordagem code-first, tenho controle total sobre o modelo de dados da aplicação diretamente no código fonte. Isso facilita a compreensão e a manutenção do modelo de dados ao longo do tempo, pois todas as alterações podem ser feitas de forma programática e versionada juntamente com o restante do código.</p>
  <h3>Portabilidade e Facilidade de Implantação</h3>
  <p>Outra vantagem do code-first é a portabilidade e facilidade de implantação. Como o modelo de dados é definido no próprio código fonte, não há necessidade de gerenciar scripts de criação de banco de dados separados. Isso simplifica o processo de implantação em diferentes ambientes e torna mais fácil compartilhar e colaborar em projetos entre membros da equipe.</p>
  <h3>Banco de Dados In-Memory como Facilitador</h3>
  <p>A utilização do banco de dados in-memory é uma escolha estratégica que se alinha perfeitamente com a abordagem code-first. Com o banco de dados in-memory, podemos criar e popular o banco de dados automaticamente ao iniciar a aplicação, sem a necessidade de configurações adicionais ou instalações de banco de dados externos. Isso simplifica o processo de desenvolvimento e teste, permitindo que os desenvolvedores comecem a trabalhar rapidamente em novos recursos sem a necessidade de configurar um ambiente de banco de dados separado.</p>
  <h3>Alteração do Range de Nomes dos Produtos</h3>
  <p>Para alterar o range de nomes dos produtos, basta editar o arquivo <strong>appsettings.json</strong>. No array <strong>Nomes</strong>, você pode adicionar ou remover nomes conforme necessário. Após editar o arquivo <strong>appsettings.json</strong>, os novos nomes serão utilizados automaticamente na próxima execução da aplicação.</p>
  <p>Exemplo de <strong>appsettings.json</strong>:</p>
  <pre>
{
  "AppSettings": {
    "Nomes": [
      "Arroz Integral",
      "Feijão Preto",
      "Óleo de Coco",
      "Açúcar Mascavo",
      "Café Premium",
      "Sal Marinho",
      "Macarrão Integral"
    ]
  }
}
  </pre>

<h2>Alteração do Número de Dados Gerados no Banco</h2>
  <p>Para alterar o número de dados gerados no banco, basta editar o método SeedData no arquivo <strong>ApplicationBuilderExtensions.cs</strong>. Dentro do loop for, você pode ajustar o número de iterações para o valor desejado. Após editar o arquivo, os novos dados serão gerados automaticamente na próxima execução da aplicação.</p>

  <pre>
    for (int i = 0; i < 10; i++)
    {
        var randomProduct = productGenerator.GenerateRandomProduct();
        context.Products.Add(randomProduct);
    }
  </pre>

    
  <h2>Como Usar</h2>
  <ol>
    <li>Clone o repositório para o seu ambiente de desenvolvimento.</li>
    <li>Certifique-se de ter o .NET 6 SDK instalado em sua máquina.</li>
    <li>Abra o projeto no Visual Studio ou em seu editor de código preferido.</li>
    <li>Execute a aplicação. Isso criará automaticamente o banco de dados em memória e populará com 5 produtos ao iniciar a aplicação.</li>
    <li>Para executar os testes unitários, navegue até o projeto TesteApiUnitTest e execute os testes usando o Test Explorer ou a linha de comando.</li>
  </ol>

   <h1>Agradecimento</h1>
   <p>Agradeço a você por ler este README!</p>
   <p>Espero que tenha gostado do meu projeto:</p>
   
   <img src="https://pm1.aminoapps.com/6229/46defe3222bccb7d52d260d4abebddf0291081bc_00.jpg"
        alt="Anime Manga" width="300">
   <p>Continue explorando e aprendendo!</p>
   <p><strong>Obrigado!</strong></p>
   <p>"A imaginação é mais importante que o conhecimento. O conhecimento é limitado. A imaginação circunda o mundo." - Albert
        Einstein</p>
  
</body>
</html>
