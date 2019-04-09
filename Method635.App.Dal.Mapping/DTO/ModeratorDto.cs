namespace Method635.App.Dal.Mapping
{
    public class ModeratorDto : ParticipantDto
    {
        public ModeratorDto() { }
        public ModeratorDto(ParticipantDto p)
        {
            FirstName = p.FirstName;
            LastName = p.LastName;
            UserName = p.UserName;
            Password = p.Password;
        }
    }
}
