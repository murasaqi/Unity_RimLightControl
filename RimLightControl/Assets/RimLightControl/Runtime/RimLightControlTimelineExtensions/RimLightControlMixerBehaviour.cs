using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class RimLightControlMixerBehaviour : PlayableBehaviour
{
    private bool firstFrameHappened;
    private VirtualRimLight.Parameter initialParameter;
    private RimLightingMaterialGroup trackBinding;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        trackBinding = playerData as RimLightingMaterialGroup;

        if (!trackBinding)
            return;

        if (!firstFrameHappened)
        {
            initialParameter = trackBinding.GetRimLightParametersOfFirst();
            firstFrameHappened = true;
        }

        var inputCount = playable.GetInputCount();
        var totalWeight = 0.0f;

        var weightedVirtualRimLightParameters =
            new List<(IEnumerable<VirtualRimLight.Parameter> rimLightParameters, float groupWeight)>();
        for (var i = 0; i < inputCount; i++)
        {
            var inputWeight = playable.GetInputWeight(i);
            var inputPlayable =
                (ScriptPlayable<RimLightControlBehaviour>) playable.GetInput(i);
            var input = inputPlayable.GetBehaviour();

            totalWeight += inputWeight;

            if (input.rimLightGroup == null) continue;

            if (input.overwriting)
            {
                weightedVirtualRimLightParameters.Add((rimLightParameters: new[] {input.overwriteParameter},
                    groupWeight: inputWeight));
            }
            else
            {
                var rimLightParameters = input.rimLightGroup.VirtualRimLights
                    .Select(virtualRimLight => virtualRimLight.Parameters).ToList();
                weightedVirtualRimLightParameters.Add(
                    (rimLightParameters: rimLightParameters, groupWeight: inputWeight));
            }
        }

        if (!Mathf.Approximately(totalWeight, 0.0f))
        {
            trackBinding.SetRimLightParameterOfAll(
                RimLightGroup.GetBlendedRimLightParameterWithWeight(weightedVirtualRimLightParameters,
                    trackBinding.TargetTransform));
        }
        else
        {
            trackBinding.SetRimLightParameterOfAll(initialParameter);
        }
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        firstFrameHappened = false;

        if (trackBinding == null) return;

        trackBinding.SetRimLightParameterOfAll(initialParameter);
    }
}