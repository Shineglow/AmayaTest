using AmayaTest.Scripts.General.UI;

namespace AmayaTest.Scripts.Gameplay.Quiz.GridQuiz.UI
{
    public interface IQuizUIView : IView
    {
        void SetQuestion(string questionText);
    }
}