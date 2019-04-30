using AutoMapper;
using Method635.App.Dal.Config;
using Method635.App.Dal.Config.JsonDto;
using Method635.App.Dal.Interfaces;
using Method635.App.Dal.Mapping.DTO;
using Method635.App.Forms.RestAccess;
using Method635.App.Forms.RestAccess.RestExceptions;
using Method635.App.Logging;
using Method635.App.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace Method635.App.Dal.Resolver
{
    public class PatternRestResolver : IPatternDalService
    {
        private readonly PatternEndpoints _patternConfig;
        private readonly IHttpClientService _clientService;
        private readonly IMapper _patternMapper;
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public PatternRestResolver(IConfigurationService configurationService, IHttpClientService httpClientService, IMapper mapper)
        {
            _patternConfig = configurationService.ServerConfig.PatternEndpoints;
            _clientService = httpClientService;
            _patternMapper = mapper;
        }
        public List<PatternIdea> GetAllPatterns()
        {

            try
            {
                _logger.Info($"Getting all patterns");
                HttpResponseMessage response = _clientService.GetCall($"{_patternConfig.PatternEndpoint}/{_patternConfig.GetAllEndpoint}");

                if (response.IsSuccessStatusCode)
                {
                    var patternsDto = response.Content.ReadAsAsync<List<PatternIdeaDto>>().Result;
                    _logger.Info($"Got {patternsDto.Count} patterns from backend");
                    var teamlist = _patternMapper.Map<List<PatternIdea>>(patternsDto);
                    return teamlist;
                }
                else
                {
                    _logger.Error($"Response Code from GetAllPatterns unsuccessful: {(int)response.StatusCode} ({response.ReasonPhrase})");
                }
            }
            catch (RestEndpointException ex)
            {
                _logger.Error($"Error getting Patterns: {ex}", ex);
            }
            catch (UnsupportedMediaTypeException ex)
            {
                _logger.Error($"Error getting Patterns (unsupported media type in response): {ex}", ex);
            }
            catch (Exception ex)
            {
                _logger.Error($"Couldn't get all patterns from backend {ex}", ex);
            }
            return new List<PatternIdea>();
        }
    }
}
