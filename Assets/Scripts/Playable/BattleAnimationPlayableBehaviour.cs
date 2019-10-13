﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// AnimationTrackの代わりにAnimationCurveでTransformを制御する。
/// </summary>
public class BattleAnimationPlayableBehaviour : PlayableBehaviour
{
    /// <summary>
    /// アニメーションのターゲット
    /// </summary>
    [SerializeField]
    private Transform m_AnimationTarget;

    /// <summary>
    /// アニメーションデータ
    /// </summary>
    [SerializeField]
    private BattleAnimationParam m_AnimParam;

    private Vector3 m_InitPosition;
    private Vector3 m_InitRotation;
    private Vector3 m_InitLocalPosition;
    private Vector3 m_InitLocalRotation;
    private Vector3 m_InitLocalScale;

    private BattleAnimationParam.BattleAnimationVectorParam m_AnimPosition;
    private BattleAnimationParam.BattleAnimationVectorParam m_AnimRotation;
    private BattleAnimationParam.BattleAnimationVectorParam m_AnimScale;

    public void SetArguments(Transform target, BattleAnimationParam animParam)
    {
        m_AnimationTarget = target;
        m_AnimParam = animParam;

        if (m_AnimParam.UsePosition)
        {
            m_AnimPosition = m_AnimParam.Position;
        }

        if (m_AnimParam.UseRotation)
        {
            m_AnimRotation = m_AnimParam.Rotation;
        }

        if (m_AnimParam.UseScale)
        {
            m_AnimScale = m_AnimParam.Scale;
        }
    }

    /// <summary>
    /// Timelineが始まった時に呼び出される。
    /// このトラックが始まる時とは異なる。
    /// </summary>
    public override void OnGraphStart(Playable playable)
    {
    }

    /// <summary>
    /// Timelineが終わった時に呼び出される。
    /// このトラックが終わる時とは異なる。
    /// </summary>
    public override void OnGraphStop(Playable playable)
    {
    }

    /// <summary>
    /// このトラックが始まる時に呼び出される。
    /// </summary>
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (m_AnimationTarget == null)
        {
            return;
        }

        m_InitPosition = m_AnimationTarget.position;
        m_InitRotation = m_AnimationTarget.eulerAngles;
        m_InitLocalPosition = m_AnimationTarget.localPosition;
        m_InitLocalRotation = m_AnimationTarget.localEulerAngles;
        m_InitLocalScale = m_AnimationTarget.localScale;
    }

    /// <summary>
    /// このトラックが終わる時に呼び出される。
    /// このトラックがTimelineのスタートよりも後に配置されていると、最初にも呼び出さる。
    /// </summary>
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (m_AnimationTarget == null || playable.GetTime() <= 0)
        {
            return;
        }

        ApplyEndValue();
    }

    /// <summary>
    /// フレームの前段階で呼び出される。
    /// </summary>
    public override void PrepareFrame(Playable playable, FrameData info)
    {

    }

    /// <summary>
    /// 毎フレーム呼び出される。
    /// </summary>
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (m_AnimationTarget == null)
        {
            return;
        }

        var currentTime = (float)playable.GetTime();
        var duration = (float)playable.GetDuration();
        ApplyAnimation(currentTime, duration);
    }

    private void ApplyAnimation(float currentTime, float duration)
    {
        if (m_AnimParam.UsePosition)
        {
            if (m_AnimPosition.SpaceType == Space.World)
            {
                m_AnimationTarget.position = GetAnimVector(ref m_AnimPosition, currentTime, duration, ref m_InitPosition);
            }
            else
            {
                m_AnimationTarget.localPosition = GetAnimVector(ref m_AnimPosition, currentTime, duration, ref m_InitLocalPosition);
            }
        }

        if (m_AnimParam.UseRotation)
        {
            if (m_AnimRotation.SpaceType == Space.World)
            {
                m_AnimationTarget.eulerAngles = GetAnimVector(ref m_AnimRotation, currentTime, duration, ref m_InitRotation);
            }
            else
            {
                m_AnimationTarget.localEulerAngles = GetAnimVector(ref m_AnimRotation, currentTime, duration, ref m_InitLocalRotation);
            }
        }

        if (m_AnimParam.UseScale)
        {
            m_AnimationTarget.localScale = GetAnimVector(ref m_AnimScale, currentTime, duration, ref m_InitLocalScale);
        }
    }

    private void ApplyEndValue()
    {
        if (m_AnimParam.UsePosition)
        {
            if (m_AnimPosition.SpaceType == Space.World)
            {
                m_AnimationTarget.position = GetEndVector(ref m_AnimPosition, ref m_InitPosition);
            }
            else
            {
                m_AnimationTarget.localPosition = GetEndVector(ref m_AnimPosition, ref m_InitLocalPosition);
            }
        }

        if (m_AnimParam.UseRotation)
        {
            if (m_AnimRotation.SpaceType == Space.World)
            {
                m_AnimationTarget.eulerAngles = GetEndVector(ref m_AnimRotation, ref m_InitRotation);
            }
            else
            {
                m_AnimationTarget.localEulerAngles = GetEndVector(ref m_AnimRotation, ref m_InitLocalRotation);
            }
        }

        if (m_AnimParam.UseScale)
        {
            m_AnimationTarget.localScale = GetEndVector(ref m_AnimScale, ref m_InitLocalScale);
        }
    }

    private Vector3 GetAnimVector(ref BattleAnimationParam.BattleAnimationVectorParam param, float currentTime, float duration, ref Vector3 initVector)
    {
        var vector = initVector;

        if (param.XParam.Use)
        {
            float time = param.XParam.IsNormalized ? (currentTime / duration) : currentTime;
            float value = param.XParam.AnimationValue.Evaluate(time);

            if (param.XParam.RelativeType == E_RELATIVE.RELATIVE)
            {
                vector.x += value;
            } else
            {
                vector.x = value;
            }
        }

        if (param.YParam.Use)
        {
            float time = param.YParam.IsNormalized ? (currentTime / duration) : currentTime;
            float value = param.YParam.AnimationValue.Evaluate(time);

            if (param.YParam.RelativeType == E_RELATIVE.RELATIVE)
            {
                vector.y += value;
            } else
            {
                vector.y = value;
            }
        }

        if (param.ZParam.Use)
        {
            float time = param.ZParam.IsNormalized ? (currentTime / duration) : currentTime;
            float value = param.ZParam.AnimationValue.Evaluate(time);

            if (param.ZParam.RelativeType == E_RELATIVE.RELATIVE)
            {
                vector.z += value;
            } else
            {
                vector.z = value;
            }
        }

        return vector;
    }

    private Vector3 GetEndVector(ref BattleAnimationParam.BattleAnimationVectorParam param, ref Vector3 initVector)
    {
        var vector = initVector;

        if (param.XParam.Use)
        {
            float value = param.XParam.EndValue;

            if (param.XParam.RelativeType == E_RELATIVE.RELATIVE)
            {
                vector.x += value;
            }
            else
            {
                vector.x = value;
            }
        }

        if (param.YParam.Use)
        {
            float value = param.YParam.EndValue;

            if (param.YParam.RelativeType == E_RELATIVE.RELATIVE)
            {
                vector.y += value;
            }
            else
            {
                vector.y = value;
            }
        }

        if (param.ZParam.Use)
        {
            float value = param.ZParam.EndValue;

            if (param.ZParam.RelativeType == E_RELATIVE.RELATIVE)
            {
                vector.z += value;
            }
            else
            {
                vector.z = value;
            }
        }

        return vector;
    }
}