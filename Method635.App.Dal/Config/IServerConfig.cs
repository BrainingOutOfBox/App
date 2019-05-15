using Method635.App.Dal.Config.JsonDto;

namespace Method635.App.Dal.Config
{
    public interface IServerConfig
    {
        Server Server { get; set; }
        BrainstormingEndpoints BrainstormingEndpoints { get; set; }
        ParticipantEndpoints ParticipantEndpoints { get; set; }
        TeamEndpoints TeamEndpoints { get; set; }
        FileEndpoints FileEndpoints { get; set; }
        PatternEndpoints PatternEndpoints { get; set; }
    }
}
