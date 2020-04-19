﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Bulletに渡すパラメータ。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleCommon/BulletParam", fileName = "param.bullet.asset", order = 0)]
public class BulletParam : ScriptableObject
{
    [Tooltip("弾の継続時間 これを超えると自動消滅する")]
    public float LifeTime;

    [Tooltip("弾発射時のSE 指定しない場合は何も鳴らない")]
    public PlaySoundParam ShotSE;

    [Tooltip("発射時の最初のパラメータ")]
    public BulletOrbitalParam OrbitalParam;

    [Tooltip("特殊な条件を満たす時のパラメータ")]
    public BulletOrbitalParam[] ConditionalOrbitalParams;

    [Tooltip("オプションのGameObjectパラメータ")]
    public OptionObjectParam[] OptionObjectParams;

    [Tooltip("オプションのfloatパラメータ")]
    public OptionValueParam[] OptionValueParams;

    /// <summary>
    /// 指定したインデックスのConditionalOrbitalParamを取得する。
    /// もし範囲外ならデフォルトのOrbitalParamを取得する。
    /// </summary>
    public BulletOrbitalParam GetOrbitalParam(int orbitalIndex = -1)
    {
        if (orbitalIndex < 0 || orbitalIndex >= ConditionalOrbitalParams.Length)
        {
            return OrbitalParam;
        }

        return ConditionalOrbitalParams[orbitalIndex];
    }
}
