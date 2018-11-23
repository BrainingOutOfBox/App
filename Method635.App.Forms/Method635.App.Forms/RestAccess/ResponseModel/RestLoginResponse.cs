using Method635.App.Forms.BusinessModels;

namespace Method635.App.Forms.RestAccess.ResponseModel
{
    public class RestLoginResponse
    {
        public Participant Participant { get; set; }
        public string JwtToken { get; set; }
    }
}
