namespace TrilhaApiDesafio.TableConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using System;
    using TrilhaApiDesafio.Models;
    using static System.Net.WebRequestMethods;

    /// <summary>
    /// Classe que representa o mapeamento da entidade tarefa para o banco de dados
    /// </summary>
    public class TarefaConfiguration : IEntityTypeConfiguration<Tarefa>
    {
        /// <summary>
        /// Rotina para configuração do mapeamento da entidade para o banco de dados
        /// </summary>
        /// <param name="builder">Api contendo métodos para o mapeamento da classe para o banco de dados</param>
        /// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=fluent-api"/>
        public void Configure(EntityTypeBuilder<Tarefa> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            var converter = new ValueConverter<EnumStatusTarefa, String>(
                    v => v.ToString(),
                    v => (EnumStatusTarefa)Enum.Parse(typeof(EnumStatusTarefa), v)
            );

            builder.ToTable("Tarefas");
            builder.HasKey(e => e.Id).HasName("PK_TAREFAS");
            builder.Property<Int32>(e => e.Id).HasColumnName("Id").IsRequired();
            builder.Property<String>(e => e.Titulo).HasColumnName("Titulo").HasMaxLength(50).IsRequired();
            builder.Property<String>(e => e.Descricao).HasColumnName("Descricao").HasMaxLength(200).IsRequired();
            builder.Property<DateTime>(e => e.Data).HasColumnName("Data").IsRequired();
            builder.Property(e => e.Status).HasColumnType("VARCHAR(15)").HasColumnName("Status").IsRequired().HasConversion(converter);
        }
    }
}
