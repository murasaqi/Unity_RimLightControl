using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RimLightingMaterialGroup : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private List<Material> materials;
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

    public Transform TargetTransform => targetTransform;

    public VirtualRimLight.Parameter GetRimLightParametersOfFirst()
    {
        var parameter = new VirtualRimLight.Parameter();
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
        parameter.addAntipodeanRimLight = threshold < firstMaterial.GetFloat(AddAntipodeanRimLight);
        parameter.apRimLightColor = firstMaterial.GetColor(ApRimLightColor);
        parameter.apRimLightPower = firstMaterial.GetFloat(ApRimLightPower);
        parameter.apRimLightFeatherOff = threshold < firstMaterial.GetFloat(ApRimLightFeatherOff);

        return parameter;
    }

    public void SetRimLightParameterOfAll(in VirtualRimLight.Parameter parameter)
    {
        foreach (var material in materials)
        {
            material.SetFloat(RimLight, parameter.rimLight ? 1.0f : 0.0f);
            material.SetColor(RimLightColor, parameter.color);
            material.SetFloat(IsNormalMapToRimLight, parameter.isNormalMapToRimLight ? 1.0f : 0.0f);
            material.SetFloat(RimLightPower, parameter.rimLightPower);
            material.SetFloat(RimLightInsideMask, parameter.rimLightInsideMask);
            material.SetFloat(RimLightFeatherOff, parameter.rimLightFeatherOff ? 1.0f : 0.0f);
            material.SetFloat(LightDirectionMaskOn, parameter.lightDirectionMaskOn ? 1.0f : 0.0f);
            material.SetFloat(TweakLightDirectionMaskLevel, parameter.tweakLightDirectionMaskLevel);
            material.SetFloat(IsRimLightBld, parameter.rimLightDirectionOverwrite ? 1.0f : 0.0f);
            material.SetFloat(OffsetXAxisRimLightBld, parameter.rimLightDirectionOffsetXAxis);
            material.SetFloat(OffsetYAxisRimLightBld, parameter.rimLightDirectionOffsetYAxis);
            material.SetFloat(InverseZAxisRimLightBld, parameter.rimLightDirectionInverseZAxis ? 1.0f : 0.0f);
            material.SetFloat(AddAntipodeanRimLight, parameter.addAntipodeanRimLight ? 1.0f : 0.0f);
            material.SetColor(ApRimLightColor, parameter.apRimLightColor);
            material.SetFloat(ApRimLightPower, parameter.apRimLightPower);
            material.SetFloat(ApRimLightFeatherOff, parameter.apRimLightFeatherOff ? 1.0f : 0.0f);
        }
    }
}