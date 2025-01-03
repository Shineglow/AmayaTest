using UnityEngine;

namespace AmayaTest.Scripts.Data
{
    [CreateAssetMenu(menuName = "AmayaTest/Resources/ItemShakeParameters", fileName = "ItemShakeParameters")]
    public class ItemShakeParametersSO : ScriptableObject
    {
        [field: SerializeField] public float ShakeAmount {get;private set;}
        [field: SerializeField] public float Duration {get;private set;}
        [field: SerializeField] public int LoopsCount {get;private set;}
        [field: SerializeField] public Vector3 ResetPosition {get;private set;}
    }
}