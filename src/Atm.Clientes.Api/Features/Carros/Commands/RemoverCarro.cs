using Atm.Clientes.Api.Extensions.Entities;
using Atm.Clientes.Domain;
using Atm.Clientes.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atm.Clientes.Api.Features.Carros.Commands
{
    public class RemoverCarroCommand : IRequest<RemoverCarroCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class RemoverCarroCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class RemoverCarroCommandHandler : IRequestHandler<RemoverCarroCommand, RemoverCarroCommandResponse>
    {
        private readonly IRepository<Carro> _repository;
        private readonly RemoverCarroCommandValidator _validator;

        public RemoverCarroCommandHandler(IRepository<Carro> repository, RemoverCarroCommandValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<RemoverCarroCommandResponse> Handle(RemoverCarroCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Carro entity = await GetCarroAsync(request);
            await RemoveCarroAsync(entity);

            return entity.ToRemoveResponse();
        }

        private async Task<Carro> GetCarroAsync(RemoverCarroCommand request)
        {
            Carro entity = await _repository.GetFirstAsync(c => c.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, entity);
            return entity;
        }

        private async Task RemoveCarroAsync(Carro entity)
        {
            entity.Ativo = false;
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }
    }

    public class RemoverCarroCommandValidator : AbstractValidator<RemoverCarroCommand>
    {
        public RemoverCarroCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id de veículo é obrigatório");
        }

        public async Task ValidateDataAsync(RemoverCarroCommand request, Carro entity)
        {
            RuleFor(r => r.Id)
                .Must(c => { return entity != null; })
                .WithMessage($"Veículo de id {request.Id} não encontrado");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
