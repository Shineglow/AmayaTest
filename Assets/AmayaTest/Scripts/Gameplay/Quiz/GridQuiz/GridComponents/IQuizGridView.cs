using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AmayaTest.Scripts.Data;
using AmayaTest.Scripts.General.ProceduralAnimations;
using UnityEngine;

namespace AmayaTest.Scripts.Gameplay.Quiz.GridQuiz.GridComponents
{
    public interface IQuizGridView
    {
        void SetPositionToItem(SpriteResource spriteResource, Transform objTransform);
        void SetItemsCanHandleMouseEvents(bool value);
        Task AnimateItem(SpriteResource resource, IProceduralAnimation animation);

        Task LoadData(List<(SpriteResource resource, Color backgroundColor)> data, 
            Action<SpriteResource> onClickAction, 
            int width, 
            int height, 
            float cellSizeInUnits, 
            float borderThicknessInUnits, 
            Vector2 gridOffset, 
            Color borderColor,
            bool skipAnimation);
    }
}