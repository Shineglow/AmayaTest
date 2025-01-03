using System;
using System.Collections.Generic;
using AmayaTest.Scripts.Data;
using AmayaTest.Scripts.General.Extensions;
using AmayaTest.Scripts.General.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AmayaTest.Scripts.Gameplay.Quiz.GridQuiz.GridComponents
{
    public class QuizGridCell : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private SpriteRenderer item;
        [SerializeField] private SpriteRenderer borderTop;
        [SerializeField] private SpriteRenderer borderBottom;
        [SerializeField] private SpriteRenderer borderLeft;
        [SerializeField] private SpriteRenderer borderRight;
        private List<SpriteRenderer> _borders;

        [field: SerializeField] public bool CanHandleMouseEvents { get; set; } = true;
        public event Action<QuizGridCell> OnItemClick;

        private int _additionalOrderInLayer = 0;
        public int AdditionalOrderInLayer
        {
            get => _additionalOrderInLayer;
            set
            {
                int delta = value - _additionalOrderInLayer;
                _additionalOrderInLayer = value;
                background.sortingOrder += delta;
                item.sortingOrder += delta;
                _borders.ForEach(i => i.sortingOrder += delta);
            }
        }

        private void Awake()
        {
            background.ThrowIfNull(nameof(background));
            item.ThrowIfNull(nameof(item));

            _borders = new List<SpriteRenderer>()
            {
                borderTop,
                borderLeft,
                borderBottom,
                borderRight,
            };
        }

        public void SetItemSprite(SpriteResource spriteResource, Color backgroundColor)
        {
            background.color = backgroundColor;
            item.sprite = spriteResource.Sprite;
            ImageUtilities.SetupSpriteRenderer(item, spriteResource);
        }

        public void SetBordersColor(Color color)
        {
            _borders.ForEach(i => i.color = color);
        }

        public Transform GetItemTransform() => item?.transform;
    
        public void OnPointerClick(PointerEventData eventData)
        {
            if(CanHandleMouseEvents)
                OnItemClick?.Invoke(this);
        }

        public void SetBordersWidth(float borderWidthInUnits)
        {
            Vector3 scale = borderBottom.transform.localScale;
            scale.y = borderWidthInUnits;
            scale.x = 1 + borderWidthInUnits;
            borderBottom.transform.localScale = scale;
            borderTop.transform.localScale = scale;
        
            scale = borderLeft.transform.localScale;
            scale.x = borderWidthInUnits;
            scale.y = 1 + borderWidthInUnits;
            borderLeft.transform.localScale = scale;
            borderRight.transform.localScale = scale;
        }
    }
}
