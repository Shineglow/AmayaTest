using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmayaTest.Scripts.Data
{
    [CreateAssetMenu(menuName = "AmayaTest/Resources/GameModeSO", fileName = "GameModeSO")]
    public class GameModeSO : ScriptableObject
    {
        [field:SerializeField] public ItemShakeParametersSO ItemShakeParametersSo { get; private set; }
        [field:SerializeField] public ItemsBackgroundColorProperties ItemsBackgroundColorProperties { get; private set; }
        [SerializeField]
        private List<LevelConfiguration> levelsQueue;
        public IReadOnlyList<LevelConfiguration> LevelsQueue => levelsQueue;
    }

    [Serializable]
    public struct LevelConfiguration
    {
        [field: SerializeField]
        public GridConfigurationSO Level { get; private set; }
        [field: SerializeField]
        public QuizSpriteDataSetSO DataSet { get; private set; }
    }

    [Serializable]
    public struct ItemsBackgroundColorProperties
    {
        [SerializeField] public float Saturation;
        [SerializeField] public float Value;
    }
}