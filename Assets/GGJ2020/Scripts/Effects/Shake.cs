using UnityEngine;

public class Shake : MonoBehaviour
{
    private float _intensity;
    private float _duration;
    private float _currDuration;
    private Vector3 pos;

    /// <summary>
    /// Shake the current gameObject
    /// </summary>
    public void ShakeMe(float intensity, float duration)
    {
        _intensity = intensity;
        _duration = duration;
        _currDuration = duration;
    }

    private void Start()
    {
        _intensity = 0f;
        _duration = 0f;
        _currDuration = 0f;
        pos = transform.position;
    }

    private void Update()
    {
        if (_currDuration > 0f)
        {
            float first = _intensity / 3f;
            float middle = _intensity / 2f;
            float x = pos.x + Random.value * middle - first;
            float z = pos.z + Random.value * middle - first;
            _currDuration -= Time.deltaTime;
            if (_currDuration > 0f)
                transform.position = new Vector3(x, transform.position.y, z);
            else
                transform.position = pos;
        }
    }
}