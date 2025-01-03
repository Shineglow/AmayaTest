using System;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AmayaTest.Scripts.General.ProceduralAnimations.DoTween.Shake
{
    public class HorizontalShake : IProceduralAnimation
    {
        private float _shakeAmount = 1f;
        private float _duration = 0.5f;
        private int _loopsCount = 1;
        private Vector3 _resetPosition = Vector3.zero;

        private bool _isNeedToResetPosition = false;
        private Action _onAnimationEndAction;
        private AnimationController _actualController;

        public HorizontalShake SetShakeAmount(float shakeAmount)
        {
            _shakeAmount = shakeAmount;
            return this;
        }
        
        public HorizontalShake SetDuration(float duration)
        {
            _duration = duration;
            return this;
        }
        
        public HorizontalShake SetLoopsCount(int loopsCount)
        {
            _loopsCount = loopsCount;
            return this;
        }

        public HorizontalShake SetReturnPosition(Vector3 position)
        {
            _isNeedToResetPosition = true;
            _resetPosition = position;
            return this;
        }

        public HorizontalShake SetOnAnimationEndAction(Action onAnimationEndAction)
        {
            _onAnimationEndAction = onAnimationEndAction;
            return this;
        }
        
        public IAnimationController Animate(Transform transform, Action onAnimationEnd)
        {
            if (_actualController != null)
            {
                _actualController.StopAnimation();
                Object.Destroy(_actualController);
                _actualController = null;
            }

            const int partsOfAnimation = 4;
            float timePerPartCountLoops = _duration / (partsOfAnimation * _loopsCount);
            Sequence sequence = DOTween.Sequence()
                .Append(transform.DOLocalMoveX(_resetPosition.x - _shakeAmount, timePerPartCountLoops).SetEase(Ease.InBounce))
                .Append(transform.DOLocalMoveX(_resetPosition.x, timePerPartCountLoops).SetEase(Ease.InBounce))
                .Append(transform.DOLocalMoveX(_resetPosition.x + _shakeAmount, timePerPartCountLoops).SetEase(Ease.InBounce))
                .Append(transform.DOLocalMoveX(_resetPosition.x, timePerPartCountLoops).SetEase(Ease.InBounce))
                .SetLoops(_loopsCount, LoopType.Restart)
                .OnKill(() =>
                {
                    if(_isNeedToResetPosition)
                        transform.localPosition = _resetPosition;
                    onAnimationEnd?.Invoke();
                    _onAnimationEndAction?.Invoke();
                });

            return _actualController = new AnimationController(sequence, transform);
        }
    }
}