using System.Collections.Generic;
using System.Threading.Tasks;
using AmayaTest.Scripts.Data;
using AmayaTest.Scripts.Gameplay.Quiz.GridQuiz;
using AmayaTest.Scripts.Gameplay.Quiz.GridQuiz.UI;
using AmayaTest.Scripts.Gameplay.UI;
using AmayaTest.Scripts.General.Extensions;
using AmayaTest.Scripts.General.ProceduralAnimations.DoTween.Shake;
using AmayaTest.Scripts.General.Utilities;
using UnityEngine;

namespace AmayaTest.Scripts.Gameplay.Quiz
{
    public class QuizGame
    {
        private const int INITIAL_LEVEL_VALUE = -1;
        private const string QUESTION_TEMPLATE = "Find {0}";
        
        private int _currentLevel;
        private string _question;
        private bool _isAnswerRight;
        private List<SpriteResource> _usedAnswers;
        private SpriteResource _currentAnswer;
        private HorizontalShake _horizontalShake;

        private GameModeSO _gameMode;
        private readonly QuizGrid _quizGrid;
        private readonly QuizGenerator _quizGenerator;
        private readonly ParticleSystem _rightAnswerParticleSystem;

        private readonly QuizViewsContainer _quizViewsContainer;
        private readonly IQuizUIView _quizUIView;
        private readonly IEndGameMenuUIView _endGameMenuUI;

        public QuizGame(QuizViewsContainer quizViewsContainer, ParticleSystem rightAnswerParticleSystem)
        {
            quizViewsContainer.ThrowIfNull(nameof(quizViewsContainer));
            quizViewsContainer.QuizGridView.ThrowIfNull(nameof(quizViewsContainer.QuizGridView));
            quizViewsContainer.QuizUIView.ThrowIfNull(nameof(quizViewsContainer.QuizUIView));
            quizViewsContainer.EndGameMenuUIView.ThrowIfNull(nameof(quizViewsContainer.EndGameMenuUIView));
            rightAnswerParticleSystem.ThrowIfNull(nameof(rightAnswerParticleSystem));

            _quizViewsContainer = quizViewsContainer;
            _quizGrid = new QuizGrid(quizViewsContainer.QuizGridView);
            _quizGenerator = new QuizGenerator();
            _quizUIView = quizViewsContainer.QuizUIView;
            _endGameMenuUI = quizViewsContainer.EndGameMenuUIView;
            _rightAnswerParticleSystem = rightAnswerParticleSystem;
            
            _endGameMenuUI.SetVisible(false);
            _quizViewsContainer.SetOverlayScreenBlockerIsVisible(false);
            
            ResetCurrentLevel();
        }

        private HorizontalShake GetShakeAnimation()
        {
            return _horizontalShake ??= new HorizontalShake()
                .SetShakeAmount(_gameMode.ItemShakeParametersSo.ShakeAmount)
                .SetReturnPosition(_gameMode.ItemShakeParametersSo.ResetPosition)
                .SetDuration(_gameMode.ItemShakeParametersSo.Duration)
                .SetLoopsCount(_gameMode.ItemShakeParametersSo.LoopsCount);
        }

        public void SetGameMode(GameModeSO gameMode, bool resetLevel = true)
        {
            _gameMode = gameMode;
            if (resetLevel)
            {
                ResetCurrentLevel();
            }
            if (_usedAnswers == null || _usedAnswers.Capacity < gameMode.LevelsQueue.Count)
            {
                _usedAnswers = new List<SpriteResource>(gameMode.LevelsQueue.Count);
            }
            else
            {
                _usedAnswers.Clear();
            }
        }

        private void ResetCurrentLevel()
        {
            _currentLevel = INITIAL_LEVEL_VALUE;
        }

        public async Task NextLevelAsync()
        {
            LevelConfiguration levelData = _gameMode.LevelsQueue[++_currentLevel];

            GridConfigurationSO gridConf = levelData.Level;
            ItemsBackgroundColorProperties colorProps = _gameMode.ItemsBackgroundColorProperties;
            List<(SpriteResource resource, Color backgroundColor)> dataSequence = _quizGenerator.GetQuizDataSequence(
                levelData.DataSet.Set, 
                _usedAnswers, 
                gridConf.Width * gridConf.Height, 
                (colorProps.Saturation, colorProps.Value));

            _currentAnswer = dataSequence.GetRandomItem().resource;
            _usedAnswers.Add(_currentAnswer);
            
            _quizUIView.SetQuestion(string.Format(QUESTION_TEMPLATE, _currentAnswer.InGameText));

            bool isFirstLevel = _currentLevel == 0;
            await _quizGrid.Initialize(gridConf, dataSequence, OnItemSelected, !isFirstLevel);
            
            _quizGrid.SetItemsCanHandleMouseEvents(true);
        }

        private void OnItemSelected(string inGameText)
        {
            _ = ProcessAnswerAsync(inGameText);
        }

        private async Task ProcessAnswerAsync(string inGameText)
        {
            bool isAnswerRight = _currentAnswer.InGameText == inGameText;
            
            await AnimateClickedItem(inGameText, isAnswerRight);

            if (isAnswerRight)
            {
                if (_currentLevel + 1 == _gameMode.LevelsQueue.Count)
                {
                    _endGameMenuUI.SetVisible(true);
                    _quizViewsContainer.SetOverlayScreenBlockerIsVisible(true);
                    _endGameMenuUI.OnRestartButtonClicked += OnRestartClicked;
                }
                else
                {
                    await NextLevelAsync();
                }
            }
            else
            {
                _quizGrid.SetItemsCanHandleMouseEvents(true);
            }
        }

        private async Task AnimateClickedItem(string inGameText, bool isAnswerRight)
        {
            List<Task> tasks = new List<Task>();
            _quizGrid.SetItemsCanHandleMouseEvents(false);
            tasks.Add(_quizGrid.ShakeItem(inGameText, GetShakeAnimation()));
            if (isAnswerRight)
            {
                _quizGrid.SetPositionToItem(inGameText, _rightAnswerParticleSystem.transform);
                _rightAnswerParticleSystem.Play();
                tasks.Add((TaskUtilities.WaitWhile(() => _rightAnswerParticleSystem.isPlaying)));
            }

            await Task.WhenAll(tasks);
        }

        private void OnRestartClicked()
        {
            _endGameMenuUI.SetVisible(false);
            _quizViewsContainer.SetOverlayScreenBlockerIsVisible(false);
            ResetCurrentLevel();
            _ = NextLevelAsync();
        }
    }
}