using System.Collections.Generic;
using UnityEngine;

namespace AmayaTest.Scripts.Data
{
    [CreateAssetMenu(menuName = "AmayaTest/Resources/QuizSpriteDataSetSO", fileName = "QuizSpriteDataSetSO")]
    public class QuizSpriteDataSetSO : ScriptableObject
    {
        [SerializeField] private List<SpriteResource> set;
        public IReadOnlyList<SpriteResource> Set => set;
    }
}