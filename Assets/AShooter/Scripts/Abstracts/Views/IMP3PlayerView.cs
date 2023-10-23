namespace Abstracts
{

    public interface IMP3PlayerView : IView
    {

        bool Ticker { get; set; }

        void ChangeText(string text);


    }
}
