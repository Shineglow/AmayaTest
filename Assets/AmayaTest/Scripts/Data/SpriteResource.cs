using UnityEngine;

namespace AmayaTest.Scripts.Data
{
    [System.Serializable]
    public struct SpriteResource
    {
        [field: SerializeField] 
        public string InGameText { get; set; }
        [field: SerializeField]
        public Sprite Sprite { get; private set; }

        [field: SerializeField]
        public ESpriteRotationAngle RotationToTheRight{ get; private set; }
        
        [field: SerializeField]
        public bool FlipX{ get; private set; }
        
        [field: SerializeField]
        public bool FlipY{ get; private set; }

        // Я не собираюсь использовать данное перечисление за пределами данной структуры
        // поэтому определил её внутри структуры
        public enum ESpriteRotationAngle
        {
            Degrees0 = 0,
            Degrees90 = 90,
            Degrees180 = 180,
            Degrees270 = 270,
        }
    }
}