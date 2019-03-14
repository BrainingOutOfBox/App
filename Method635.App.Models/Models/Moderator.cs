namespace Method635.App.Models
{
    public class Moderator : Participant
    {
        public Moderator() { }
        public Moderator(Participant p)
        {
            FirstName = p.FirstName;
            LastName = p.LastName;
            UserName = p.UserName;
            Password = p.Password;
        }
    }
}
