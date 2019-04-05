namespace Method635.App.Dal.Mapping
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
