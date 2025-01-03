using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AmayaTest.Scripts.Data;
using AmayaTest.Scripts.General;
using AmayaTest.Scripts.General.Extensions;
using AmayaTest.Scripts.General.ProceduralAnimations;
using AmayaTest.Scripts.General.ProceduralAnimations.DoTween;
using AmayaTest.Scripts.General.Utilities;
using UnityEngine;

namespace AmayaTest.Scripts.Gameplay.Quiz.GridQuiz.GridComponents
{
    public class QuizGridView : MonoBehaviour, IQuizGridView
    {
        private const int INITIAL_POOL_CAPACITY = 9;
        
        [SerializeField] private OrthogonalCameraInfo orthogonalCameraInfo;
        [SerializeField] private QuizGridCell cellPrefab;
        [SerializeField] private bool isNeedToScaleWithViewPortSize = true;

        private List<List<QuizGridCell>> _items;
        private readonly Dictionary<QuizGridCell, SpriteResource> _cellToResource = new Dictionary<QuizGridCell, SpriteResource>();
        private readonly Dictionary<SpriteResource, QuizGridCell> _resourceToCell = new Dictionary<SpriteResource, QuizGridCell>();

        private ObjectsPool<QuizGridCell> _itemsPool;

        private Action<SpriteResource> _onClickAction;

        public async Task LoadData(List<(SpriteResource resource, Color backgroundColor)> data, 
                             Action<SpriteResource> onClickAction, 
                             int width, 
                             int height, 
                             float cellSizeInUnits, 
                             float borderThicknessInUnits, 
                             Vector2 gridOffset, 
                             Color borderColor,
                             bool skipAnimation)
        {
            data.ThrowIfNull(nameof(data));
            onClickAction.ThrowIfNull(nameof(onClickAction));
            
            int itemsCount = width * height;
            if (itemsCount > data.Count)
                throw new ArgumentException("Количество элементов для данной сетки превышает размер пула объектов.");

            _itemsPool ??= new ObjectsPool<QuizGridCell>(INITIAL_POOL_CAPACITY, () =>
                {
                    QuizGridCell item = Instantiate(cellPrefab);
                    item.gameObject.SetActive(false);
                    return item;
                }, cell =>
                {
                    cell.OnItemClick -= OnItemClick;
                    cell.gameObject.SetActive(false);
                });
            _itemsPool.FreeAllForce();

            _items = SpawnGrid(data, width, height, cellSizeInUnits, borderColor, borderThicknessInUnits);

            ScaleToViewPort(borderThicknessInUnits, width, height);
            transform.position = Vector2.zero + gridOffset;
            _onClickAction = onClickAction;

            await AnimateFieldCreation(cellSizeInUnits, skipAnimation);
        }

        public async Task AnimateItem(SpriteResource resource, IProceduralAnimation animation)
        {
            resource.Sprite.ThrowIfNull(nameof(resource.Sprite));
            animation.ThrowIfNull(nameof(animation));

            if (!_resourceToCell.TryGetValue(resource, out QuizGridCell gridCell))
                throw new DataException($"{nameof(_resourceToCell)} dictionary has no item by actual resource!");

            try
            {
                bool animationEnded = false;
                _ = animation.Animate(gridCell.GetItemTransform(), () => animationEnded = true);
                await TaskUtilities.WaitWhile(() => !animationEnded);
            }
            catch (NullReferenceException e)
            {
                Debug.LogException(e);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void SetItemsCanHandleMouseEvents(bool value)
        {
            _items.ForEach(y => y.ForEach( x => x.CanHandleMouseEvents = value ));
        }

        public void SetPositionToItem(SpriteResource spriteResource, Transform objTransform)
        {
            objTransform.position = _resourceToCell[spriteResource].transform.position;
        }
        
        private List<List<QuizGridCell>> SpawnGrid(
            List<(SpriteResource resource, Color backgroundColor)> data, 
            int width, 
            int height, 
            float cellSizeInUnits,
            Color borderColor,
            float borderWidthInUnits)
        {
            List<List<QuizGridCell>> items = CollectionUtilities.InitializeCollectionWithSize<QuizGridCell>(width, height);
            Vector2 initialPosition = orthogonalCameraInfo.WorldPosition - new Vector2(width, height) / 2 * cellSizeInUnits;
            Vector2 cellInitialPosition = initialPosition + Vector2.one / 2f * cellSizeInUnits;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    QuizGridCell item = _itemsPool.GetObject();
                    (SpriteResource resource, Color backgroundColor) dataItem = data[i * width + j];
                    items[i][j] = item;

                    item.gameObject.SetActive(true);
                    item.transform.SetParent(transform);
                    item.transform.localPosition = cellInitialPosition + new Vector2(j, i) * cellSizeInUnits;
                    item.transform.localScale = Vector3.zero;
                    item.SetItemSprite(dataItem.resource, dataItem.backgroundColor);
                    item.SetBordersColor(borderColor);
                    item.SetBordersWidth(borderWidthInUnits);
                    item.OnItemClick += OnItemClick;

                    _cellToResource[item] = dataItem.resource;
                    _resourceToCell[dataItem.resource] = item;
                }
            }

            return items;
        }

        private void OnItemClick(QuizGridCell obj)
        {
            if (!_cellToResource.TryGetValue(obj, out SpriteResource val))
                throw new ArgumentException("Попытка обратиться к несуществующему элементу");
            _onClickAction(val);
        }

        private void ScaleToViewPort(float borderThicknessInUnits, int width, int height)
        {
            if (!isNeedToScaleWithViewPortSize) return;
            
            float totalWidth = width + borderThicknessInUnits;
            float totalHeight = height + borderThicknessInUnits;

            Vector2 cameraRect = orthogonalCameraInfo.GetOrthographicRectInUnits();

            float aspectGrid = totalWidth / totalHeight;
            float aspectRect = cameraRect.x / cameraRect.y;
            float scaleToViewPort = 0;

            if (aspectGrid > aspectRect)
            {
                scaleToViewPort = cameraRect.x / totalWidth * 0.8f;
            }
            else
            {
                scaleToViewPort = cameraRect.y / totalHeight * 0.65f;
            }

            transform.localScale *= scaleToViewPort;
        }
        
        private async Task AnimateFieldCreation(float cellSizeInUnits, bool skipAnimation)
        {
            if (!skipAnimation)
            {
                await new GridAnimation<QuizGridCell>()
                    .AnimateCellsAsync(_items, Vector3.left * 4, Vector3.one * cellSizeInUnits * 1.2f,
                        Vector3.one * cellSizeInUnits,
                        (item, additionalOrderInLayer) => { item.AdditionalOrderInLayer += additionalOrderInLayer; }, 0.1f, 1f);
            }
            else
            {
                _items.ForEach(y => y.ForEach(x => x.transform.localScale = Vector3.one * cellSizeInUnits));
            }
        }
    }
}