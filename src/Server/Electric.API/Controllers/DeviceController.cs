﻿using Electric.Application.Commands;
using Electric.Application.Queries;
using Electric.Application.Responses;
using Electric.Core.Entities;
using Electric.Core.Exeptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Electric.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDeviceQuery _filterDeviceQuery;

        public DeviceController(IMediator mediator, IDeviceQuery filterDeviceQuery)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _filterDeviceQuery = filterDeviceQuery ?? throw new ArgumentNullException(nameof(filterDeviceQuery)); ;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GateWayResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<GateWayResponse>>> GetAll()
        {
            var result = await _filterDeviceQuery.GetAlls();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GateWayResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DeviceResponse>>> GetDetail(string id)
        {
            var result = await _filterDeviceQuery.GetDetail(id);

            if (result == null)
                throw new NotFoundExeption("Could not found record.");

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDevice([FromBody] CreateDeviceCommand command)
        {
            var duplicateElectric = await _filterDeviceQuery.GetDuplicateItems(command.SeriaNumber);

            if (duplicateElectric != null)
                throw new ConflictExeption("Can not insert same record.");

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Response<Device>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateElectric(UpdateDeviceCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string id)
        {
            var deleteElectricCommand = new DeleteDeviceCommand { Id = id };

            var result = await _mediator.Send(deleteElectricCommand);

            return Ok(result);
        }
    }
}
