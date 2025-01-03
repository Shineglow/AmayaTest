using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace AmayaTest.Scripts.General.ProceduralAnimations.DoTween
{
    public class GridAnimation<T> where T : MonoBehaviour
    {
        private float CalculateMaxDistance(List<List<T>> cells, Vector3 startPoint)
        {
            float maxDist = 0f;
            foreach (var row in cells)
            {
                foreach (var cell in row)
                {
                    float distance = Vector3.Distance(startPoint, cell.transform.position);
                    if (distance > maxDist)
                        maxDist = distance;
                }
            }
            return maxDist;
        }

        public async Task AnimateCellsAsync(
            List<List<T>> cells, 
            Vector3 startPoint,
            Vector3 maxScale,
            Vector3 normalScale,
            Action<T, int> onItemAnimated,
            float animationDistanceDelta = 0.2f,
            float scaleDuration = 0.5f)
        {
            List<Task> animationTasks = new List<Task>();
            float currentDistance = 0f;
            float lastDistance = -1;
            float maxDistance = CalculateMaxDistance(cells, startPoint) + animationDistanceDelta + 0.1f;

            while (currentDistance <= maxDistance)
            {
                foreach (var row in cells)
                {
                    foreach (var cell in row)
                    {
                        float distanceToCell = Vector3.Distance(startPoint, cell.transform.position);
                        if (distanceToCell <= currentDistance && distanceToCell > lastDistance)
                        {
                            animationTasks.Add(AnimateCellAsync(cell, maxScale, normalScale, scaleDuration, onItemAnimated));
                        }
                    }
                }

                lastDistance = currentDistance;
                currentDistance += animationDistanceDelta;
                await Task.Yield();
            }

            await Task.WhenAll(animationTasks);
        }

        async Task AnimateCellAsync(T item, Vector3 maxScale, Vector3 normalScale, float scaleDuration, Action<T, int> onItemAnimated)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            Sequence seq = DOTween.Sequence();
            seq.Append(item.transform.DOScale(maxScale, scaleDuration / 2).SetEase(Ease.OutSine).OnUpdate(() => onItemAnimated(item, 30)));
            seq.Append(item.transform.DOScale(normalScale, scaleDuration / 2).SetEase(Ease.InSine).OnUpdate(() => onItemAnimated(item, -30)));
            seq.OnComplete(() => { tcs.SetResult(true); });

            await tcs.Task;
        }
    }
}