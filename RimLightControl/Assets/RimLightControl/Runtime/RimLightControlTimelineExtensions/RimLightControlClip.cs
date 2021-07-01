using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class RimLightControlClip : PlayableAsset, ITimelineClipAsset
{
    public RimLightControlBehaviour template = new RimLightControlBehaviour();
    public ExposedReference<RimLightGroup> rimLightGroup;
    public bool overwriting;
    public VirtualRimLight.Parameter overwriteParameter;
    public ExposedReference<Transform> overwriteLightTransform;

    public ClipCaps clipCaps => ClipCaps.Blending;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<RimLightControlBehaviour>.Create(graph, template);
        var clone = playable.GetBehaviour();
        clone.rimLightGroup = rimLightGroup.Resolve(graph.GetResolver());
        clone.overwriting = overwriting;
        clone.overwriteParameter = overwriteParameter;
        clone.overwriteParameter.transform = overwriteLightTransform.Resolve(graph.GetResolver());

        return playable;
    }
}