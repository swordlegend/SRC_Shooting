﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのボスの振る舞いのセットを保持するクラス。
/// </summary>
[Serializable]
public class BattleRealBossBehaviorSet
{
    [Header("通常の振る舞い")]

    [SerializeField, Tooltip("ボスの通常の振る舞いのタイプ")]
    private E_ENEMY_BEHAVIOR_TYPE m_BehaviorType;
    public E_ENEMY_BEHAVIOR_TYPE BehaviorType => m_BehaviorType;

    [SerializeField, Tooltip("ボスの通常の振る舞い")]
    private BattleRealEnemyBehaviorUnit m_Behavior;
    public BattleRealEnemyBehaviorUnit Behavior => m_Behavior;

    [SerializeField, Tooltip("ボスの通常の振る舞いのグループ")]
    private BattleRealEnemyBehaviorGroup m_BehaviorGroup;
    public BattleRealEnemyBehaviorGroup BehaviorGroup => m_BehaviorGroup;

    [Header("ダウン時の振る舞い")]

    [SerializeField, Tooltip("ボスのダウン時の振る舞いのタイプ")]
    private E_ENEMY_BEHAVIOR_TYPE m_DownBehaviorType;
    public E_ENEMY_BEHAVIOR_TYPE DownBehaviorType => m_DownBehaviorType;

    [SerializeField, Tooltip("ボスのダウン時の振る舞い")]
    private BattleRealEnemyBehaviorUnit m_DownBehavior;
    public BattleRealEnemyBehaviorUnit DownBehavior => m_DownBehavior;

    [SerializeField, Tooltip("ボスのダウン時の振る舞いのグループ")]
    private BattleRealEnemyBehaviorGroup m_DownBehaviorGroup;
    public BattleRealEnemyBehaviorGroup DownBehaviorGroup => m_DownBehaviorGroup;

    [Space()]

    [SerializeField, Tooltip("ハッキングを開始した時に読み込まれるレベルデータ")]
    private BattleHackingLevelParamSet m_HackingLevelParamSet;
    public BattleHackingLevelParamSet HackingLevelParamSet => m_HackingLevelParamSet;

    [SerializeField, Tooltip("この振る舞いに切り替わった時に用いるシーケンスデータ")]
    private SequenceGroup m_StartSequenceGroup;
    public SequenceGroup StartSequenceGroup => m_StartSequenceGroup;

    [SerializeField, Tooltip("この振る舞いでのダウンHP")]
    private int m_DownHp;
    public int DownHp => m_DownHp;

    [SerializeField, Tooltip("ダウンから復帰するまでの時間")]
    private float m_DownHealTime;
    public float DownHealTime => m_DownHealTime;

    [SerializeField, Tooltip("次の振る舞いセットへと遷移するHPの割合 これを下回ると次の振る舞いへと遷移する"), Range(0, 1)]
    private float m_HpRateThresholdNextBehavior;
    public float HpRateThresholdNextBehavior => m_HpRateThresholdNextBehavior;

    [SerializeField, Tooltip("ハッキング成功時にばらまくアイテムのパラメータ")]
    private ItemCreateParam m_HackingSuccessItemParam;
    public ItemCreateParam HackingSuccessItemParam => m_HackingSuccessItemParam;
}