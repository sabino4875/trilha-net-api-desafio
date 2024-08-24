namespace TrilhaApiDesafio.ViewModels
{
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Annotations;
    using System;

    /// <summary>
    /// Classe que representa o mapeamento de uma entidade tarefa para inclusão
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CreateTarefaViewModel
    {
        /// <summary>
        /// Título 
        /// </summary>
        [SwaggerSchema(Description = "Título da tarefa", Nullable = false)]
        [JsonProperty("titulo")]
        public String Titulo { get; set; }

        /// <summary>
        /// Descricao
        /// </summary>
        [SwaggerSchema(Description = "Descrição da tarefa", Nullable = false)]
        [JsonProperty("descricao")]
        public String Descricao { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        [SwaggerSchema(Description = "Data de início da tarefa", Nullable = false)]
        [JsonProperty("data")]
        public Nullable<DateTime> Data { get; set; }
    }
}
