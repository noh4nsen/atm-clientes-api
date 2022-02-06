using Atm.Clientes.Api.Features.Clientes.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Atm.Clientes.Api.Features.Cliente
{
    [Route("cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClienteController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] InserirClienteCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
