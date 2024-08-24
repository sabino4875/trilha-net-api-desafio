using System.ComponentModel;

namespace TrilhaApiDesafio.Models
{
    /// <summary>
    /// Estados da tarefa
    /// </summary>
    public enum EnumStatusTarefa
    {
        /// <summary>
        /// Invalido
        /// </summary>
        [Description("Invalida")]
        Invalido,
        /// <summary>
        /// Tarefa pendente
        /// </summary>
        [Description("Pendente")]
        Pendente,
        /// <summary>
        /// Tarefa finalizada
        /// </summary>
        [Description("Finalizado")]
        Finalizado
    }
}