using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[ExecuteAlways]
public class VirtualRimLight : MonoBehaviour
{
    [Serializable]
    public class RimLightParameter
    {
        public AnimationCurve distanceDecayCurve;
        public float range;
        public bool rimLight;
        public Color color;
        public bool isNormalMapToRimLight;
        [Range(0, 5)] public float rimLightPower;
        [Range(0.0001f, 1)] public float rimLightInsideMask;
        public bool rimLightFeatherOff;
        public bool lightDirectionMaskOn;
        [Range(0, 0.5f)] public float tweakLightDirectionMaskLevel;
        [HideInInspector] public bool rimLightDirectionOverwrite = true;
        [Range(-1, 1), HideInInspector] public float rimLightDirectionOffsetXAxis;
        [Range(-1, 1), HideInInspector] public float rimLightDirectionOffsetYAxis;
        [HideInInspector] public bool rimLightDirectionInverseZAxis;
        public bool addAntipodeanRimLight;
        public Color apRimLightColor;
        [Range(0, 5)] public float apRimLightPower;
        public bool apRimLightFeatherOff;
    }

    public static RimLightParameter DefaultRimLightParameter => new RimLightParameter
    {
        distanceDecayCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0)),
        range = 5.0f,
        rimLight = true,
        color = Color.white,
        isNormalMapToRimLight = false,
        rimLightPower = 1.0f,
        rimLightInsideMask = 0.2f,
        rimLightFeatherOff = true,
        lightDirectionMaskOn = true,
        tweakLightDirectionMaskLevel = 0.5f,
        addAntipodeanRimLight = false,
        apRimLightColor = Color.white,
        apRimLightPower = 1.0f,
        apRimLightFeatherOff = false
    };

    [SerializeField] private List<RimLightingMaterialGroup> targetMaterialGroups;
    [SerializeField] private Light parameterReference;
    [SerializeField] private RimLightParameter parameter = DefaultRimLightParameter;

    public RimLightParameter Parameter => parameter;

    private void Start()
    {
        foreach (var targetMaterialGroup in targetMaterialGroups)
        {
            targetMaterialGroup.AddRimLightParameter(this);
        }
    }

    private void Update()
    {
        if (parameterReference != null)
        {
            parameter.color = parameterReference.color;
            parameter.rimLightPower = parameterReference.intensity;
            parameter.range = parameterReference.range;
        }
    }

    private void OnValidate()
    {
        foreach (var targetMaterialGroup in targetMaterialGroups)
        {
            targetMaterialGroup.AddRimLightParameter(this);
        }
    }

    private void OnEnable()
    {
        foreach (var targetMaterialGroup in targetMaterialGroups)
        {
            targetMaterialGroup.AddRimLightParameter(this);
        }
    }

    private void OnDestroy()
    {
        foreach (var targetMaterialGroup in targetMaterialGroups)
        {
            targetMaterialGroup.RemoveRimLightParameter(this);
        }
    }

    private void OnDisable()
    {
        foreach (var targetMaterialGroup in targetMaterialGroups)
        {
            targetMaterialGroup.RemoveRimLightParameter(this);
        }
    }
}