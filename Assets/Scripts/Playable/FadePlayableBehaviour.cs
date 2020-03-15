﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Timeline上で、FadeManagerに処理を渡すための動作。
/// </summary>
public class FadePlayableBehaviour : PlayableBehaviour
{
    public FadeParam FadeParam;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {

    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (FadeManager.Instance == null)
        {
            Debug.LogWarningFormat("{0} : FadeManager is null. Fade is invalid!", GetType().Name);
            return;
        }

        FadeManager.Instance.Fade(FadeParam);
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }
}