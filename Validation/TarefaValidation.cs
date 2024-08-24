namespace TrilhaApiDesafio.Validation
{
    using FluentValidation;
    using System;
    using TrilhaApiDesafio.Extensions;
    using TrilhaApiDesafio.Helpers;
    using TrilhaApiDesafio.Models;
    using TrilhaApiDesafio.ViewModels;

    /// <summary>
    /// Classe responsável pela validação de uma alteração na entidade tarefa
    /// </summary>
    public class TarefaValidation : AbstractValidator<TarefaViewModel>
    {
        private readonly ApplicationRoutines _routines;
        /// <summary>
        /// Método construtor da classe
        /// </summary>
        public TarefaValidation(ApplicationRoutines routines)
        {
            ArgumentNullException.ThrowIfNull(routines);
            _routines = routines;

            RuleFor(e => e.Id).NotNull().NotEmpty().WithMessage("O campo id deve ser informado.");
            RuleFor(e => e.Id).Must(IsValidId).WithMessage("Id informado inválido.");

            RuleFor(e => e.Titulo).NotNull().NotEmpty().WithMessage("O campo título deve ser informado.");
            RuleFor(e => e.Titulo).Length(5, 50).WithMessage("O campo título deve ter entre 5 e 50 caracteres.");

            RuleFor(e => e.Descricao).NotNull().NotEmpty().WithMessage("O campo descrição deve ser informado.");
            RuleFor(e => e.Descricao).Length(10, 200).WithMessage("O campo descrição deve ter entre 10 e 200 caracteres.");

            RuleFor(e => e.Data).NotNull().NotEmpty().WithMessage("O campo data deve ser informado.");
            RuleFor(e => e.Data).Must(IsValidDate).WithMessage("Data informada inválida.");

            RuleFor(e => e.Status).NotNull().NotEmpty().WithMessage("O campo status deve ser informado.");
            RuleFor(e => e.Status).Must(IsValidStatus).WithMessage("Status informado inválido.");
        }

        /// <summary>
        /// Rotina para validação do campo data
        /// </summary>
        /// <param name="date">Valor a ser validado</param>
        /// <returns></returns>
        private Boolean IsValidDate(Nullable<DateTime> date)
        {
            if (date != null)
            {
                if (date > DateTime.Now.AddDays(-1))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Rotina para validação do campo status
        /// </summary>
        /// <param name="status">Valor a ser validado</param>
        /// <returns></returns>
        private Boolean IsValidStatus(String status)
        {
            if (_routines.ContainsValue(status))
            {
                var valid = status.ParseEnum<EnumStatusTarefa>(EnumStatusTarefa.Invalido);
                if (valid != EnumStatusTarefa.Invalido) return true;
            }
            return false;
        }

        /// <summary>
        /// Rotina para validação do campo id
        /// </summary>
        /// <param name="id">Valor a ser validado</param>
        /// <returns></returns>
        private Boolean IsValidId(Nullable<Int32> id)
        {
            if (id.HasValue)
            {
                if (id.Value > 0) return true;
            }
            return false;
        }
    }
}
