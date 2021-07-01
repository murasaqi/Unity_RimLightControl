using UnityEngine;

[ExecuteAlways]
public class RimLightControl : MonoBehaviour
{
    [SerializeField] private RimLightingMaterialGroup rimLightingMaterialGroup;
    [SerializeField] private RimLightGroup rimLightGroup;

    private void Update()
    {
        rimLightingMaterialGroup.SetRimLightParameterOfAll(
            rimLightGroup.GetBlendedRimLightParameter(rimLightingMaterialGroup.TargetTransform));
    }
}