using System;
using System.Collections.Generic;
using System.Linq;
using GGJ2020.Parts;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GGJ2020.Stages
{
    public class AssemblyArea : MonoBehaviour
    {
        [Header("生成するパーツ数の最小値")] [SerializeField]
        private int MinRandomAppPartsNum;

        [Header("生成するパーツ数の最大値")] [SerializeField]
        private int MaxRandomAppPartsNum;

        [Header("エリアスケール")] [SerializeField] private float areaScale;
        [SerializeField] private GameObject[] partPrefabs;

        [SerializeField] private Transform CenterTransform;
        [SerializeField] private float springK = 1.0f;
        [SerializeField] private float freezeRadius = 0.2f;
        public HashSet<Parts.PartObject> CurrentPartObjects { get; } = new HashSet<PartObject>();

        private bool _isMoving;

        private void FixedUpdate()
        {
            foreach (var partObject in CurrentPartObjects)
            {
                if (partObject == null) continue;

                var deltaVec = (CenterTransform.position - partObject.transform.position);
                var dir = deltaVec.normalized;
                var length = deltaVec.magnitude;

                if (length < freezeRadius)
                {
                    var r = partObject.GetComponent<Rigidbody2D>();
                    r.velocity = Vector2.zero;
                    r.angularVelocity = 0;
                }
                else
                {
                    partObject.GetComponent<Rigidbody2D>()
                        .AddForce(dir * (length * length * springK), ForceMode2D.Impulse);
                }
            }
        }

        public void Init()
        {
            var normals = partPrefabs
                .Where(x => x.GetComponent<PartObject>().Part.Quality == Quality.Normal)
                .ToArray();

            var high = partPrefabs
                .Where(x => x.GetComponent<PartObject>().Part.Quality == Quality.High)
                .ToArray();

            var low = partPrefabs
                .Where(x => x.GetComponent<PartObject>().Part.Quality == Quality.Low)
                .ToArray();


            var partsNum = UnityEngine.Random.Range(MinRandomAppPartsNum, MaxRandomAppPartsNum);
            for (int i = 0; i < partsNum; i++)
            {
                var qualityProbability = UnityEngine.Random.Range(0, 100);
                var targetList = default(GameObject[]);
                if (qualityProbability < 10)
                {
                    // 10%
                    targetList = high;
                }
                else if (qualityProbability < 10 + 70)
                {
                    // 70%
                    targetList = normals;
                }
                else
                {
                    targetList = low;
                }


                var part = targetList[Random.Range(0, targetList.Length)];
                var partObj = Instantiate<GameObject>(part, transform.position, Quaternion.identity);
                partObj.transform.parent = transform;
                var randomPos = new Vector2(Random.Range(-areaScale / 2, areaScale / 2),
                    Random.Range(-areaScale / 2, areaScale / 2));
                partObj.transform.localPosition = randomPos;
                AddPart(partObj.GetComponent<PartObject>());
            }
        }

        /// <summary>
        /// 移動状態になると中のコライダーの判定が消える
        /// </summary>
        public void SetMoving(bool move)
        {
            _isMoving = move;
            CurrentPartObjects.RemoveWhere(x => x == null);

            foreach (var c in CurrentPartObjects
                .SelectMany(x => x.GetComponentsInChildren<Collider2D>()))
            {
                c.isTrigger = move;
            }

            foreach (var c in CurrentPartObjects
                .SelectMany(x => x.GetComponentsInChildren<SpriteRenderer>()))
            {
                c.sortingLayerName = move ? "PartInAssemblyArea" : "Part";
            }
        }

        public void AddPart(PartObject partObject)
        {
            if (_isMoving) return;

            CurrentPartObjects.RemoveWhere(x => x == null);

            CurrentPartObjects.Add(partObject);
            partObject.transform.parent = this.transform;
        }

        public void RemovePart(PartObject partObject)
        {
            CurrentPartObjects.RemoveWhere(x => x == null);

            CurrentPartObjects.Remove(partObject);

            if (!partObject.IsHold.Value)
            {
                partObject.transform.parent = null;
            }
        }

        public void DestroyAllParts()
        {
            foreach (var partObject in CurrentPartObjects.ToArray())
            {
                if (partObject != null)
                {
                    Destroy(partObject.gameObject);
                }
            }

            CurrentPartObjects.Clear();
        }
    }
}