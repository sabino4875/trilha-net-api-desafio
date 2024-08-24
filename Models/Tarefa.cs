namespace TrilhaApiDesafio.Models
{
    using System;

    /// <summary>
    /// Classe que implementa a entidade tarefa
    /// </summary>
    /// <param name="id">Código de identificação da tarefa</param>
    /// <param name="titulo">Título da tarefa</param>
    /// <param name="descricao">Descrição da tarefa</param>
    /// <param name="data">Data de inicialização da tarefa</param>
    /// <param name="status">Status da tarefa</param>
    public class Tarefa(Int32 id, String titulo, String descricao, DateTime data, EnumStatusTarefa status)
    {

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="titulo">Título da tarefa</param>
        /// <param name="descricao">Descrição da tarefa</param>
        /// <param name="data">Data de inicialização da tarefa</param>
        /// <param name="status">Status da tarefa</param>
        public Tarefa(String titulo, String descricao, DateTime data, EnumStatusTarefa status) 
               : this(0, titulo, descricao, data, status)
        {
        }

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        public Tarefa() 
               : this(String.Empty, String.Empty, DateTime.Now, EnumStatusTarefa.Pendente)
        {
        }

        /// <summary>
        /// Código de identificação da entidade
        /// </summary>
        public Int32 Id { get; set; } = id;

        /// <summary>
        /// Título 
        /// </summary>
        public String Titulo { get; set; } = titulo;

        /// <summary>
        /// Descricao
        /// </summary>
        public String Descricao { get; set; } = descricao;

        /// <summary>
        /// Data
        /// </summary>
        public DateTime Data { get; set; } = data;

        /// <summary>
        /// Status
        /// </summary>
        public EnumStatusTarefa Status { get; set; } = status;
    }
}