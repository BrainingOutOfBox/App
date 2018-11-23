﻿using AutoMapper;
using Method635.App.Forms.BusinessModels;
using Method635.App.Forms.Dto;
using Method635.App.Forms.RestAccess.ResponseModel;
using Method635.App.Forms.RestAccess.RestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Method635.App.Forms.RestAccess
{
    public class BrainstormingFindingRestResolver : RestResolverBase
    {
        private const string FINDINGS_ENDPOINT = "Finding";
        private const string CREATE_FINDING_ENDPOINT = "createBrainstormingFinding";
        private const string START_FINDING_ENDPOINT = "startBrainstorming";
        private const string TIMING_ENDPOINT_DIFF = "remainingTime";
        private const string GET_FINDINGS_ENDPOINT = "getBrainstormingFindings";
        private const string GET_FINDING_ENDPOINT = "getBrainstormingFinding";

        public string GetRemainingTime(string findingId, string teamId)
        {
            try
            {
                Console.WriteLine("Getting remaining time..");
                HttpResponseMessage response = GetCall($"{FINDINGS_ENDPOINT}/{teamId}/{findingId}/{TIMING_ENDPOINT_DIFF}");

                if (response.IsSuccessStatusCode)
                {
                    var remainingTimeInMs = response.Content.ReadAsAsync<long>().Result;
                    var timeRemaining = TimeSpan.FromMilliseconds(remainingTimeInMs);
                    return ($"{timeRemaining.Minutes:D2}m:{timeRemaining.Seconds:D2}s");
                }
                else
                {
                    Console.WriteLine($"{(int)response.StatusCode} ({response.ReasonPhrase})");
                    return string.Empty;
                }
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Error getting remaining time: {ex}");
            }
            return string.Empty;
        }

        public List<BrainstormingFinding> GetAllFindingsForTeam(string teamId)
        {
            try
            {
                Console.WriteLine($"Getting all brainstorming findings for team {teamId}..");
                var res = GetCall($"{FINDINGS_ENDPOINT}/{teamId}/{GET_FINDINGS_ENDPOINT}");
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Got all Brainstormingfindings finding. Content: {res.Content}");
                    var brainstormingFindings = res.Content.ReadAsAsync<List<BrainstormingFindingDto>>().Result;

                    var mappedFindings = brainstormingFindings.Select(f => Mapper.Map<BrainstormingFinding>(f)).ToList();

                    Console.WriteLine("got findings: ");
                    mappedFindings.ForEach(finding => Console.WriteLine(finding.Name));
                    return mappedFindings;
                }
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Failed to create brainstorming finding: {ex.Message}");
            }
            Console.WriteLine($"No brainstorming findings found for team {teamId}");
            return new List<BrainstormingFinding>();
        }

        internal BrainstormingFinding GetFinding(BrainstormingFinding finding)
        {
            try
            {
                Console.WriteLine("Getting brainstorming finding..");
                var res = GetCall($"{FINDINGS_ENDPOINT}/{finding.Id}/{GET_FINDING_ENDPOINT}");
                var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                Console.WriteLine(parsedResponseMessage.Title);
                Console.WriteLine(parsedResponseMessage.Text);
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Started brainstorming finding. Content: {res.Content}");
                    var findingDto = res.Content.ReadAsAsync<BrainstormingFindingDto>().Result;
                    return Mapper.Map<BrainstormingFinding>(findingDto);
                }
                else
                {
                    Console.WriteLine("The brainstorming finding couldn't be started.");
                }
            }
            catch(RestEndpointException ex)
            {
                Console.WriteLine($"There was an error getting the finding {finding.Id}");
            }
            return finding;
        }

        public BrainstormingFinding CreateBrainstormingFinding(BrainstormingFinding finding)
        {
            try
            {
                Console.WriteLine("Creating brainstorming finding..");

                var findingDto = Mapper.Map<BrainstormingFindingDto>(finding);

                var res = PostCall(findingDto, $"{FINDINGS_ENDPOINT}/{finding.TeamId}/{CREATE_FINDING_ENDPOINT}");
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Created brainstorming finding. Content: {res.Content}");
                    var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                    finding.Id = parsedResponseMessage.Text;
                    return finding;
                }
                else
                {
                    Console.WriteLine("The brainstorming finding couldn't be created.");
                    var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                    Console.WriteLine(parsedResponseMessage.Title);
                    Console.WriteLine(parsedResponseMessage.Text);
                }
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Failed to create brainstorming finding: {ex.Message}");
            }
            return finding;
        }
        public bool StartBrainstormingFinding(string findingId)
        {
            try
            {
                Console.WriteLine("Starting brainstorming finding..");
                var res = GetCall($"{FINDINGS_ENDPOINT}/{findingId}/{START_FINDING_ENDPOINT}");
                var parsedResponseMessage = res.Content.ReadAsAsync<RestResponseMessage>().Result;
                Console.WriteLine(parsedResponseMessage.Title);
                Console.WriteLine(parsedResponseMessage.Text);
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Started brainstorming finding. Content: {res.Content}");
                    return true;
                }
                else
                {
                    Console.WriteLine("The brainstorming finding couldn't be started.");
                }
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Failed to create brainstorming finding: {ex.Message}");
            }
            return false;

        }
    }
}
