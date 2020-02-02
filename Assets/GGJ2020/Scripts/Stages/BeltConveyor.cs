using GGJ2020.Managers;
using UnityEngine;
using UniRx.Async;
using Zenject;

namespace GGJ2020.Stages
{
    public class BeltConveyor : MonoBehaviour
    {
        public Vector3 PartsAppPoint
        {
            get { return partsAppPoint.position; }
        }

        public Vector3 CenterPoint
        {
            get { return centerPoint.position; }
        }

        public Vector3 ShippingPoint
        {
            get { return shippingPoint.position; }
        }

        public AssemblyArea NowFieldAssemblyArea
        {
            get { return nowFieldAssemblyArea; }
        }

        [Header("パーツセット出現ポイント")] [SerializeField]
        private Transform partsAppPoint;

        [Header("中央ポイント")] [SerializeField] private Transform centerPoint;

        [Header("パーツセット納品ポイント")] [SerializeField]
        private Transform shippingPoint;

        [SerializeField] private SpriteRenderer beltConveyorSprite;
        [SerializeField] private float scrollSpeed;
        public bool IsMove
        {
            get { return isMove; }
        }

        [Inject] private StageAudioManager _audioManager;

        private Vector3 movePoint = Vector3.zero;
        private float moveSpeed = 0;
        private AssemblyArea nowFieldAssemblyArea = null;
        private bool isMove = false;
        private float scrollCount = 0;

        public void SetFieldAssemblyArea(AssemblyArea _assemblyArea)
        {
            nowFieldAssemblyArea = _assemblyArea;
        }

        public void RemoveFieldAssemblyArea()
        {
            nowFieldAssemblyArea = null;
        }

        public async UniTask MoveToCenterAsync(float moveTime)
        {
            _audioManager.PlayBeltConvey();
            
            movePoint = centerPoint.position;
            moveSpeed = Vector3.Distance(centerPoint.position, partsAppPoint.position) / moveTime;
            isMove = true;
            nowFieldAssemblyArea.SetMoving(true);
            scrollCount = beltConveyorSprite.material.GetTextureOffset("_MainTex").y;
            while (Mathf.Abs(nowFieldAssemblyArea.transform.position.y - movePoint.y) >= 0.1f)
            {
                nowFieldAssemblyArea.transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
                scrollCount -= Time.deltaTime * scrollSpeed;
                beltConveyorSprite.material.SetTextureOffset("_MainTex", new Vector2(0, scrollCount));
                await UniTask.Yield();
            }

            nowFieldAssemblyArea.transform.position = movePoint;
            isMove = false;
            nowFieldAssemblyArea.SetMoving(false);
        }

        public async UniTask MoveToShippingAsync(float moveTime)
        {
            _audioManager.PlayBeltConvey();

            
            movePoint = shippingPoint.position;
            moveSpeed = Vector3.Distance(centerPoint.position, partsAppPoint.position) / moveTime;
            isMove = true;
            nowFieldAssemblyArea.SetMoving(true);
            scrollCount = beltConveyorSprite.material.GetTextureOffset("_MainTex").y;
            while (nowFieldAssemblyArea.transform.position.y <= movePoint.y) 
            {
                nowFieldAssemblyArea.transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
                scrollCount -= Time.deltaTime * scrollSpeed;
                beltConveyorSprite.material.SetTextureOffset("_MainTex", new Vector2(0, scrollCount));
                await UniTask.Yield();
            }

            nowFieldAssemblyArea.transform.position = movePoint;
            isMove = false;
            nowFieldAssemblyArea.SetMoving(false);
        }
    }
}