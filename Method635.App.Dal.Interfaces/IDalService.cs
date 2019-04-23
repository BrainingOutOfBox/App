namespace Method635.App.Dal.Interfaces
{
    public interface IDalService
    {
        IBrainstormingDalService BrainstormingDalService { get; }
        IParticipantDalService ParticipantDalService { get; }
        ITeamDalService TeamDalService { get; }
        IFileDalService FileDalService { get; }
    }
}
