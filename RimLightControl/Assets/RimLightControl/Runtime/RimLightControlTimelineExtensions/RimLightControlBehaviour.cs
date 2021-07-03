using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class RimLightControlBehaviour : PlayableBehaviour
{
    public RimLightGroup rimLightGroup;
    public bool overwriting;
    public VirtualRimLight.Parameter overwriteParameter;

    public override void OnPlayableCreate(Playable playable)
    {
    }
}