using Method635.App.Dal.Config;
using Method635.App.Dal.Config.JsonDto;
using Method635.App.Dal.Interfaces;
using Method635.App.Forms.RestAccess;
using Method635.App.Forms.RestAccess.ResponseModel;
using Method635.App.Forms.RestAccess.RestExceptions;
using Method635.App.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Method635.App.Dal.Resolver
{
    public class FileRestResolver : IFileDalService
    {
        private readonly FileEndpoints _fileEndpoints;
        private readonly IHttpClientService _clientService;
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public FileRestResolver(IConfigurationService configurationService, IHttpClientService httpClientService)
        {
            _fileEndpoints = configurationService.ServerConfig.FileEndpoints;
            _clientService = httpClientService;
        }
        public Task<Stream> Download(string fileId)
        {
            throw new NotImplementedException();
        }

        public string UploadFile(Stream stream)
        {
            if (stream == null)
            {
                _logger.Error("Can't upload empty stream.");
                return string.Empty;
            }
            try
            {
                var res = _clientService.PostCall(stream, $"{_fileEndpoints.FilesEndpoint}/{_fileEndpoints.UploadEndpoint}");
                if (res.IsSuccessStatusCode)
                {
                    var responseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                    var fileId = responseMessage.Text;
                    if (string.IsNullOrEmpty(fileId))
                    {
                        _logger.Error("Got empty fileid from backend");
                    }
                    return fileId;
                }
            }
            catch (RestEndpointException)
            {
                _logger.Error("There was an exception with the clientservice");
            }
            catch (Exception ex)
            {
                _logger.Error("Couldn't upload image", ex.Message);
            }
            return string.Empty;
        }
    }
}
