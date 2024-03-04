using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestesDeIntegracao
{
    public class Product
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public int Estoque { get; set; }
        public decimal ValorUnitario { get; set; }
    }

    public class ProductGenerator
    {
        private readonly Random _random;
        private readonly List<string> _nomes;

        public ProductGenerator()
        {
            _random = new Random();
            _nomes = LoadNamesFromFile("C:\\Users\\mateu\\source\\repos\\TesteApi\\TesteApiUnitTest\\nomes.txt");
        }

        private List<string> LoadNamesFromFile(string filePath)
        {
            var nomes = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] nomeSplit = line.Split(',');
                    foreach (string nome in nomeSplit)
                    {
                        nomes.Add(nome.Trim());
                    }
                }
            }
            return nomes;
        }

        public Product GenerateRandomProduct()
        {
            string nome = _nomes[_random.Next(_nomes.Count)];
            string descricao = $"Descrição do {nome}";
            int estoque = _random.Next(1, 100); // Gerando valor aleatório para o estoque
            decimal valorUnitario = Math.Round((decimal)(_random.NextDouble() * 100), 2);

            return new Product
            {       
                Nome = nome,
                Descricao = descricao,
                Estoque = estoque, // Atribuindo o valor gerado para o estoque
                ValorUnitario = valorUnitario
            };
        }
    }
}
