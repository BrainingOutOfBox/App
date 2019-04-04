using Method635.App.Models;

namespace Method635.App.Dal.Interfaces
{
    public interface IParticipantDalService
    {
        Participant Login(Participant loginParticipant);
        bool CreateParticipant(Participant newParticipant);
    }
}