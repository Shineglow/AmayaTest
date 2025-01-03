using UnityEngine;

namespace AmayaTest.Scripts.Data
{
    [CreateAssetMenu(menuName = "AmayaTest/Resources/GridConfigurationSO", fileName = "GridConfigurationSO")]
    public class GridConfigurationSO : ScriptableObject
    {
        [field: SerializeField, Min(1)] public int Width { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int Height { get; private set; } = 1;
        [field: SerializeField] public Color BorderColor { get; private set; } = new Color(0.8f,0.45f,0.3f);
        [field: SerializeField] public float CellSizeInUnits { get; private set; } = 1f;
        [field: SerializeField] public float BorderThicknessInUnits { get; private set; } = 0.1f;
        [field: SerializeField] public Vector2 GridOffset { get; private set; }
    }
}