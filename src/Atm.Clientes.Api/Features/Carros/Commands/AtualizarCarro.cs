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
    public class AtualizarCarroCommand : IRequest<AtualizarCarroCommandResponse>
    {
        public Guid Id { get; set; }
        public string Placa { get; set; }
        public string Descricao { get; set; }
        public long Quilometragem { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public short Ano { get; set; }
    }

    public class AtualizarCarroCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarCarroCommandHandler : IRequestHandler<AtualizarCarroCommand, AtualizarCarroCommandResponse>
    {
        private readonly IRepository<Carro> _repository;
        private readonly AtualizarCarroCommandValidator _validator;

        public AtualizarCarroCommandHandler(IRepository<Carro> repository, AtualizarCarroCommandValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<AtualizarCarroCommandResponse> Handle(AtualizarCarroCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException("Erro ao processar requisição");

            Carro entity = await GetCarroAsync(request);
            await UpdateCarroAsync(request, entity);

            return entity.ToUpdateResponse();
        }

        private async Task<Carro> GetCarroAsync(AtualizarCarroCommand request)
        {
            Carro carro = await _repository.GetFirstAsync(c => c.Id.Equals(request.Id));
            await _validator.ValidateDataAsync(request, carro);
            return carro;
        }

        private async Task UpdateCarroAsync(AtualizarCarroCommand request, Carro carro)
        {
            request.Update(carro);
            await _repository.UpdateAsync(carro);
            await _repository.SaveChangesAsync();
        }
    }

    public class AtualizarCarroCommandValidator : AbstractValidator<AtualizarCarroCommand>
    {
        public AtualizarCarroCommandValidator()
        {
            RuleFor(r => r.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do veículo é obrigatório");
            RuleFor(r => r.Placa)
                .NotEmpty()
                .WithMessage("Placa de veículo é obrigatório");
        }

        public async Task ValidateDataAsync(AtualizarCarroCommand request, Carro entity)
        {
            RuleFor(r => r.Id)
                .Must(c => { return entity is not null; })
                .WithMessage($"Veículo de id {request.Id} não encontrado");
            await this.ValidateAndThrowAsync(request);
        }
    }
}
