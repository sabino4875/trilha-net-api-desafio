namespace TrilhaApiDesafio.Repositories
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System;
    using TrilhaApiDesafio.Models;
    using Serilog;
    using TrilhaApiDesafio.Context;
    using System.Linq;

    /// <summary>
    /// Classe representando as rotinas de manipulação de dados de uma entidade do sistema
    /// </summary>
    public class TarefaRepository : ITarefaRepository
    {
        private Boolean _disposable;
        private readonly ILogger _logger;
        private readonly OrganizadorContext _context;
        /// <summary>
        /// Método construtor da classe
        /// </summary>
        public TarefaRepository(OrganizadorContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            _disposable = true;
            _logger = Log.ForContext<TarefaRepository>();
            _context = context;
        }

        /// <summary>
        /// Métoodo destrutor da classe
        /// </summary>
        /// <param name="disposing">Executa rotinas adicionais ao remover a classe da memória</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing && _disposable)
            {
                _disposable = false;
            }
        }

        /// <summary>
        /// Métoodo destrutor da classe
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Métoodo destrutor da classe - Garbage Collection
        /// </summary>
        ~TarefaRepository()
        {
            Dispose(false);
        }

        /// <summary>
        /// Inclui um novo registro
        /// </summary>
        /// <param name="entity">Entidade a ser cadastrada</param>
        public Int32 Insert(Tarefa entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            try
            {
                entity.Id = NextId();
                _context.Tarefas.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar incluir o registro.");
                throw;
            }
        }

        /// <summary>
        /// Altera o registro informado
        /// </summary>
        /// <param name="id">código de identificação da entidade</param>
        /// <param name="entity">Entidade a ser atualizada</param>
        public void Update(Int32 id, Tarefa entity)
        {
            ArgumentNullException.ThrowIfNull(id);
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                var exists = _context.Tarefas.Any(x => x.Id == id);
                if (exists)
                {
                    _context.Tarefas.Update(entity);
                    _context.SaveChanges();
                }
                else throw new InvalidOperationException("Registro não localizado.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar alterar o registro.");
                throw;
            }
        }

        /// <summary>
        /// Exclui o registro informado
        /// </summary>
        /// <param name="filter">Filtro utilizado na exclusão do registro</param>
        public void Delete(Expression<Func<Tarefa, Boolean>> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);
            try
            {
                var exists = _context.Tarefas.Any(filter);
                if (exists)
                {
                    var items = _context.Tarefas.Where(filter).ToArray();
                    _context.Tarefas.RemoveRange(items);
                    _context.SaveChanges();
                }
                else throw new InvalidOperationException("Filtro informado inválido.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar excluir o registro.");
                throw;
            }
        }

        /// <summary>
        /// Localiza um registro
        /// </summary>
        /// <param name="filter">Filtro utilizado na consulta</param>
        /// <returns>O registro referente a entidade, caso exista</returns>
        public Tarefa Find(Expression<Func<Tarefa, Boolean>> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);
            try
            {
                var exists = _context.Tarefas.Any(filter);
                if (exists)
                {
                    var item = _context.Tarefas.Where(filter).First();
                    return item;
                }
                else throw new InvalidOperationException("Filtro informado inválido.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar localizar o registro.");
                throw;
            }
        }

        /// <summary>
        /// Lista os registros da entidade
        /// </summary>
        /// <param name="filter">Filtro utilizado na consulta</param>
        /// <param name="limit">Quantidade de registros a serem recuperados na consulta</param>
        /// <param name="offset">Quantos registros "pular" na consulta</param>

        /// <returns>Listagem com os registros da entidade, de acordo com o filtro informado</returns>
        public IEnumerable<Tarefa> ListAll(Expression<Func<Tarefa, Boolean>> filter, Int32 limit, Int32 offset)
        {
            ArgumentNullException.ThrowIfNull(filter);
            try
            {
                var item = _context.Tarefas.Where(filter).Skip(offset).Take(limit).ToList();
                return item;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar listar os registros.");
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter">Filtro utilizado na consulta</param>
        /// <returns>O total de registros da entidade no banco de dados</returns>
        public Int32 Count(Expression<Func<Tarefa, Boolean>> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);
            try
            {
                var total = _context.Tarefas.Count(filter);
                return total;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar contar os registros.");
                throw;
            }
        }

        /// <summary>
        /// Recupera o id do próximo registro a ser cadastrado no banco de dados
        /// </summary>
        /// <returns>O id do registro</returns>
        public Int32 NextId()
        {
            try
            {
                var total = 1;
                if (_context.Tarefas.Any())
                {
                    total = _context.Tarefas.OrderByDescending(e => e.Id).Select(e => e.Id).First() + 1;
                }
                return total;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Houve um erro ao tentar recuperar o próximo id da tarefa.");
                throw;
            }

        }
    }
}
