using System;
using UnityEngine;

namespace AmayaTest.Scripts.General.ProceduralAnimations
{
    public interface IProceduralAnimation
    {
        IAnimationController Animate(Transform transform, Action onAnimationEnd);
    }
}