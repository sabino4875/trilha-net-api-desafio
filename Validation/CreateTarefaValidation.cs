namespace TrilhaApiDesafio.Validation
{
    using FluentValidation;
    using System;
    using TrilhaApiDesafio.ViewModels;

    /// <summary>
    /// Classe responsável pela validação de uma inclusão na entidade tarefa
    /// </summary>
    public class CreateTarefaValidation : AbstractValidator<CreateTarefaViewModel>
    {
        /// <summary>
        /// Método construtor da classe
        /// </summary>
        public CreateTarefaValidation()
        {
            RuleFor(e => e.Titulo).NotNull().NotEmpty().WithMessage("O campo título deve ser informado.");
            RuleFor(e => e.Titulo).Length(5,50).WithMessage("O campo título deve ter entre 5 e 50 caracteres.");

            RuleFor(e => e.Descricao).NotNull().NotEmpty().WithMessage("O campo descrição deve ser informado.");
            RuleFor(e => e.Descricao).Length(10, 200).WithMessage("O campo descrição deve ter entre 10 e 200 caracteres.");

            RuleFor(e => e.Data).NotNull().NotEmpty().WithMessage("O campo data deve ser informado.");
            RuleFor(e => e.Data).Must(IsValidDate).WithMessage("Data informada inválida.");
        }

        /// <summary>
        /// Rotina para validação do campo data
        /// </summary>
        /// <param name="date">Valor a ser validado</param>
        /// <returns></returns>
        private Boolean IsValidDate(Nullable<DateTime> date)
        {
            if(date!=null)
            {
                if(date > DateTime.Now.AddDays(-1))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
