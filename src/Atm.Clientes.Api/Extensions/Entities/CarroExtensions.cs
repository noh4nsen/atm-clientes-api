﻿using Atm.Clientes.Api.Features.Carros.Commands;
using Atm.Clientes.Api.Features.Carros.Queries;
using Atm.Clientes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atm.Clientes.Api.Extensions.Entities
{
    public static class CarroExtensions
    {
        public static Carro ToDomain(this InserirCarroCommand request, Cliente cliente)
        {
            IList<Cliente> clientes = new List<Cliente>();
            clientes.Add(cliente);
            return new Carro()
            {
                Placa = request.Placa,
                Descricao = request.Descricao,
                Quilometragem = request.Quilometragem,
                Modelo = request.Modelo,
                Marca = request.Marca,
                Ano = request.Ano,
                Clientes = clientes
            };
        }

        public static InserirCarroCommandResponse ToInsertResponse(this Carro entity)
        {
            return new InserirCarroCommandResponse()
            {
                Id = entity.Id,
                DataCadastro = entity.DataCadastro
            };
        }

        public static void Update(this AtualizarCarroCommand request, Carro entity)
        {
            entity.Ativo = true;
            entity.Placa = request.Placa;
            entity.Descricao = request.Descricao;
            entity.Quilometragem= request.Quilometragem;
            entity.Modelo = request.Modelo;
            entity.Marca = request.Marca;
            entity.Ano = request.Ano;
        }

        public static AtualizarCarroCommandResponse ToUpdateResponse(this Carro entity)
        {
            return new AtualizarCarroCommandResponse()
            {
                DataAtualizacao = (DateTime)(entity.DataAtualizacao)
            };
        }

        public static RemoverCarroCommandResponse ToRemoveResponse(this Carro entity)
        {
            return new RemoverCarroCommandResponse()
            {
                Id = entity.Id
            };
        }

        public static SelecionarCarroByIdQueryResponse ToQueryResponse(this Carro entity)
        {
            entity.Clientes.ToList().ForEach(c => c.Carros = null);
            return new SelecionarCarroByIdQueryResponse()
            {
                Id = entity.Id,
                Ativo = entity.Ativo,
                Placa = entity.Placa,
                Descricao = entity.Descricao,
                Quilometragem = entity.Quilometragem,
                Modelo = entity.Modelo,
                Marca = entity.Marca,
                Ano = entity.Ano,
                Clientes = entity.Clientes
            };
        }

        public static IEnumerable<SelecionarCarroByIdQueryResponse> ToFiltersQueryResponse(this IEnumerable<Carro> list)
        {
            IList<SelecionarCarroByIdQueryResponse> response = new List<SelecionarCarroByIdQueryResponse>();
            foreach (Carro entity in list)
                response.Add(entity.ToQueryResponse());
            return response;
        }
    }
}
