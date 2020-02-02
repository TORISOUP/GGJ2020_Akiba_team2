using System.Collections;
using UnityEngine;

namespace GGJ2020.Common
{
    public class ElementShake : MonoBehaviour
    {
        private RectTransform rect;
        private Vector3 originalPosition;
        private Quaternion originalRotation;

        private Coroutine rotateCoroutine;
        private Coroutine positionCoroutine;

        private void Start()
        {
            rect = GetComponent<RectTransform>();
            originalPosition = rect.anchoredPosition;
            originalRotation = rect.rotation;
        }

        public void ShakePosition(float power, float frame)
        {
            if (rect == null) return;
            if (positionCoroutine != null) StopCoroutine(positionCoroutine);
            positionCoroutine = StartCoroutine(ShakePositionCoroutine(power, frame));
        }

        private IEnumerator ShakePositionCoroutine(float power, float frame)
        {
            for (var i = 0; i < frame; i++)
            {
                var ox = UnityEngine.Random.Range(-1.0f, 1.0f);
                var oy = UnityEngine.Random.Range(-1.0f, 1.0f);
                var offset = new Vector3(ox, oy, 0).normalized * UnityEngine.Random.Range(0, power);
                rect.anchoredPosition = originalPosition + offset;
                yield return null;
            }

            rect.anchoredPosition = originalPosition;
        }

        public void ShakeRotation(float angleRange, float frame)
        {
            if (rect == null) return;
            if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
            rotateCoroutine = StartCoroutine(ShakeRotationCorountine(angleRange, frame, true));
        }

        private IEnumerator ShakeRotationCorountine(float angleRange, float frame, bool isResetLast)
        {
            for (int i = 0; i < frame; i++)
            {
                var angle = UnityEngine.Random.Range(-angleRange, angleRange);
                rect.rotation = Quaternion.AngleAxis(angle, Vector3.forward) * originalRotation;
                yield return null;
            }

            if (isResetLast)
            {
                rect.rotation = originalRotation;
            }
        }

        /// <summary>
        /// 揺れた後そのままにする
        /// </summary>
        public void ShakeRotationPermanent(float angleRange, float frame)
        {
            if (rect == null) return;
            if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
            rotateCoroutine = StartCoroutine(ShakeRotationCorountine(angleRange, frame, false));
        }
    }
}