using MediatR;
using System;

namespace Atm.Clientes.Api.Features.Clientes.Queries
{
    public class SelecionarClienteByIdQuery : IRequest<SelecionarClienteByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }

    public class SelecionarClienteByIdQueryResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
