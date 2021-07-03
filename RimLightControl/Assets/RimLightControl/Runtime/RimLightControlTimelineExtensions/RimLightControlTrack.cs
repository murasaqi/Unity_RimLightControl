using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackClipType(typeof(RimLightControlClip))]
[TrackBindingType(typeof(RimLightingMaterialGroup))]
public class RimLightControlTrack : TrackAsset
{
    private RimLightingMaterialGroup trackBinding;
    private VirtualRimLight.Parameter initialParameter;

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<RimLightControlMixerBehaviour>.Create(graph, inputCount);
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
        trackBinding = director.GetGenericBinding(this) as RimLightingMaterialGroup;
        if (trackBinding == null) return;

        initialParameter = trackBinding.GetRimLightParametersOfFirst();

        base.GatherProperties(director, driver);
    }

    public void OnDestroy()
    {
        if (trackBinding == null) return;

        trackBinding.SetRimLightParameterOfAll(initialParameter);
    }
}