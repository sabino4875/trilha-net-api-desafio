namespace TrilhaApiDesafio.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using TrilhaApiDesafio.Models;

    /// <summary>
    /// Contrato representando as rotinas de manipulação de dados de uma entidade do sistema
    /// </summary>
    public interface ITarefaRepository : IDisposable
    {
        /// <summary>
        /// Inclui um novo registro
        /// </summary>
        /// <param name="entity">Entidade a ser cadastrada</param>
        Int32 Insert(Tarefa entity);

        /// <summary>
        /// Altera o registro informado
        /// </summary>
        /// <param name="id">código de identificação da entidade</param>
        /// <param name="entity">Entidade a ser atualizada</param>
        void Update(Int32 id, Tarefa entity);

        /// <summary>
        /// Exclui o registro informado
        /// </summary>
        /// <param name="filter">Filtro utilizado na exclusão do registro</param>
        void Delete(Expression<Func<Tarefa, Boolean>> filter);

        /// <summary>
        /// Localiza um registro
        /// </summary>
        /// <param name="filter">Filtro utilizado na consulta</param>
        /// <returns>O registro referente a entidade, caso exista</returns>
        Tarefa Find(Expression<Func<Tarefa, Boolean>> filter);

        /// <summary>
        /// Lista os registros da entidade
        /// </summary>
        /// <param name="filter">Filtro utilizado na consulta</param>
        /// <param name="limit">Quantidade de registros a serem recuperados na consulta</param>
        /// <param name="offset">Quantos registros "pular" na consulta</param>

        /// <returns>Listagem com os registros da entidade, de acordo com o filtro informado</returns>
        IEnumerable<Tarefa> ListAll(Expression<Func<Tarefa, Boolean>> filter, Int32 limit, Int32 offset);

        /// <summary>
        /// Contagem de registros
        /// </summary>
        /// <param name="filter">Filtro utilizado na consulta</param>
        /// <returns>O total de registros da entidade no banco de dados</returns>
        Int32 Count(Expression<Func<Tarefa, Boolean>> filter);

        /// <summary>
        /// Recupera o id do próximo registro a ser cadastrado no banco de dados
        /// </summary>
        /// <returns>O id do registro</returns>
        Int32 NextId();
    }
}
