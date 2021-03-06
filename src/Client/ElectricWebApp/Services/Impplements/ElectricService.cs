﻿using ElectricWebApp.Models;
using ElectricWebApp.Services.Infrastructure;
using ElectricWebApp.Services.Interfaces;
using ElectricWebApp.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ElectricWebApp.Services.Impplements
{
    public class ElectricService : BaseHttpClientWithFactory, IElectricService
    {
        private readonly IApiSettings _settings;
        private readonly ILogger<ElectricService> _logger;

        public ElectricService(IHttpClientFactory factory, IApiSettings settings, ILogger<ElectricService> logger)
            : base(factory)
        {
            _settings = settings;
            _logger = logger;
        }

        public async Task<IEnumerable<DeviceViewModel>> GetAll()
        {
            _logger.LogInformation($"Processing Get All Devices");

            var message = new HttpRequestBuilder(_settings.BaseAddress)
                                .SetPath(_settings.ElectricPath)
                                .HttpMethod(HttpMethod.Get)
                                .GetHttpMessage();

            return await SendRequest<IEnumerable<DeviceViewModel>>(message);
        }

        public async Task<DeviceViewModel> GetDetail(string id)
        {
            _logger.LogInformation($"Processing Get Detail Device");

            var message = new HttpRequestBuilder(_settings.BaseAddress)
                                .SetPath(_settings.ElectricPath)
                                .AddToPath(id)
                                .HttpMethod(HttpMethod.Get)
                                .GetHttpMessage();

            return await SendRequest<DeviceViewModel>(message);
        }

        public async Task<ResponseDataModel<string>> Create(DeviceViewModel electricModel)
        {
            try
            {
                _logger.LogInformation($"Processing Create Device");

                var message = new HttpRequestBuilder(_settings.BaseAddress)
                                .SetPath(_settings.ElectricPath)
                                .HttpMethod(HttpMethod.Post)
                                .GetHttpMessage();

                var json = JsonConvert.SerializeObject(electricModel);
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");

                return await SendRequest<ResponseDataModel<string>>(message);
            }
            //Todo: should catch specific exeption
            catch (Exception ex)
            {
                if (ex.Message.Contains("409"))
                {
                    _logger.LogError($"Duplicated Item Error: {ex.Message}");
                    return new ResponseDataModel<string> { Data = null, ErrorMessage = "Duplicated Record Found"};
                }
               
                throw;
            }
        }

        public async Task<object> Edit(DeviceViewModel electricModel)
        {
            _logger.LogInformation($"Processing Update Device");

            var message = new HttpRequestBuilder(_settings.BaseAddress)
                                .SetPath(_settings.ElectricPath)
                                .HttpMethod(HttpMethod.Put)
                                .GetHttpMessage();

            var json = JsonConvert.SerializeObject(electricModel);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return await SendRequest<object>(message);
        }

        public async Task<ResponseDataModel<string>> Delete(string id)
        {
            _logger.LogInformation($"Processing Delete Device");

            var message = new HttpRequestBuilder(_settings.BaseAddress)
                               .SetPath(_settings.ElectricPath)
                               .AddToPath(id)
                               .HttpMethod(HttpMethod.Delete)
                               .GetHttpMessage();

            return await SendRequest<ResponseDataModel<string>>(message);
        }
    }
}
