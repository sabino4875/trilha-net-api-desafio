namespace TrilhaApiDesafio.Controllers
{
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Collections.Generic;
    using TrilhaApiDesafio.Extensions;
    using TrilhaApiDesafio.Helpers;
    using TrilhaApiDesafio.Models;
    using TrilhaApiDesafio.Services;
    using TrilhaApiDesafio.Validation;
    using TrilhaApiDesafio.ViewModels;

    /// <summary>
    /// Classe contendo os métodos relacionados ao acesso as rotinas para manipulação e consulta de dados da entidade tarefa
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _service;
        private readonly IValidator<CreateTarefaViewModel> _createValidation;
        private readonly IValidator<TarefaViewModel> _updateValidation;
        private readonly ApplicationRoutines _routines;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="service">Classe contendo rotinas para a manipulação de dados da entidade tarefa</param>
        /// <param name="createValidation">Classe contendo rotinas para a validação da inclusão de uma entidade tarefa</param>
        /// <param name="updateValidation">Classe contendo rotinas para a validação da atualização de uma entidade tarefa</param>
        /// <param name="routines">Classe contendo rotinas para utilização no sistema</param>
        public TarefaController([FromServices] ITarefaService service,
                                [FromServices] IValidator<CreateTarefaViewModel> createValidation,
                                [FromServices] IValidator<TarefaViewModel> updateValidation,
                                [FromServices] ApplicationRoutines routines)
        {
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(createValidation);
            ArgumentNullException.ThrowIfNull(updateValidation);
            ArgumentNullException.ThrowIfNull(routines);

            _service = service;
            _createValidation = createValidation;
            _updateValidation = updateValidation;
            _routines = routines;
        }

        /// <summary>
        /// Localiza um registro pelo seu id
        /// </summary>
        /// <param name="id">Id da tarefa</param>
        /// <returns></returns>
        [HttpGet(Name = "ObterPorId")]
        [Produces("application/json")]
        [SwaggerOperation(summary: "Localiza um registro pelo id", description: "Recupera um registro pelo seu id.")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TarefaViewModel), Description = "A tarefa carregada com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(MessageViewModel), Description = "Id informado inválido.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Tarefa não localizada.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemHttpResult), Description = "Erro ao tentar localizar o registro.")]
        public IActionResult ObterPorId([SwaggerParameter(Description = "Código de identificação da tarefa.", Required = true)][FromQuery] String id)
        {
            try
            {
                if(_routines.ContainsValue(id))
                {
                    if(Int32.TryParse(id, out Int32 value))
                    {
                        // TODO: Buscar o Id no banco utilizando o EF
                        var exists = _service.Exists(value);
                        // TODO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
                        if (exists)
                        {
                            //TODO: caso contrário retornar OK com a tarefa encontrada
                            var result = _service.Find(value);
                            return Ok(result);
                        }
                        return NotFound();
                    }
                }
                return BadRequest(new MessageViewModel { Message = "Id informado inválido." });
            }
            catch (ApplicationException ex)
            {
                return Problem(statusCode: 500, title: "Erro ao tentar localizar o registro.", detail: ex.Message);
            }
        }

        /// <summary>
        /// Recupera todos os registros
        /// </summary>
        /// <returns>Todos os registros</returns>
        /// <response code="200">A listagem de tarefas</response>
        /// <response code="500">Houve um erro ao tentar listar os registros</response>
        [HttpGet("ObterTodos")]
        [Produces("application/json")]
        [SwaggerOperation(summary: "Recupera todos os registros", description: "Recupera todos os registros da entidade.")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TarefaViewModel>), Description = "Listagem de tarefas efetuada com sucesso.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemHttpResult), Description = "Houve um erro ao tentar listar os registros.")]
        public IActionResult ObterTodos()
        {
            // TODO: Buscar todas as tarefas no banco utilizando o EF
            try
            {
                var result = _service.ListAll(EnumTarefaCriteriaField.Todos, null, 100, 0);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return Problem(statusCode: 500, title: "Erro ao tentar listar os registros.", detail: ex.Message);
            }
        }

        /// <summary>
        /// Recupera todos os registros utilizando o título informado
        /// </summary>
        /// <param name="titulo">Texto contendo uma parte de um título de tarefa</param>
        /// <returns>Todos os registros</returns>
        /// <response code="200">A listagem de tarefas</response>
        /// <response code="400">Título informado inválido</response>
        /// <response code="500">Houve um erro ao tentar listar os registros</response>
        [HttpGet("ObterPorTitulo")]
        [Produces("application/json")]
        [SwaggerOperation(summary: "Recupera todos os registros contendo o título informado", description: "Recupera todos os registros da entidade com o título informado.")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TarefaViewModel>), Description = "Listagem de tarefas efetuada com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(MessageViewModel), Description = "Título informado inválido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemHttpResult), Description = "Houve um erro ao tentar listar os registros.")]
        public IActionResult ObterPorTitulo([SwaggerParameter(Description = "Título da tarefa.", Required = true)][FromQuery] String titulo)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            try
            {
                if(_routines.ContainsValue(titulo))
                {
                    var result = _service.ListAll(EnumTarefaCriteriaField.PorTitulo, titulo, 100, 0);
                    return Ok(result);
                }
                return BadRequest(new { message = "Título informado inválido." });
            }
            catch (ApplicationException ex)
            {
                return Problem(statusCode: 500, title: "Erro ao tentar listar os registros.", detail: ex.Message);
            }
        }

        /// <summary>
        /// Recupera todos os registros utilizando a data informada
        /// </summary>
        /// <param name="data">Data cadastrada na tarefa</param>
        /// <returns>Todos os registros</returns>
        /// <response code="200">A listagem de tarefas</response>
        /// <response code="400">Data informada inválida</response>
        /// <response code="500">Houve um erro ao tentar listar os registros</response>
        [HttpGet("ObterPorData")]
        [Produces("application/json")]
        [SwaggerOperation(summary: "Recupera todos os registros contendo a data informada", description: "Recupera todos os registros da entidade com a data informada.")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TarefaViewModel>), Description = "Listagem de tarefas efetuada com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(MessageViewModel), Description = "Data informada inválida.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemHttpResult), Description = "Houve um erro ao tentar listar os registros.")]
        public IActionResult ObterPorData([SwaggerParameter(Description = "Data de início da tarefa.", Required = true)][FromQuery] String data)
        {
            try
            {
                if(_routines.ContainsValue(data))
                {
                    if(DateTime.TryParse(data, out DateTime value))
                    {
                        var result = _service.ListAll(EnumTarefaCriteriaField.PorData, value, 100, 0);
                        return Ok(result);
                    }
                }
                return BadRequest(new { message = "Data informada inválida." });
            }
            catch (ApplicationException ex)
            {
                return Problem(statusCode: 500, title: "Erro ao tentar listar os registros.", detail: ex.Message);
            }
        }

        /// <summary>
        /// Recupera todos os registros utilizando o status
        /// </summary>
        /// <param name="status">Status da tarefa</param>
        /// <returns>Todos os registros</returns>
        /// <response code="200">A listagem de tarefas</response>
        /// <response code="400">Status informado inválido</response>
        /// <response code="500">Houve um erro ao tentar listar os registros</response>
        [HttpGet("ObterPorStatus")]
        [Produces("application/json")]
        [SwaggerOperation(summary: "Recupera todos os registros contendo o status informado", description: "Recupera todos os registros da entidade com o status informado.")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TarefaViewModel>), Description = "Listagem de tarefas efetuada com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(MessageViewModel), Description = "Status informado inválido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemHttpResult), Description = "Houve um erro ao tentar listar os registros.")]
        public IActionResult ObterPorStatus([SwaggerParameter(Description = "Status da tarefa.", Required = true)][FromQuery] String status)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            try
            {
                if(_routines.ContainsValue(status))
                {
                    var filter = status.ParseEnum<EnumStatusTarefa>(EnumStatusTarefa.Invalido);
                    if(filter != EnumStatusTarefa.Invalido)
                    {
                        var result = _service.ListAll(EnumTarefaCriteriaField.PorStatus, filter, 100, 0);
                        return Ok(result);
                    }
                }
                return BadRequest(new { message = "Status informado inválido." });
            }
            catch (ApplicationException ex)
            {
                return Problem(statusCode: 500, title: "Erro ao tentar listar os registros.", detail: ex.Message);
            }
        }

        /// <summary>
        /// Insere uma nova tarefa
        /// </summary>
        /// <param name="tarefa">Dados da tarefa</param>
        /// <returns>A url para consulta da tarefa criada</returns>
        /// <response code="201">Uri contendo dados da tarefa criada</response>
        /// <response code="400">Tarefa informada inválida</response>
        /// <response code="422">Erros na validação da tarefa</response>
        /// <response code="500">Houve um erro ao tentar incluir o registro</response>
        [HttpPost("Criar")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerOperation(summary: "Inclui uma nova tarefa", description: "Inclui uma nova tarefa no banco de dados.")]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(TarefaViewModel), Description = "Registro incluído com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(MessageViewModel), Description = "Tarefa informada inválida.")]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, Type = typeof(UnprocessableEntity<IDictionary<String, String[]>>), Description = "Houve um ou mais erros na validação da tarefa.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemHttpResult), Description = "Houve um erro ao tentar incluir a tarefa.")]
        public IActionResult Criar([FromBody] CreateTarefaViewModel tarefa)
        {
            try
            {
                if(tarefa!=null)
                {
                    // TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
                    var validate = _createValidation.Validate(tarefa);
                    if (validate.IsValid)
                    {
                        var createdId = _service.Insert(tarefa);
                        return CreatedAtAction(nameof(ObterPorId), new { id = createdId }, new TarefaViewModel() 
                            { 
                                Id = createdId, 
                                Titulo = tarefa.Titulo, 
                                Descricao = tarefa.Descricao, 
                                Data = tarefa.Data, 
                                Status = EnumStatusTarefa.Pendente.GetDescription() 
                            } 
                        );
                    }
                    return UnprocessableEntity(validate.ToDictionary());
                }
                return BadRequest(new { message = "Tarefa informada inválida." });
            }
            catch (ApplicationException ex)
            {
                return Problem(statusCode: 500, title: "Erro ao tentar incluir o registro.", detail: ex.Message);
            }
        }

        /// <summary>
        /// Altera a tarefa 
        /// </summary>
        /// <param name="id">Código de identificação da tarefa</param>
        /// <param name="tarefa">Dados da tarefa</param>
        /// <returns></returns>
        /// <response code="204">Tarefa alterada com sucesso</response>
        /// <response code="400">Tarefa informada inválida</response>
        /// <response code="422">Erros na validação da tarefa</response>
        /// <response code="500">Houve um erro ao tentar alterar o registro</response>
        [HttpPut("Atualizar")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerOperation(summary: "Altera uma tarefa existente", description: "Altera uma tarefa existente no banco de dados.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, Type = typeof(NoContent), Description = "Tarefa alterada com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(MessageViewModel), Description = "Id ou tarefa informados inválido.")]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, Type = typeof(UnprocessableEntity<IDictionary<String, String[]>>), Description = "Houve um ou mais erros na validação da tarefa.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemHttpResult), Description = "Houve um erro ao tentar alterar a tarefa.")]
        public IActionResult Atualizar([SwaggerParameter(Description = "Código de identificação da tarefa.", Required = true)][FromQuery]String id, [FromBody] TarefaViewModel tarefa)
        {
            try
            {
                if(_routines.ContainsValue(id) && tarefa != null)
                {
                    if(Int32.TryParse(id, out Int32 value))
                    {
                        // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
                        // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)

                        var exists = _service.Exists(value);
                        if ((exists))
                        {
                            var validate = _updateValidation.Validate(tarefa);
                            if (validate.IsValid)
                            {
                                tarefa.Id = value;
                                _service.Update(value, tarefa);
                                return NoContent();
                            }
                            return UnprocessableEntity(validate.ToDictionary());
                        }
                        return NotFound();
                    }
                }
                return BadRequest(new { message = "Id ou tarefa informados inválido." });
            }
            catch (ApplicationException ex)
            {
                return Problem(statusCode: 500, title: "Erro ao tentar alterar o registro.", detail: ex.Message);
            }
        }

        /// <summary>
        /// Exclui a tarefa
        /// </summary>
        /// <param name="id">Código de identificação da tarefa</param>
        /// <returns></returns>
        /// <response code="204">Tarefa excluída com sucesso</response>
        /// <response code="400">Id informado inválido</response>
        /// <response code="500">Houve um erro ao tentar excluir o registro</response>
        [HttpDelete("Deletar")]
        [Produces("application/json")]
        [SwaggerOperation(summary: "Exclui uma tarefa existente", description: "Exclui uma tarefa existente no banco de dados.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, Type = typeof(NoContent), Description = "Tarefa excluída com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(MessageViewModel), Description = "Id informado inválido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemHttpResult), Description = "Houve um erro ao tentar excluir a tarefa.")]
        public IActionResult Deletar([SwaggerParameter(Description = "Código de identificação da tarefa.", Required = true)][FromQuery]String id)
        {
            try
            {
                if(_routines.ContainsValue(id))
                {
                    if(Int32.TryParse(id, out Int32 value))
                    {
                        // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
                        var exists = _service.Exists(value);
                        if (exists)
                        {
                            _service.Delete(value);
                            return NoContent();
                        }
                        return NotFound();
                    }
                }
                return BadRequest(new { message = "Id informado inválido." });
            }
            catch (ApplicationException ex)
            {
                return Problem(statusCode: 500, title: "Erro ao tentar excluir o registro.", detail: ex.Message);
            }
        }
    }
}
