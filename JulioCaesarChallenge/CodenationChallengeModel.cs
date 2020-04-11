using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JulioCaesarChallenge
{
    class CodenationChallengeModel
    {
        [JsonProperty(PropertyName = "numero_casas")]
        public int NumeroCasas { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "cifrado")]
        public string Cifrado { get; set; }

        [JsonProperty(PropertyName = "decifrado")]
        public string Decifrado { get; set; }

        [JsonProperty(PropertyName = "resumo_criptografico")]
        public string ResumoCriptografico { get; set; }
    }
}
