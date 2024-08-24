namespace TrilhaApiDesafio.Context
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using TrilhaApiDesafio.Models;
    using TrilhaApiDesafio.TableConfigurations;

    /// <summary>
    /// Classe que implementa o acesso ao banco de dados utilizado pelo sistema
    /// </summary>
    /// <param name="options">Configura��es do servidor de dados</param>
    public class OrganizadorContext(DbContextOptions<OrganizadorContext> options) : DbContext(options)
    {
        /// <summary>
        /// Rotina utilizada no mapeamento e inicializa��o do banco de dados
        /// </summary>
        /// <param name="modelBuilder">Classe respons�vel pelas rotinas para o mapeamento das entidades para uma tabela no banco de dados</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<Tarefa>(new TarefaConfiguration());
        }

        /// <summary>
        /// Representa��o da entidade tarefas no banco de dados
        /// </summary>
        public DbSet<Tarefa> Tarefas { get; set; }
    }
}