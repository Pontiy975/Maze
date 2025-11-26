using UISystem.Transitions;

namespace UISystem.Screens
{
    public interface ISlideableScreen
    {
        public SlideTransition.SlideData SlideData { get; }
    }
}
