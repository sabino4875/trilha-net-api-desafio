namespace TrilhaApiDesafio.Models
{
    using System;

    /// <summary>
    /// Classe que implementa a entidade tarefa
    /// </summary>
    /// <param name="id">C�digo de identifica��o da tarefa</param>
    /// <param name="titulo">T�tulo da tarefa</param>
    /// <param name="descricao">Descri��o da tarefa</param>
    /// <param name="data">Data de inicializa��o da tarefa</param>
    /// <param name="status">Status da tarefa</param>
    public class Tarefa(Int32 id, String titulo, String descricao, DateTime data, EnumStatusTarefa status)
    {

        /// <summary>
        /// M�todo construtor da classe
        /// </summary>
        /// <param name="titulo">T�tulo da tarefa</param>
        /// <param name="descricao">Descri��o da tarefa</param>
        /// <param name="data">Data de inicializa��o da tarefa</param>
        /// <param name="status">Status da tarefa</param>
        public Tarefa(String titulo, String descricao, DateTime data, EnumStatusTarefa status) 
               : this(0, titulo, descricao, data, status)
        {
        }

        /// <summary>
        /// M�todo construtor da classe
        /// </summary>
        public Tarefa() 
               : this(String.Empty, String.Empty, DateTime.Now, EnumStatusTarefa.Pendente)
        {
        }

        /// <summary>
        /// C�digo de identifica��o da entidade
        /// </summary>
        public Int32 Id { get; set; } = id;

        /// <summary>
        /// T�tulo 
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