namespace TrilhaApiDesafio.Services
{
    /// <summary>
    /// Enum contendo as opções de filtro
    /// </summary>
    public enum EnumTarefaCriteriaField
    {
        /// <summary>
        /// Todos os registros
        /// </summary>
        Todos,
        /// <summary>
        /// Contendo um valor no titulo
        /// </summary>
        PorTitulo,
        /// <summary>
        /// Pela da da tarefa
        /// </summary>
        PorData,
        /// <summary>
        /// Pelo status da tarefa
        /// </summary>
        PorStatus
    }
}
