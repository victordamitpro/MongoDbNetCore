﻿using Electric.Application.Responses;
using MediatR;

namespace Electric.Application.Commands
{
    public class CreateDeviceCommand: IRequest<Response<string>>
    {
        public string SeriaNumber { get; set; }
        public string FirmwareVersion { get; set; }
        public string State { get; set; }
        public int Type { get; set; }
        public GateWayCommand GateWay { get; set; }
    }

    public class GateWayCommand
    {
        public string IP { get; set; }
        public int? Port { get; set; }
    }
}
