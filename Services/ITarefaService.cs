namespace TrilhaApiDesafio.Services
{
    using System;
    using System.Collections.Generic;
    using TrilhaApiDesafio.ViewModels;

    /// <summary>
    /// Contrato contendo os métodos para manipulação de dados no sistema
    /// </summary>
    public interface ITarefaService : IDisposable
    {
        /// <summary>
        /// Inclui um novo registro
        /// </summary>
        /// <param name="entity">Entidade a ser cadastrada</param>
        Int32 Insert(CreateTarefaViewModel entity);

        /// <summary>
        /// Altera o registro informado
        /// </summary>
        /// <param name="id">código de identificação da entidade</param>
        /// <param name="entity">Entidade a ser atualizada</param>
        void Update(Int32 id, TarefaViewModel entity);

        /// <summary>
        /// Exclui o registro informado
        /// </summary>
        /// <param name="id">Código de identificação da tarefa</param>
        void Delete(Int32 id);

        /// <summary>
        /// Localiza um registro
        /// </summary>
        /// <param name="id">Código de identificação da tarefa</param>
        /// <returns>O registro referente a entidade, caso exista</returns>
        TarefaViewModel Find(Int32 id);

        /// <summary>
        /// Lista os registros da entidade
        /// </summary>
        /// <param name="criteriaField">Critério de consulta</param>
        /// <param name="criteriaValue">Valor utilizado na consulta</param>
        /// <param name="limit">Quantidade de registros a serem recuperados na consulta</param>
        /// <param name="offset">Quantos registros "pular" na consulta</param>
        /// <returns>Listagem com os registros da entidade, de acordo com o filtro informado</returns>
        IEnumerable<TarefaViewModel> ListAll(EnumTarefaCriteriaField criteriaField, Object criteriaValue, Nullable<Int32> limit, Nullable<Int32> offset);

        /// <summary>
        /// Verifica se uma tarefa existe
        /// </summary>
        /// <param name="id">Id da tarefa</param>
        /// <returns>Se a tarefa existe</returns>
        Boolean Exists(Int32 id);
    }
}
