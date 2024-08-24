namespace TrilhaApiDesafio.Profiles
{
    using AutoMapper;
    using TrilhaApiDesafio.Extensions;
    using TrilhaApiDesafio.Models;
    using TrilhaApiDesafio.ViewModels;

    /// <summary>
    /// Classe responsável pelo mapeamento da entidade Tarefa 
    /// </summary>
    public class TarefaProfile  : Profile
    {
        /// <summary>
        /// Método construtor da classe
        /// </summary>
        public TarefaProfile()
        {
            CreateMap<CreateTarefaViewModel, Tarefa>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => EnumStatusTarefa.Pendente)
            );
            CreateMap<TarefaViewModel, Tarefa>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ParseEnum<EnumStatusTarefa>(EnumStatusTarefa.Invalido)));
            CreateMap<Tarefa, TarefaViewModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.GetDescription()));
        }
    }
}
