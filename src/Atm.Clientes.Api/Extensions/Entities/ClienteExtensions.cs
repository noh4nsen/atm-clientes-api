using Atm.Clientes.Api.Features.Clientes.Commands;
using Atm.Clientes.Domain;
using System;
using System.Collections.Generic;

namespace Atm.Clientes.Api.Extensions.Entities
{
    public static class ClienteExtensions
    {
        public static Cliente ToDomain(this InserirClienteCommand request)
        {
            return new Cliente()
            {
                Nome = request.Nome,
                Email = request.Email,
                Cpf = request.Cpf,
                Telefone = request.Telefone,
                Endereco = request.Endereco,
                Carros = request.CarrosToDomain()
            };
        }

        private static ICollection<Carro> CarrosToDomain(this InserirClienteCommand request)
        {
            if (request.Carros.Count <= 0)
                return new List<Carro>();

            IList<Carro> carros = new List<Carro>();
            foreach (CarroDto carro in request.Carros)
            {
                carros.Add(new Carro
                {
                    Id = carro.IdCarro ?? Guid.Empty,
                    Placa = carro.Placa,
                    Descricao = carro.Descricao,
                    Quilometragem = carro.Quilometragem,
                    Modelo = carro.Modelo,
                    Ano = carro.Ano
                });
            }

            return carros;
        }
    }
}
