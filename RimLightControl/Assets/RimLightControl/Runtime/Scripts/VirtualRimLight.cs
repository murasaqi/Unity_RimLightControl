using System;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class VirtualRimLight : MonoBehaviour
{
    [Serializable]
    public struct Parameter
    {
        public Transform transform;
        public AnimationCurve distanceDecayCurve;
        [HideInInspector] public bool rimLight;
        public Color color;
        public bool isNormalMapToRimLight;
        [Range(0, 5)] public float rimLightPower;
        [Range(0.0001f, 1)] public float rimLightInsideMask;
        public bool rimLightFeatherOff;
        public bool lightDirectionMaskOn;
        [Range(0, 0.5f)] public float tweakLightDirectionMaskLevel;
        [HideInInspector] public bool rimLightDirectionOverwrite;
        [Range(-1, 1), HideInInspector] public float rimLightDirectionOffsetXAxis;
        [Range(-1, 1), HideInInspector] public float rimLightDirectionOffsetYAxis;
        [HideInInspector] public bool rimLightDirectionInverseZAxis;
        public bool addAntipodeanRimLight;
        public Color apRimLightColor;
        [Range(0, 5)] public float apRimLightPower;
        public bool apRimLightFeatherOff;
    }

    [SerializeField] private Light parameterReference;
    [SerializeField] private Parameter parameters;

    public Parameter Parameters => parameters;

    private void Update()
    {
        if (parameterReference != null)
        {
            parameters.color = parameterReference.color;
            parameters.rimLightPower = parameterReference.intensity;
        }
    }
}