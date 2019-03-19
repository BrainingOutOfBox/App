namespace Method635.App.Dal.Config
{
    interface IServerConfig
    {
        Server Server { get; set; }
        BrainstormingEndpoints BrainstormingEndpoints { get; set; }
        ParticipantEndpoints ParticipantEndpoints { get; set; }
    }
}
