namespace Method635.App.Forms.Dto
{
    public class ModeratorDto : ParticipantDto
    {
        public ModeratorDto() { }
        public ModeratorDto(ParticipantDto p)
        {
            this.FirstName = p.FirstName;
            this.LastName = p.LastName;
            this.UserName = p.UserName;
            this.Password = p.Password;
        }
    }
}
