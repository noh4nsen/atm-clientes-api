using Atm.Clientes.Api.Extensions.Entities;
using Atm.Clientes.Domain;
using Atm.Clientes.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Clientes.Api.Features.Clientes.Commands
{
    public class RemoverClienteCommand : IRequest<RemoverClienteCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class RemoverClienteCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class RemoverClienteCommandHandler : IRequestHandler<RemoverClienteCommand, RemoverClienteCommandResponse>
    {
        private readonly IRepository<Cliente> _repository;
        private readonly RemoverClienteCommandValidator _validator;

        public RemoverClienteCommandHandler
            (
                IRepository<Cliente> repositoryCliente, 
                RemoverClienteCommandValidator validator
            )
        {
            _repository = repositoryCliente;
            _validator = validator;
        }

        public async Task<RemoverClienteCommandResponse> Handle(RemoverClienteCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Cliente entity = await _repository.GetFirstAsync(c => c.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, entity);
            await RemoverClienteAsync(entity);

            return entity.ToRemoveResponse();
        }

        public async Task RemoverClienteAsync(Cliente entity)
        {
            await Task.FromResult(_repository.RemoveAsync(entity)).Result;
            await Task.FromResult(_repository.SaveChangesAsync()).Result;
        }
    }

    public class RemoverClienteCommandValidator : AbstractValidator<RemoverClienteCommand>
    {
        public RemoverClienteCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de fornecedor é obrigatório");
        }

        public async Task ValidateDataAsync(RemoverClienteCommand request, Cliente entity)
        {
            RuleFor(r => r.Id)
               .Must(f => { return entity != null; })
               .WithMessage($"Fornecedor de id {request.Id} não encontrado.");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
