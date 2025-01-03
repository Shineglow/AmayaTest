using UnityEngine;

namespace AmayaTest.Scripts.General
{
    public class OrthogonalCameraInfo : MonoBehaviour
    {
        [SerializeField] 
        private new Camera camera;

        private Vector2 _sizeInUnits;
        private bool _isSizeInUnitsDirty = true;

        public Vector2 WorldPosition => camera.transform.position;

        public Vector2 GetOrthographicRectInUnits()
        {
            if (camera == null) return Vector2.zero;
        
            if (_isSizeInUnitsDirty)
            {
                _sizeInUnits.y = camera.orthographicSize * 2; // 2 cause camera.orthographicSize is half of its real size
                _sizeInUnits.x = _sizeInUnits.y * camera.aspect;

                _isSizeInUnitsDirty = false;
            }

            return _sizeInUnits;
        }

        public override string ToString()
        {
            return GetOrthographicRectInUnits().ToString();
        }
    }
}
