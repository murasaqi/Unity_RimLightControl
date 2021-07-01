using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class RimLightControlBehaviour : PlayableBehaviour
{
    [HideInInspector] public RimLightGroup rimLightGroup;
    [HideInInspector] public bool overwriting;
    [HideInInspector] public VirtualRimLight.Parameter overwriteParameter;

    public override void OnPlayableCreate(Playable playable)
    {
    }
}