using Atm.Clientes.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atm.Clientes.Dados.Extensions.Tables
{
    internal static class ClienteExtensions
    {
        internal static void SetupCliente(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                        .HasIndex(c => c.Id);

            modelBuilder.Entity<Cliente>()
                        .Property(c => c.Nome)
                        .HasMaxLength(60)
                        .IsRequired();

            modelBuilder.Entity<Cliente>()
                        .Property(c => c.Email)
                        .HasMaxLength(30);

            modelBuilder.Entity<Cliente>()
                        .Property(c => c.Telefone)
                        .HasMaxLength(50);

            modelBuilder.Entity<Cliente>()
                        .Property(c => c.Endereco)
                        .HasMaxLength(200);
        }
    }
}
