using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace TrilhaApiDesafio.ViewModels
{
    /// <summary>
    /// Classe que representa o mapeamento de uma mensagem
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MessageViewModel
    {
        /// <summary>
        /// Mensagem
        /// </summary>
        [SwaggerSchema(Description = "Mensagem enviada para o usuário contendo informações sobre a requisição efetuada.", Nullable = false)]
        [JsonProperty("mensagem")]
        public String Message { get; set; }
    }
}
