namespace Method635.App.Forms.BusinessModels
{
    public class Moderator : Participant
    {
        public Moderator() { }
        public Moderator(Participant p)
        {
            this.FirstName = p.FirstName;
            this.LastName = p.LastName;
            this.UserName = p.UserName;
            this.Password = p.Password;
        }
    }
}
