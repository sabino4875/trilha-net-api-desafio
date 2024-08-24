namespace TrilhaApiDesafio.ViewModels
{
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Annotations;
    using System;

    /// <summary>
    /// Classe que representa o mapeamento de uma entidade tarefa para edição / consulta
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TarefaViewModel
    {
        /// <summary>
        /// Código de identificação da entidade
        /// </summary>
        [SwaggerSchema(Description = "Código de identificação da tarefa", Nullable = false)]
        [JsonProperty("id")]
        public Nullable<Int32> Id { get; set; }

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

        /// <summary>
        /// Status
        /// </summary>
        [SwaggerSchema(Description = "Status da tarefa", Nullable = false)]
        [JsonProperty("status")]
        public String Status { get; set; }
    }
}
