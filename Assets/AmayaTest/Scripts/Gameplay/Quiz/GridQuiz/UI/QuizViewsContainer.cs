using AmayaTest.Scripts.Gameplay.Quiz.GridQuiz.GridComponents;
using AmayaTest.Scripts.Gameplay.UI;
using UnityEngine.UI;

namespace AmayaTest.Scripts.Gameplay.Quiz.GridQuiz.UI
{
    public class QuizViewsContainer
    {
        private readonly QuizGridView _quizGridView;
        private readonly QuizUIView _quizUIView;
        private readonly EndGameMenuUIView _endGameMenuUIView;
        private readonly Image _overlayScreenBlocker;
        
        public IQuizGridView QuizGridView => _quizGridView;
        public IQuizUIView QuizUIView => _quizUIView;
        public IEndGameMenuUIView EndGameMenuUIView => _endGameMenuUIView;

        public QuizViewsContainer(
            QuizGridView quizGridView,
            QuizUIView quizUIView,
            EndGameMenuUIView endGameMenuUIView,
            Image overlayScreenBlocker)
        {
            _quizGridView = quizGridView;
            _quizUIView = quizUIView;
            _endGameMenuUIView = endGameMenuUIView;
            _overlayScreenBlocker = overlayScreenBlocker;
        }

        public void SetOverlayScreenBlockerIsVisible(bool isVisible)
        {
            _overlayScreenBlocker.gameObject.SetActive(isVisible);
        }
    }
}