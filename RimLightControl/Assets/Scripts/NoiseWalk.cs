using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NoiseWalk : MonoBehaviour
{
    [SerializeField] private float r = 2.0f;
    [SerializeField] private float freq = 5.0f;
    [SerializeField] private float intensity = 1.0f;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = initialPosition + new Vector3(Mathf.Cos(Time.time * freq) * r, 0.0f, Mathf.Sin(Time.time * freq) * r);
        pos.x += noise.cnoise(new Vector2(initialPosition.x, initialPosition.y) * Time.time * 0.3f) * intensity;
        pos.y += noise.cnoise(new Vector2(initialPosition.y, initialPosition.z) * Time.time * 0.3f) * intensity;
        pos.z += noise.cnoise(new Vector2(initialPosition.z, initialPosition.x) * Time.time * 0.3f) * intensity;
        transform.position = pos;
    }
}