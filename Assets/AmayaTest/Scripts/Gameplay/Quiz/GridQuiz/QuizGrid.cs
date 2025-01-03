using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AmayaTest.Scripts.Data;
using AmayaTest.Scripts.Gameplay.Quiz.GridQuiz.GridComponents;
using AmayaTest.Scripts.General.ProceduralAnimations;
using UnityEngine;

namespace AmayaTest.Scripts.Gameplay.Quiz.GridQuiz
{
    public class QuizGrid
    {
        private readonly IQuizGridView _view;
        
        private List<(SpriteResource resource, Color backgroundColor)> _quizData;
        private List<SpriteResource> _usedAnswers;
        private SpriteResource _lastFindedResource;
        
        private event Action<string> OnAnswerSelected;

        public QuizGrid(IQuizGridView view)
        {
            _view = view;
        }

        public async Task Initialize(
            GridConfigurationSO gridConfiguration, 
            List<(SpriteResource resource, Color backgroundColor)> quizData, 
            Action<string> onAnswerSelected, 
            bool skipAnimation = true)
        {
            _quizData = quizData;
            
            await _view.LoadData(
                quizData, 
                OnItemClicked, 
                gridConfiguration.Width,
                gridConfiguration.Height,
                gridConfiguration.CellSizeInUnits,
                gridConfiguration.BorderThicknessInUnits,
                gridConfiguration.GridOffset,
                gridConfiguration.BorderColor,
                skipAnimation);
            OnAnswerSelected = onAnswerSelected;
        }

        private void OnItemClicked(SpriteResource resource)
        {
            OnAnswerSelected?.Invoke(resource.InGameText);
        }

        public async Task ShakeItem(string inGameText, IProceduralAnimation proceduralAnimation)
        {
            try
            {
                await _view.AnimateItem(FindResourceByInGameText(inGameText), proceduralAnimation);
            }
            catch (NullReferenceException e)
            {
                Debug.LogException(e);
            }
            catch (DataException e)
            {
                Debug.LogException(e);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void SetItemsCanHandleMouseEvents(bool value)
        {
            _view.SetItemsCanHandleMouseEvents(value);
        }

        public void SetPositionToItem(string inGameText, Transform transform)
        {
            _view.SetPositionToItem(FindResourceByInGameText(inGameText), transform);
        }

        private SpriteResource FindResourceByInGameText(string inGameText)
        {
            if (_lastFindedResource.InGameText == null || !_lastFindedResource.InGameText.Equals(inGameText))
            {
                _lastFindedResource = _quizData.Where(i => i.resource.InGameText.Equals(inGameText)).ElementAtOrDefault(0).resource;
            }
            
            return _lastFindedResource;
        }
    }
}
