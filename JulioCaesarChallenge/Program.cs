using JulioCaesarChallenge.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JulioCaesarChallenge
{
    class Program
    {
        private static readonly string FileName = "answer";
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{FileName}.json");

        static void Main(string[] args)
        {
            // [1] Obtém resultado da API e grava em arquivo answer.json

            CodenationAPI api = new CodenationAPI();
            var response = api.Get();

            string body = response.Content.ReadAsStringAsync().Result;
            var jsonObject = new JObject(JObject.Parse(body));
            
            WriteTextToFile(text: jsonObject.ToString(), FileName);

            // [2] Atualiza arquivo Json com resultado decifrado

            CodenationChallengeModel obj = jsonObject.ToObject<CodenationChallengeModel>();
            obj.Decifrado = Decrypt(obj.Cifrado, obj.NumeroCasas);
            WriteTextToFile(text: JsonConvert.SerializeObject(obj), FileName);

            // [3] Atualiza arquivo Json com resumo criptográfico SHA1

            obj.ResumoCriptografico = GeraResumoCriptografico(obj.Decifrado);
            WriteTextToFile(text: JsonConvert.SerializeObject(obj), FileName);

            // [4] Envia arquivo answer.json via Post

            response = api.Post(FilePath, FileName).Result;

            // Exibe no console o resultado
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        private static string GeraResumoCriptografico(string text)
        {
            byte[] decifradoInBytes = Encoding.ASCII.GetBytes(text);
            byte[] hash = SHA1.Create().ComputeHash(decifradoInBytes);

            var resumoCriptografico = new StringBuilder();

            foreach (var hashByte in hash)
            {
                resumoCriptografico.Append(hashByte.ToString("x2"));
            }

            return resumoCriptografico.ToString();
        }

        private static string Decrypt(string cifrado, int numeroDeCasas)
        {
            cifrado = cifrado.ToLowerInvariant();

            StringBuilder builder = new StringBuilder();

            foreach (char c in cifrado)
            {
                builder.Append(Char.IsLetter(c) ? AdvanceCharacters(c, numeroDeCasas) : c );
            }

            return builder.ToString();
        }

        private static char AdvanceCharacters(char c, int numeroDeCasas)
        {
            char newChar = (char)(c - numeroDeCasas);
            return (newChar < 97 ? (char)(newChar + 26) : newChar);
        }

        private static void WriteTextToFile(string text, string fileName)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            //
            File.WriteAllText(
                path: Path.Combine(basePath, $"{fileName}.json"), 
                contents: text
            );
        }

        private static string ReadTextFromFile(string fileName)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string fileContent = File.ReadAllText(Path.Combine(basePath, $"{fileName}.json"));

            return fileContent;
        }

        
    }
}
