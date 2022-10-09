using Newtonsoft.Json;
using ProjetoAvaliacao.CorreiosWebService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAvaliacao
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var numeros = new List<int>();

            Console.WriteLine("Entre com uma numeração para que possamos ordenar ou qualquer outra tecla para finalizar:");

            while (true)
            {
                var entrada = Console.ReadLine();

                var isNumeric = int.TryParse(entrada.ToString(), out int numero);

                if (!isNumeric)
                    break;

                numeros.Add(numero);

                Console.WriteLine("Números ordenados: " + string.Join(", ", numeros.OrderBy(n => n)));
            }

            var nomeArquivo = "NumerosOrdenados.txt";

            var salvoSucesso = SalvarNumeros(nomeArquivo, numeros);

            if (salvoSucesso)
                Console.WriteLine($"Arquivo {nomeArquivo} salvo com sucesso, o mesmo se encontra no mesmo diretório do executável.");


            var clsTeste = new ClsTeste()
            {
                Codigo = 1000,
                Descricao = "teste"
            };

            string json = JsonConvert.SerializeObject(clsTeste);

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Entre com um CEP para buscarmos na base dos correios: ");

            var cep = Console.ReadLine();

            var cepResult = ConsultarCep(cep);

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Endereço Localizado:");

            Console.WriteLine("Endereço: {0}", cepResult.end);
            Console.WriteLine("Complemento: {0}", cepResult.complemento2);
            Console.WriteLine("Bairro: {0}", cepResult.bairro);
            Console.WriteLine("Cidade: {0}", cepResult.cidade);
            Console.WriteLine("Estado: {0}", cepResult.uf);

            Console.WriteLine("Entre com qualquer tecla para continuar...");
            Console.ReadKey();

            //Downlad Image:
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Baixando logotipo da google...");

            string urlImage = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png";
            new WebClient().DownloadFile(new Uri(urlImage), @"image.png");

            Console.WriteLine("Logotipo baixado com sucesso!, o mesmo se encontra no diretório do aplicativo");

            //Lê arquivo
            var fileBytes = File.ReadAllBytes("image.png");

            var base64 = Convert.ToBase64String(fileBytes);

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Arquivo baixado codificado em base64: " + base64);

            Console.ReadKey();
        }

        static bool SalvarNumeros(string nomeArquivo, List<int> numeros)
        {
            try
            {
                File.WriteAllText(nomeArquivo, string.Join(", ", numeros.OrderBy(n => n)));
                return true;
            }
            catch
            {
                return false;
            }
        }

        static enderecoERP ConsultarCep(string cep)
        {
            var service = new CorreiosWebService.AtendeClienteClient();
            return service.consultaCEP(cep);
        }

        static public string EncodeToBase64(string texto)
        {
            try
            {
                byte[] textoAsBytes = Encoding.ASCII.GetBytes(texto);
                string resultado = System.Convert.ToBase64String(textoAsBytes);
                return resultado;
            }
            catch
            {
                throw;
            }
        }

    }
}
