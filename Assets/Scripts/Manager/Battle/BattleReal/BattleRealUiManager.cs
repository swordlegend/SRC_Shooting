﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BattleRealUiManager : SingletonMonoBehavior<BattleRealUiManager>
{
    private const string TO_HACKING = "battle_real_ui_to_hacking";
    private const string TO_REAL = "battle_real_ui_to_real";
    private const string CLEAR_WITHOUT_HACKING_COMPLETE = "stage_clear_without_hacking_complete";
    private const string CLEAR_WITH_HACKING_COMPLETE = "stage_clear_with_hacking_complete";
    private const string CLEAR_CLOSE = "stage_clear_close_banner";

    #region Field Inspector

    [SerializeField]
    private CanvasGroup m_MainUiGroup;

    [SerializeField]
    private CanvasGroup m_ResultUiGroup;

    [Header("Front View")]

    [SerializeField]
    private FrontViewEffect m_FrontViewEffect;
    public FrontViewEffect FrontViewEffect => m_FrontViewEffect;

    [Header("Indicator")]

    [SerializeField]
    private Text m_ModeIndicator;

    [SerializeField]
    private Text m_StageIndicator;

    [SerializeField]
    private ScoreIndicator m_BestScoreIndicator;

    [SerializeField]
    private ScoreIndicator m_ScoreIndicator;

    [SerializeField]
    private IconCountIndicator m_LifeIndicator;

    [SerializeField]
    private IconCountIndicator m_LevelIcon;

    [SerializeField]
    private IconGageIndicator m_LevelGage;

    [SerializeField]
    private IconCountIndicator m_EnergyIcon;

    [SerializeField]
    private IconGageIndicator m_EnergyGage;

    [SerializeField]
    private WeaponIndicator m_WeaponIndicator;
    [SerializeField]
    private IconGageIndicator m_BossHpGage;
    [SerializeField]
    private IconGageIndicator m_BossDownGage;
    [SerializeField]
    private RemainingHackingNumIndicator m_BossRemainingHackingIndicator;

    [SerializeField]
    private GameObject m_BossUI;

    [Header("Telop Indicator")]

    [SerializeField]
    private TelopEffectIndicator m_StartTelop;

    [SerializeField]
    private TelopEffectIndicator m_WarningTelop;

    [SerializeField]
    private TelopEffectIndicator m_ClearTelop;

    [Header("Animator")]

    [SerializeField]
    private Animator m_MainViewAnimator;

    [SerializeField]
    private Animator m_ResultViewAnimator;

    [SerializeField]
    private Animator m_StageClearAnimator;

    [Header("Result")]

    [SerializeField]
    private ResultIndicator m_ResultIndicator;

    #endregion

    private bool m_IsShowResult;

    #region Closed Callback

    private Action EndAction { get; set; }

    #endregion

    public void SetCallback(BattleRealManager manager)
    {
        EndAction += manager.End;
    }

    #region Game Cycle

    protected override void OnAwake()
    {
        base.OnAwake();
        m_StageClearAnimator.gameObject.SetActive(false);
        m_IsShowResult = false;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        var battleData = DataManager.Instance.BattleData;

        m_ModeIndicator.text = battleData.GameMode.ToString();
        m_StageIndicator.text = battleData.Stage.ToString().Replace("_", " ");

        m_FrontViewEffect.OnInitialize();
        m_BestScoreIndicator.OnInitialize();
        m_ScoreIndicator.OnInitialize();
        m_LifeIndicator.OnInitialize();
        m_LevelIcon.OnInitialize();
        m_LevelGage.OnInitialize();
        m_EnergyIcon.OnInitialize();
        m_EnergyGage.OnInitialize();
        m_WeaponIndicator.OnInitialize();
        m_BossHpGage.OnInitialize();
        m_BossDownGage.OnInitialize();
        m_BossRemainingHackingIndicator.OnInitialize();
        m_ResultIndicator.OnInitialize();

        m_StartTelop.OnInitialize();
        m_WarningTelop.OnInitialize();
        m_ClearTelop.OnInitialize();

        SetEnableBossUI(false);
    }

    public override void OnFinalize()
    {
        EndAction = null;

        m_ClearTelop.OnFinalize();
        m_WarningTelop.OnFinalize();
        m_StartTelop.OnFinalize();

        m_ResultIndicator.OnFinalize();
        m_BossRemainingHackingIndicator.OnFinalize();
        m_BossDownGage.OnFinalize();
        m_BossHpGage.OnFinalize();
        m_WeaponIndicator.OnFinalize();
        m_EnergyGage.OnFinalize();
        m_EnergyIcon.OnFinalize();
        m_LevelGage.OnFinalize();
        m_LevelIcon.OnFinalize();
        m_LifeIndicator.OnFinalize();
        m_ScoreIndicator.OnFinalize();
        m_BestScoreIndicator.OnFinalize();
        m_FrontViewEffect.OnFinalize();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_FrontViewEffect.OnUpdate();
        m_BestScoreIndicator.OnUpdate();
        m_ScoreIndicator.OnUpdate();
        m_LifeIndicator.OnUpdate();
        m_LevelIcon.OnUpdate();
        m_LevelGage.OnUpdate();
        m_EnergyIcon.OnUpdate();
        m_EnergyGage.OnUpdate();
        m_WeaponIndicator.OnUpdate();
        m_BossHpGage.OnUpdate();
        m_BossDownGage.OnUpdate();
        m_BossRemainingHackingIndicator.OnUpdate();
        m_ResultIndicator.OnUpdate();

        m_StartTelop.OnUpdate();
        m_WarningTelop.OnUpdate();
        m_ClearTelop.OnUpdate();
        
        if (m_IsShowResult && Input.anyKey)
        {
            m_IsShowResult = false;
            EndAction?.Invoke();
        }
    }

    #endregion

    public void PlayToHacking()
    {
        m_MainViewAnimator.Play(TO_HACKING, 0);
        m_ResultViewAnimator.Play(TO_HACKING, 0);
    }

    public void PlayToReal()
    {
        m_MainViewAnimator.Play(TO_REAL, 0);
        m_ResultViewAnimator.Play(TO_REAL, 0);
    }

    public void SetAlpha(float normalizedAlpha)
    {
        m_MainUiGroup.alpha = normalizedAlpha;
        m_ResultUiGroup.alpha = normalizedAlpha;
    }

    public void SetEnableBossUI(bool isEnable){
        m_BossUI.SetActive(isEnable);
    }

    public void PlayGameClearAnimation()
    {
        //m_StageClearAnimator.gameObject.SetActive(true);
        //if (DataManager.Instance.BattleData.IsHackingComplete)
        //{
        //    m_StageClearAnimator.Play(CLEAR_WITH_HACKING_COMPLETE, 0);
        //}
        //else
        //{
        //    m_StageClearAnimator.Play(CLEAR_WITHOUT_HACKING_COMPLETE, 0);
        //}
        PlayClearTelop();
    }

    public void PlayStartTelop(Action onEnd = null)
    {
        m_StartTelop.Play(null, false, onEnd);
    }

    public void PlayWarningTelop(Action onEnd = null)
    {
        m_WarningTelop.Play(null, false, onEnd);
    }

    public void PlayClearTelop(Action onEnd = null)
    {
        m_ClearTelop.Play(DataManager.Instance.BattleData.IsHackingComplete ? "Hacking Complete" : null, false, onEnd);
    }

    public void PlayMainViewHideAnimation()
    {
        m_MainViewAnimator.Play(TO_HACKING, 0);
    }

    public void DisplayResult()
    {
        m_ResultIndicator.PlayResult();
        m_IsShowResult = true;
    }
}
