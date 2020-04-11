using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JulioCaesarChallenge.Services
{
    class CodenationAPI
    {
        private static string Token { get; } = "here goes the token";
        private HttpClient cliente;

        public CodenationAPI()
        {
        }            
        

        public HttpResponseMessage Get() {
        
            using (cliente = new HttpClient() { BaseAddress = new Uri("https://api.codenation.dev/") }) {

                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //
                var requestUri = new Uri($"v1/challenge/dev-ps/generate-data?token={Token}", UriKind.Relative);

                return cliente.GetAsync(requestUri).Result;
            }
        }

        public async Task<HttpResponseMessage> Post(string file, string fileName)
        {
            using (cliente = new HttpClient() { BaseAddress = new Uri("https://api.codenation.dev/") })
            {
                var requestUri = new Uri($"v1/challenge/dev-ps/submit-solution?token={Token}", UriKind.Relative);
                //cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                byte[] jsonFileBytes = File.ReadAllBytes(file);
                ByteArrayContent byteArrayContent = new ByteArrayContent(jsonFileBytes);
                byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                //
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(byteArrayContent, "answer", "answer.json");

                return await cliente.PostAsync(requestUri, form);
            }
        }
    }
}
