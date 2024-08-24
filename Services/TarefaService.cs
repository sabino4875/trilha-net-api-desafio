namespace TrilhaApiDesafio.Services
{
    using System.Collections.Generic;
    using System;
    using TrilhaApiDesafio.ViewModels;
    using TrilhaApiDesafio.Repositories;
    using AutoMapper;
    using TrilhaApiDesafio.Models;
    using System.Linq.Expressions;
    using TrilhaApiDesafio.Helpers;
    using TrilhaApiDesafio.Extensions;

    /// <summary>
    /// Classe contendo os métodos para manipulação de dados no sistema
    /// </summary>
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _repository;
        private readonly IMapper _mapper;
        private readonly ApplicationRoutines _routines;
        private Boolean _disposable;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="repository">Classe contendo os métodos para manipulação de dados</param>
        /// <param name="mapper">Classe contendo os métodos para o mapeamento da entidade</param>
        /// <param name="routines">Classe contendo os métodos de uso geral no sistema</param>
        public TarefaService(ITarefaRepository repository, IMapper mapper, ApplicationRoutines routines)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(mapper);
            ArgumentNullException.ThrowIfNull(routines);

            _repository = repository;
            _mapper = mapper;
            _disposable = true;
            _routines = routines;
        }

        /// <summary>
        /// Método destrutor da classe
        /// </summary>
        /// <param name="disposing">Executa métodos adicionais</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (_disposable && disposing)
            {
                _disposable = false;
            }
        }

        /// <summary>
        /// Método destrutor da classe
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Método destrutor da classe
        /// </summary>
        ~TarefaService()
        {
            Dispose(false);
        }

        /// <summary>
        /// Inclui um novo registro
        /// </summary>
        /// <param name="entity">Entidade a ser cadastrada</param>
        public Int32 Insert(CreateTarefaViewModel entity)
        {
            var data = _mapper.Map<Tarefa>(entity);
            return _repository.Insert(data);
        }

        /// <summary>
        /// Altera o registro informado
        /// </summary>
        /// <param name="id">código de identificação da entidade</param>
        /// <param name="entity">Entidade a ser atualizada</param>
        public void Update(Int32 id, TarefaViewModel entity)
        {
            var data = _mapper.Map<Tarefa>(entity);
            _repository.Update(id, data);
        }

        /// <summary>
        /// Exclui o registro informado
        /// </summary>
        /// <param name="id">Código de identificação da tarefa</param>
        public void Delete(Int32 id)
        {
            _repository.Delete(e => e.Id == id);
        }

        /// <summary>
        /// Localiza um registro
        /// </summary>
        /// <param name="id">Código de identificação da tarefa</param>
        /// <returns>O registro referente a entidade, caso exista</returns>
        public TarefaViewModel Find(Int32 id)
        {
            var data = _repository.Find(e => e.Id == id);
            if (data != null)
            {
                var result = _mapper.Map<TarefaViewModel>(data);
                return result;
            }
            return null;
        }

        /// <summary>
        /// Lista os registros da entidade
        /// </summary>
        /// <param name="criteriaField">Critério de consulta</param>
        /// <param name="criteriaValue">Valor utilizado na consulta</param>
        /// <param name="limit">Quantidade de registros a serem recuperados na consulta</param>
        /// <param name="offset">Quantos registros "pular" na consulta</param>
        /// <returns>Listagem com os registros da entidade, de acordo com o filtro informado</returns>
        public IEnumerable<TarefaViewModel> ListAll(EnumTarefaCriteriaField criteriaField, Object criteriaValue, 
                                                    Nullable<Int32> limit, Nullable<Int32> offset)
        {

            //todos os registros
            Expression<Func<Tarefa, Boolean>> filter = e => true;

            var skip = offset ?? 0;
            var take = limit ?? 30;

            if (criteriaValue != null)
            {
                var value = criteriaValue.ToString();
                if (_routines.ContainsValue(criteriaValue.ToString()))
                {
                    switch (criteriaField)
                    {
                        case EnumTarefaCriteriaField.PorTitulo:
                            filter = e => e.Titulo.Contains(criteriaValue.ToString());
                            break;
                        case EnumTarefaCriteriaField.PorData:
                            if (DateTime.TryParse(value, out DateTime date))
                            {
                                filter = e => e.Data.Date == date.Date;
                            }
                            break;
                        case EnumTarefaCriteriaField.PorStatus:
                            var status = value.ParseEnum<EnumStatusTarefa>(EnumStatusTarefa.Invalido);
                            if (status != EnumStatusTarefa.Invalido)
                            {
                                filter = e => e.Status == status;
                            }
                            break;
                    }
                }
            }
            var data = _repository.ListAll(filter, take, skip);
            var result = _mapper.Map<IEnumerable<TarefaViewModel>>(data);

            return result;
        }

        /// <summary>
        /// Verifica se uma tarefa existe
        /// </summary>
        /// <param name="id">Id da tarefa</param>
        /// <returns>Se a tarefa existe</returns>
        public Boolean Exists(Int32 id)
        {
            return _repository.Count(e => e.Id == id) > 0;
        }
    }
}
