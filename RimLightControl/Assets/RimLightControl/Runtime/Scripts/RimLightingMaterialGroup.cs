using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[ExecuteAlways]
public class RimLightingMaterialGroup : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private List<Material> materials;

    public Transform TargetTransform => targetTransform;

    private readonly List<VirtualRimLight> virtualRimLights = new();

    private static readonly int RimLight = Shader.PropertyToID("_RimLight");
    private static readonly int RimLightColor = Shader.PropertyToID("_RimLightColor");
    private static readonly int IsNormalMapToRimLight = Shader.PropertyToID("_Is_NormalMapToRimLight");
    private static readonly int RimLightPower = Shader.PropertyToID("_RimLight_Power");
    private static readonly int RimLightInsideMask = Shader.PropertyToID("_RimLight_InsideMask");
    private static readonly int RimLightFeatherOff = Shader.PropertyToID("_RimLight_FeatherOff");
    private static readonly int LightDirectionMaskOn = Shader.PropertyToID("_LightDirection_MaskOn");
    private static readonly int TweakLightDirectionMaskLevel = Shader.PropertyToID("_Tweak_LightDirection_MaskLevel");
    private static readonly int IsRimLightBld = Shader.PropertyToID("_Is_RimLightBLD");
    private static readonly int OffsetXAxisRimLightBld = Shader.PropertyToID("_Offset_X_Axis_RimLightBLD");
    private static readonly int OffsetYAxisRimLightBld = Shader.PropertyToID("_Offset_Y_Axis_RimLightBLD");
    private static readonly int InverseZAxisRimLightBld = Shader.PropertyToID("_Inverse_Z_Axis_RimLightBLD");
    private static readonly int AddAntipodeanRimLight = Shader.PropertyToID("_Add_Antipodean_RimLight");
    private static readonly int ApRimLightColor = Shader.PropertyToID("_Ap_RimLightColor");
    private static readonly int ApRimLightPower = Shader.PropertyToID("_Ap_RimLight_Power");
    private static readonly int ApRimLightFeatherOff = Shader.PropertyToID("_Ap_RimLight_FeatherOff");
    private static readonly int MultiplyColor = Shader.PropertyToID("_MultiplyColor");
    private static readonly int MultiplyLevel = Shader.PropertyToID("_MultiplyLevel");

    private VirtualRimLight.RimLightParameter GetRimLightParametersOfFirst()
    {
        var parameter = new VirtualRimLight.RimLightParameter();
        var firstMaterial = materials.FirstOrDefault();

        if (firstMaterial == null) return parameter;

        const float threshold = 0.5f;
        parameter.rimLight = threshold < firstMaterial.GetFloat(RimLight);
        parameter.color = firstMaterial.GetColor(RimLightColor);
        parameter.isNormalMapToRimLight = threshold < firstMaterial.GetFloat(IsNormalMapToRimLight);
        parameter.rimLightPower = firstMaterial.GetFloat(RimLightPower);
        parameter.rimLightInsideMask = firstMaterial.GetFloat(RimLightInsideMask);
        parameter.rimLightFeatherOff = threshold < firstMaterial.GetFloat(RimLightFeatherOff);
        parameter.lightDirectionMaskOn = threshold < firstMaterial.GetFloat(LightDirectionMaskOn);
        parameter.tweakLightDirectionMaskLevel = firstMaterial.GetFloat(TweakLightDirectionMaskLevel);
        parameter.rimLightDirectionOverwrite = threshold < firstMaterial.GetFloat(IsRimLightBld);
        parameter.rimLightDirectionOffsetXAxis = firstMaterial.GetFloat(OffsetXAxisRimLightBld);
        parameter.rimLightDirectionOffsetYAxis = firstMaterial.GetFloat(OffsetYAxisRimLightBld);
        parameter.rimLightDirectionInverseZAxis = threshold < firstMaterial.GetFloat(InverseZAxisRimLightBld);
        parameter.multiplyColor = firstMaterial.GetColor(MultiplyColor);
        parameter.multiplyLevel = firstMaterial.GetFloat(MultiplyLevel);
        parameter.addAntipodeanRimLight = threshold < firstMaterial.GetFloat(AddAntipodeanRimLight);
        parameter.apRimLightColor = firstMaterial.GetColor(ApRimLightColor);
        parameter.apRimLightPower = firstMaterial.GetFloat(ApRimLightPower);
        parameter.apRimLightFeatherOff = threshold < firstMaterial.GetFloat(ApRimLightFeatherOff);

        return parameter;
    }

    public void AddRimLightParameter(in VirtualRimLight virtualRimLight)
    {
        if (!virtualRimLights.Contains(virtualRimLight))
        {
            virtualRimLights.Add(virtualRimLight);
        }
    }

    public void RemoveRimLightParameter(in VirtualRimLight virtualRimLight)
    {
        virtualRimLights.Remove(virtualRimLight);
    }

    private VirtualRimLight.RimLightParameter BlendParameters()
    {
        var blendedParameter = new VirtualRimLight.RimLightParameter();

        var rimLightFeatherOff = 0.0f;
        var rimLightDirectionInverseZAxis = 0.0f;
        var apRimLightFeatherOff = 0.0f;

        var rimLinearWeight = 1.0f / virtualRimLights.Count(virtualRimLight =>
        {
            Vector3 localLightPosition =
                targetTransform.worldToLocalMatrix * virtualRimLight.transform.position;
            return virtualRimLight.Parameter.rimLight &&
                   localLightPosition.magnitude / virtualRimLight.Parameter.range <= 1.0f;
        });
        var apRimLinearWeight =
            1.0f / virtualRimLights.Count(virtualRimLight =>
            {
                Vector3 localLightPosition =
                    targetTransform.worldToLocalMatrix * virtualRimLight.transform.position;
                return virtualRimLight.Parameter.addAntipodeanRimLight &&
                       localLightPosition.magnitude / virtualRimLight.Parameter.range <= 1.0f;
            });

        foreach (var virtualRimLight in virtualRimLights)
        {
            Vector3 localLightPosition =
                targetTransform.worldToLocalMatrix * virtualRimLight.transform.position;

            var parameter = virtualRimLight.Parameter;

            var decayedPower = Mathf.Clamp(parameter.rimLightPower * parameter.distanceDecayCurve.Evaluate(
                localLightPosition.magnitude / parameter.range), 0, 5) * rimLinearWeight;

            var decayedApPower = parameter.apRimLightPower * parameter.distanceDecayCurve.Evaluate(
                localLightPosition.magnitude / parameter.range) * rimLinearWeight;

            blendedParameter.rimLight |= parameter.rimLight;
            if (parameter.rimLight && localLightPosition.magnitude / virtualRimLight.Parameter.range <= 1.0f)
            {
                blendedParameter.color += parameter.color * decayedPower;
                blendedParameter.isNormalMapToRimLight &= parameter.isNormalMapToRimLight;
                blendedParameter.rimLightPower += decayedPower;
                blendedParameter.rimLightInsideMask += parameter.rimLightInsideMask * rimLinearWeight * decayedPower;
                rimLightFeatherOff += (parameter.rimLightFeatherOff ? 1.0f : 0.0f) * rimLinearWeight;
                blendedParameter.lightDirectionMaskOn |= parameter.lightDirectionMaskOn;
                blendedParameter.tweakLightDirectionMaskLevel += parameter.tweakLightDirectionMaskLevel * decayedPower;
                blendedParameter.rimLightDirectionOverwrite |= parameter.rimLightDirectionOverwrite;
                blendedParameter.rimLightDirectionOffsetXAxis += localLightPosition.x / parameter.range * decayedPower;
                blendedParameter.rimLightDirectionOffsetYAxis += localLightPosition.y / parameter.range * decayedPower;
                rimLightDirectionInverseZAxis +=
                    (localLightPosition.z < 0 ? 1.0f : -1.0f) * decayedPower;
                blendedParameter.multiplyColor += parameter.multiplyColor * rimLinearWeight;
                blendedParameter.multiplyLevel += (localLightPosition.z < 0 ? 1.0f : 0.0f) * decayedPower;
            }

            blendedParameter.addAntipodeanRimLight |= parameter.addAntipodeanRimLight;
            if (parameter.addAntipodeanRimLight &&
                localLightPosition.magnitude / virtualRimLight.Parameter.range <= 1.0f)
            {
                blendedParameter.apRimLightColor += parameter.apRimLightColor * decayedApPower;
                blendedParameter.apRimLightPower += decayedApPower;
                apRimLightFeatherOff += (parameter.apRimLightFeatherOff ? 1.0f : 0.0f) * apRimLinearWeight;
            }
        }

        blendedParameter.rimLightInsideMask = Mathf.Clamp(blendedParameter.rimLightInsideMask, 0.001f, 1.0f);
        blendedParameter.tweakLightDirectionMaskLevel =
            Mathf.Clamp(blendedParameter.tweakLightDirectionMaskLevel, 0.0f, 0.5f);
        blendedParameter.rimLightDirectionOffsetXAxis =
            Mathf.Clamp(blendedParameter.rimLightDirectionOffsetXAxis, -1f, 1f);
        blendedParameter.rimLightDirectionOffsetYAxis =
            Mathf.Clamp(blendedParameter.rimLightDirectionOffsetYAxis, -1f, 1f);

        const float threshold = 0.5f;
        blendedParameter.rimLightFeatherOff = threshold < rimLightFeatherOff;
        blendedParameter.rimLightDirectionInverseZAxis = 0.0f < rimLightDirectionInverseZAxis;
        blendedParameter.apRimLightFeatherOff = threshold < apRimLightFeatherOff;

        return blendedParameter;
    }

    private void SetRimLightParameterOfAll(in VirtualRimLight.RimLightParameter blendedParameter)
    {
        foreach (var material in materials)
        {
            material.SetFloat(RimLight, blendedParameter.rimLight ? 1.0f : 0.0f);
            material.SetColor(RimLightColor, blendedParameter.color);
            material.SetFloat(IsNormalMapToRimLight, blendedParameter.isNormalMapToRimLight ? 1.0f : 0.0f);
            material.SetFloat(RimLightPower, blendedParameter.rimLightPower);
            material.SetFloat(RimLightInsideMask, blendedParameter.rimLightInsideMask);
            material.SetFloat(RimLightFeatherOff, blendedParameter.rimLightFeatherOff ? 1.0f : 0.0f);
            material.SetFloat(LightDirectionMaskOn, blendedParameter.lightDirectionMaskOn ? 1.0f : 0.0f);
            material.SetFloat(TweakLightDirectionMaskLevel, blendedParameter.tweakLightDirectionMaskLevel);
            material.SetFloat(IsRimLightBld, blendedParameter.rimLightDirectionOverwrite ? 1.0f : 0.0f);
            material.SetFloat(OffsetXAxisRimLightBld, blendedParameter.rimLightDirectionOffsetXAxis);
            material.SetFloat(OffsetYAxisRimLightBld, blendedParameter.rimLightDirectionOffsetYAxis);
            material.SetFloat(InverseZAxisRimLightBld, blendedParameter.rimLightDirectionInverseZAxis ? 1.0f : 0.0f);
            material.SetColor(MultiplyColor, blendedParameter.multiplyColor);
            material.SetFloat(MultiplyLevel, blendedParameter.multiplyLevel);
            material.SetFloat(AddAntipodeanRimLight, blendedParameter.addAntipodeanRimLight ? 1.0f : 0.0f);
            material.SetColor(ApRimLightColor, blendedParameter.apRimLightColor);
            material.SetFloat(ApRimLightPower, blendedParameter.apRimLightPower);
            material.SetFloat(ApRimLightFeatherOff, blendedParameter.apRimLightFeatherOff ? 1.0f : 0.0f);
        }
    }

    private void LateUpdate()
    {
        SetRimLightParameterOfAll(BlendParameters());
    }
}