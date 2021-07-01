using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RimLightGroup : MonoBehaviour
{
    [SerializeField] private List<VirtualRimLight> virtualRimLights;
    public IReadOnlyList<VirtualRimLight> VirtualRimLights => virtualRimLights;

    public VirtualRimLight.Parameter GetBlendedRimLightParameter(Transform targetTransform)
    {
        var rimLightParameters = new VirtualRimLight.Parameter[virtualRimLights.Count];
        for (var i = 0; i < virtualRimLights.Count; i++)
        {
            rimLightParameters[i] = virtualRimLights[i].Parameters;
        }

        return GetBlendedRimLightParameter(rimLightParameters, targetTransform);
    }

    public static VirtualRimLight.Parameter GetBlendedRimLightParameter(
        IEnumerable<VirtualRimLight.Parameter> virtualRimLightParameters, Transform targetTransform)
    {
        return GetBlendedRimLightParameterWithWeight(new[] {(virtualRimLightParameters, 1.0f)}, targetTransform);
    }

    public static VirtualRimLight.Parameter GetBlendedRimLightParameterWithWeight(
        IEnumerable<(IEnumerable<VirtualRimLight.Parameter> rimLightParameters, float groupWeight)>
            weightedVirtualRimLightParameters,
        Transform targetTransform)
    {
        var blendedParameter = new VirtualRimLight.Parameter();
        var weightedRimRightParameters = weightedVirtualRimLightParameters.ToArray();

        if (weightedRimRightParameters.Length == 0) return blendedParameter;

        float rimLight = 0.0f,
            isNormalMapToRimLight = 0.0f,
            rimLightFeatherOff = 0.0f,
            lightDirectionMaskOn = 0.0f,
            rimLightDirectionOverwrite = 0.0f,
            rimLightDirectionInverseZAxis = 0.0f,
            addAntipodeanRimLight = 0.0f,
            apRimLightFeatherOff = 0.0f;

        const float baseDistance = 10.0f;
        foreach (var (rimLightParameters, groupWeight) in weightedRimRightParameters)
        {
            var virtualRimLights = rimLightParameters.ToArray();

            foreach (var virtualRimLight in virtualRimLights)
            {
                var weight = groupWeight / virtualRimLights.Length;

                Vector3 localLightPosition =
                    targetTransform.worldToLocalMatrix * virtualRimLight.transform.position;
                var decayedPower = virtualRimLight.rimLightPower *
                                   virtualRimLight.distanceDecayCurve.Evaluate(
                                       localLightPosition.magnitude / baseDistance);
                var decayedApPower = virtualRimLight.apRimLightPower *
                                     virtualRimLight.distanceDecayCurve.Evaluate(
                                         localLightPosition.magnitude / baseDistance);

                // 色と強さのみクリップ内で加算
                rimLight += (virtualRimLight.rimLight ? 1.0f : 0.0f) * weight;
                blendedParameter.color += virtualRimLight.color * decayedPower * groupWeight;
                isNormalMapToRimLight += (virtualRimLight.isNormalMapToRimLight ? 1.0f : 0.0f) * weight;
                blendedParameter.rimLightPower += decayedPower * groupWeight;
                blendedParameter.rimLightInsideMask += virtualRimLight.rimLightInsideMask * weight;
                rimLightFeatherOff += (virtualRimLight.rimLightFeatherOff ? 1.0f : 0.0f) * weight;
                lightDirectionMaskOn += (virtualRimLight.lightDirectionMaskOn ? 1.0f : 0.0f) * weight;
                blendedParameter.tweakLightDirectionMaskLevel +=
                    virtualRimLight.tweakLightDirectionMaskLevel * weight;
                rimLightDirectionOverwrite += 1.0f * weight;
                blendedParameter.rimLightDirectionOffsetXAxis += localLightPosition.x / baseDistance * weight;
                blendedParameter.rimLightDirectionOffsetYAxis += localLightPosition.y / baseDistance * weight;
                rimLightDirectionInverseZAxis += (localLightPosition.z < 0 ? 1.0f : -1.0f) * weight;
                addAntipodeanRimLight += (virtualRimLight.addAntipodeanRimLight ? 1.0f : 0.0f) * weight;
                blendedParameter.apRimLightColor += virtualRimLight.apRimLightColor * decayedApPower * groupWeight;
                blendedParameter.apRimLightPower += decayedApPower * groupWeight;
                apRimLightFeatherOff += (virtualRimLight.apRimLightFeatherOff ? 1.0f : 0.0f) * weight;
            }
        }

        const float threshold = 0.5f;
        blendedParameter.rimLight = threshold < rimLight;
        blendedParameter.isNormalMapToRimLight = threshold < isNormalMapToRimLight;
        blendedParameter.rimLightFeatherOff = threshold < rimLightFeatherOff;
        blendedParameter.lightDirectionMaskOn = threshold < lightDirectionMaskOn;
        blendedParameter.rimLightDirectionOverwrite = threshold < rimLightDirectionOverwrite;
        blendedParameter.rimLightDirectionInverseZAxis = threshold < rimLightDirectionInverseZAxis;
        blendedParameter.addAntipodeanRimLight = threshold < addAntipodeanRimLight;
        blendedParameter.apRimLightFeatherOff = threshold < apRimLightFeatherOff;

        return blendedParameter;
    }
}