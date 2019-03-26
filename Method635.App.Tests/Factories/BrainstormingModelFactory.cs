using Method635.App.Forms.Context;
using Method635.App.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.Tests.Factories
{
    static class BrainstormingModelFactory
    {

        internal static BrainstormingContext CreateContext(int findingRound)
        {
            return new BrainstormingContext()
            {
                CurrentFinding = CreateFinding(findingRound),
                CurrentBrainstormingTeam = CreateTeam(),
                CurrentParticipant = CreateParticipant()
            };
        }

        private static Participant CreateParticipant()
        {
            return new Participant()
            {
                UserName = "dummy"
            };
        }

        private static BrainstormingTeam CreateTeam()
        {
            return new BrainstormingTeam()
            {
                Participants = new List<Participant>()
                {
                    CreateParticipant()
                }
            };
        }

        internal static BrainstormingFinding CreateFinding(int round)
        {
            return new BrainstormingFinding()
            {
                CurrentRound = round,
                BrainSheets = CreateBrainSheets(),
            };
        }

        private static List<BrainSheet> CreateBrainSheets()
        {
            return new List<BrainSheet>()
            {
                new BrainSheet()
                {
                    BrainWaves = new List<BrainWave>()
                }
            };
        }
    }
}
