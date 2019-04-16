using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Method635.App.Forms.Views.Brainstorming.SpecialContent.Sketching.TouchEffect
{
    public class TouchEffect : RoutingEffect
    {
        public event TouchActionEventHandler TouchAction;

        public TouchEffect() : base("Method635.TouchEffect")
        {
        }

        public bool Capture { set; get; }

        public void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            TouchAction?.Invoke(element, args);
        }
    }
}
