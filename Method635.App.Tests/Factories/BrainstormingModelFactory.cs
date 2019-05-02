using Method635.App.BL.Context;
using Method635.App.Models;
using System.Collections.Generic;

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

        public static Participant CreateParticipant()
        {
            return new Participant()
            {
                UserName = "dummy"
            };
        }
        public static Moderator CreateModerator()
        {
            return new Moderator(CreateParticipant());
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

        public static List<BrainSheet> CreateBrainSheets()
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
