using AmayaTest.Scripts.Data;
using AmayaTest.Scripts.Gameplay.Quiz;
using AmayaTest.Scripts.Gameplay.Quiz.GridQuiz.GridComponents;
using AmayaTest.Scripts.Gameplay.Quiz.GridQuiz.UI;
using AmayaTest.Scripts.Gameplay.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AmayaTest.Scripts.General
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private QuizGridView quizGridView;
        [SerializeField] private QuizUIView quizUIView;
        [SerializeField] private GameModeSO gameMode;
        [SerializeField] private EndGameMenuUIView endGameMenuUIView;
        [SerializeField] private Image backgroundBlocker;
        [SerializeField] private ParticleSystem rightAnswerParticleSystem;
        private QuizGame _quizGame;

        private void Awake()
        {
            _quizGame = new QuizGame(new QuizViewsContainer(quizGridView, quizUIView, endGameMenuUIView, backgroundBlocker), rightAnswerParticleSystem);
        }

        private void Start()
        {
            _quizGame.SetGameMode(gameMode);
            _ = _quizGame.NextLevelAsync();
        }
    }
}
