using Method635.App.Models;

namespace Method635.App.Forms.Models
{
    public class PatternIdeaModel : PatternIdea
    {
        public PatternIdeaModel(PatternIdea p)
        {
            PictureId = p.PictureId;
            Problem = p.Problem;
            Solution = p.Solution;
            Url = p.Url;
            Description = p.Description;
            Category = p.Category;
        }
    }
}
