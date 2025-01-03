using AmayaTest.Scripts.General.Extensions;
using DG.Tweening;
using UnityEngine;

namespace AmayaTest.Scripts.General.ProceduralAnimations.DoTween
{
    public class AnimationController : Object, IAnimationController
    {
        private readonly Sequence _sequence;
        private readonly Transform _transform;

        private AnimationController(){}
        
        public AnimationController(Sequence sequence, Transform transform)
        {
            sequence.ThrowIfNull(nameof(sequence));
            transform.ThrowIfNull(nameof(transform));
            
            _sequence = sequence;
            _transform = transform;
        }

        public void StopAnimation()
        {
            if (_sequence is { active: true }) 
            {
                _transform.DOKill();
            }
        }
    }
}