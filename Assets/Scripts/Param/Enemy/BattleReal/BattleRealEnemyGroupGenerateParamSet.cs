﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 敵グループの生成パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/EnemyGroupGenerate", fileName = "param.battle_real_enemy_group_generate.asset")]
public class BattleRealEnemyGroupGenerateParamSet : ScriptableObject
{
    [SerializeField, Tooltip("敵グループの生成ビューポート座標")]
    private Vector2 m_ViewPortPos;
    public Vector2 ViewPortPos => m_ViewPortPos;

    [SerializeField, Tooltip("敵グループのビュポート座標からのオフセット座標")]
    private Vector2 m_OffsetPosFromViewPort;
    public Vector2 OffsetPosFromViewPort => m_OffsetPosFromViewPort;

    [SerializeField, Tooltip("敵グループの生成角度")]
    private float m_GenerateAngle;
    public float GenerateAngle => m_GenerateAngle;
    
    [SerializeField, Tooltip("敵グループの振る舞いパラメータのセット")]
    private BattleRealEnemyGroupBehaviorParamSet m_EnemyGroupBehaviorParamSet;
    public BattleRealEnemyGroupBehaviorParamSet EnemyGroupBehaviorParamSet => m_EnemyGroupBehaviorParamSet;

    [SerializeField, Tooltip("敵グループ内の個別の生成パラメータ群")]
    private EnemyIndividualGenerateParamSet[] m_IndividualGenerateParamSets;
    public EnemyIndividualGenerateParamSet[] IndividualGenerateParamSets => m_IndividualGenerateParamSets;
}

/// <summary>
/// 敵グループの中での敵の個別の生成パラメータのセット。
/// </summary>
[Serializable]
public class EnemyIndividualGenerateParamSet
{
    [SerializeField, Tooltip("敵グループが生成されてからの相対的な生成時間。")]
    private float m_GenerateTime;
    public float GenerateTime => m_GenerateTime;

    [SerializeField, Tooltip("生成座標系を敵グループを基準にするか。")]
    private E_RELATIVE m_Relative;
    public E_RELATIVE Relative => m_Relative;

    [SerializeField, Tooltip("生成するビューポート座標。RELATIVEにすると、敵グループの位置の相対座標になる。")]
    private Vector2 m_ViewPortPos;
    public Vector2 ViewPortPos => m_ViewPortPos;

    [SerializeField, Tooltip("ビューポート座標からのオフセット座標。RELATIVEにすると、敵グループの位置の相対座標になる。")]
    private Vector2 m_OffsetPosFromViewPort;
    public Vector2 OffsetPosFromViewPort => m_OffsetPosFromViewPort;

    [SerializeField, Tooltip("生成角度。RELATIVEにすると、敵グループの角度の相対角度になる。")]
    private float m_GenerateAngle;
    public float GenerateAngle => m_GenerateAngle;

    [SerializeField]
    private BattleRealEnemyGenerateParamSet m_EnemyGenerateParamSet;
    public BattleRealEnemyGenerateParamSet EnemyGenerateParamSet => m_EnemyGenerateParamSet;
}