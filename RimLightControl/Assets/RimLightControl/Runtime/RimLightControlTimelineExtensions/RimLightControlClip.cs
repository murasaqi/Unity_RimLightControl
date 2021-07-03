using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class RimLightControlClip : PlayableAsset, ITimelineClipAsset
{
    public ExposedReference<RimLightGroup> rimLightGroup;
    public ExposedReference<Transform> overwriteLightTransform;
    public RimLightControlBehaviour template = new RimLightControlBehaviour();

    public ClipCaps clipCaps => ClipCaps.Blending;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<RimLightControlBehaviour>.Create(graph, template);
        var clone = playable.GetBehaviour();
        clone.rimLightGroup = rimLightGroup.Resolve(graph.GetResolver());
        clone.overwriteParameter.transform = overwriteLightTransform.Resolve(graph.GetResolver());

        return playable;
    }
}