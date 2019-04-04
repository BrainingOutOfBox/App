using Method635.App.Models;

namespace Method635.App.BL.Interfaces
{
    public interface IParticipantService
    {
        Participant Login(Participant participant);
        bool Register(Participant participant);
    }
}
