using Method635.App.Models;
using Xamarin.Forms;

namespace Method635.App.Forms.Views.Brainstorming.TemplateSelector
{
    public class IdeaTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NoteIdeaTemplate { get; set; }
        public DataTemplate SketchIdeaTemplate { get; set; }
        public DataTemplate PatternIdeaTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is NoteIdea)
                return NoteIdeaTemplate;
            if (item is SketchIdea)
                return SketchIdeaTemplate;
            if (item is PatternIdea)
                return PatternIdeaTemplate;

            return null;
        }
    }
}
