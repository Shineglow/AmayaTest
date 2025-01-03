using System.Collections.Generic;
using System.Linq;
using AmayaTest.Scripts.Data;
using AmayaTest.Scripts.General;
using AmayaTest.Scripts.General.Utilities;
using UnityEngine;

namespace AmayaTest.Scripts.Gameplay.Quiz
{
    public class QuizGenerator
    {
        public List<(SpriteResource resource, Color backgroundColor)> GetQuizDataSequence(
            IReadOnlyList<SpriteResource> configurationData, 
            IReadOnlyList<SpriteResource> exclude, 
            int sequenceCount,
            (float saturation, float value) backgroundColorProperties)
        {
            List<SpriteResource> dataCopy = configurationData.Where(i => !exclude.Contains(i)).ToList();
            List<Color> colorsSet = ColorGenerator.GetColorSet(backgroundColorProperties.saturation, backgroundColorProperties.value);
            List<(SpriteResource resource, Color backgroundColor)> result = new List<(SpriteResource resource, Color backgroundColor)>();

            for (int i = 0; i < sequenceCount; i++)
                result.Add((dataCopy.GetRandomRemovedItem(), colorsSet.GetRandomItem()));

            return result;
        }
    }
}