﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

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
    private Text m_ChapterIndicator;

    [SerializeField]
    private Text m_DifficultyIndicator;

    [SerializeField]
    private TextValueIndicator m_BestScoreIndicator;

    [SerializeField]
    private TextValueIndicator m_ScoreIndicator;

    [SerializeField]
    private TextValueIndicator m_ChainIndicator;

    [SerializeField]
    private IconCountIndicator m_LifeIndicator;

    [SerializeField]
    private IconCountIndicator m_LevelIcon;

    [SerializeField]
    private IconGaugeIndicator m_LevelGage;

    [SerializeField]
    private IconCountIndicator m_EnergyIcon;

    [SerializeField]
    private IconGaugeIndicator m_EnergyGage;

    [SerializeField]
    private WeaponIndicator m_WeaponIndicator;

    [Space()]
    [Header("Achievement")]

    [SerializeField]
    private GameObject m_AchievementRoot;

    [SerializeField]
    private List<AchievementIndicator> m_AchievementIndicators;

    [Space()]
    [Header("Boss UI")]

    [SerializeField]
    private BattleRealBossUI m_BossUiLeft;

    [SerializeField]
    private BattleRealBossUI m_BossUiRight;

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

    [Header("Result")]

    [SerializeField]
    private ResultIndicator m_ResultIndicator;

    [Header("Game Over")]

    [SerializeField]
    private GameOverController m_GameOverController;

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
        m_IsShowResult = false;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_ChapterIndicator.text = DataManager.Instance.GetChapterString();
        m_DifficultyIndicator.text = DataManager.Instance.Difficulty.ToString();

        m_FrontViewEffect.OnInitialize();
        m_BestScoreIndicator.OnInitialize();
        m_ScoreIndicator.OnInitialize();
        m_ChainIndicator.OnInitialize();
        m_LifeIndicator.OnInitialize();
        m_LevelIcon.OnInitialize();
        m_LevelGage.OnInitialize();
        m_EnergyIcon.OnInitialize();
        m_EnergyGage.OnInitialize();
        m_WeaponIndicator.OnInitialize();

        m_BossUiLeft.OnInitialize();
        m_BossUiRight.OnInitialize();

        m_ResultIndicator.OnInitialize();
        m_GameOverController.OnInitialize();
        m_GameOverController.EndAction += OnEndGameOver;

        m_StartTelop.OnInitialize();
        m_WarningTelop.OnInitialize();
        m_ClearTelop.OnInitialize();

        if (DataManager.Instance.IsInvalidAchievement())
        {
            m_AchievementRoot?.SetActive(false);
        }
        else
        {
            if (m_AchievementIndicators != null)
            {
                m_AchievementIndicators.ForEach(i => i.OnInitialize());
            }
        }

        DisableAllBossUI();
    }

    public override void OnFinalize()
    {
        EndAction = null;

        if (!DataManager.Instance.IsInvalidAchievement() && m_AchievementIndicators != null)
        {
            m_AchievementIndicators.ForEach(i => i.OnFinalize());
        }

        m_ClearTelop.OnFinalize();
        m_WarningTelop.OnFinalize();
        m_StartTelop.OnFinalize();

        m_GameOverController.OnFinalize();
        m_ResultIndicator.OnFinalize();

        m_BossUiRight.OnFinalize();
        m_BossUiLeft.OnFinalize();

        m_WeaponIndicator.OnFinalize();
        m_EnergyGage.OnFinalize();
        m_EnergyIcon.OnFinalize();
        m_LevelGage.OnFinalize();
        m_LevelIcon.OnFinalize();
        m_LifeIndicator.OnFinalize();
        m_ChainIndicator.OnFinalize();
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
        m_ChainIndicator.OnUpdate();
        m_LifeIndicator.OnUpdate();
        m_LevelIcon.OnUpdate();
        m_LevelGage.OnUpdate();
        m_EnergyIcon.OnUpdate();
        m_EnergyGage.OnUpdate();
        m_WeaponIndicator.OnUpdate();

        m_BossUiLeft.OnUpdate();
        m_BossUiRight.OnUpdate();

        m_ResultIndicator.OnUpdate();
        m_GameOverController.OnUpdate();

        m_StartTelop.OnUpdate();
        m_WarningTelop.OnUpdate();
        m_ClearTelop.OnUpdate();

        if (!DataManager.Instance.IsInvalidAchievement() && m_AchievementIndicators != null)
        {
            m_AchievementIndicators.ForEach(i => i.OnUpdate());
        }


        if (m_IsShowResult && Input.anyKey)
        {
            m_IsShowResult = false;
            EndAction?.Invoke();
        }
    }

    #endregion

    /// <summary>
    /// リアルモードに遷移する時のビューの透明度アニメーションを再生する。
    /// </summary>
    public void PlayToReal()
    {
        m_MainViewAnimator.Play(TO_REAL, 0);
        m_ResultViewAnimator.Play(TO_REAL, 0);
    }

    /// <summary>
    /// リアルモードのUIの透明度を設定する。
    /// </summary>
    public void SetAlpha(float normalizedAlpha)
    {
        m_MainUiGroup.alpha = normalizedAlpha;
        m_ResultUiGroup.alpha = normalizedAlpha;
    }

    /// <summary>
    /// ボスUIを有効にする。
    /// </summary>
    public void EnableBossUI(BattleRealBossController boss)
    {
        if (boss == null)
        {
            return;
        }

        if (m_BossUiLeft != null && m_BossUiLeft.ReferencedBoss == null)
        {
            m_BossUiLeft.EnableBossUI(boss);
        }
        else if (m_BossUiRight != null && m_BossUiRight.ReferencedBoss == null)
        {
            m_BossUiRight.EnableBossUI(boss);
        }
    }

    public void DisableAllBossUI()
    {
        m_BossUiLeft?.DisableBossUI();
        m_BossUiRight?.DisableBossUI();
    }

    /// <summary>
    /// ゲームオーバー演出を再生する。
    /// </summary>
    public void PlayGameOver()
    {
        m_GameOverController.PlayGameOver();
    }

    private void OnEndGameOver()
    {
        EndAction?.Invoke();
    }

    /// <summary>
    /// ゲームスタート演出を再生する。
    /// </summary>
    public void PlayStartTelop(Action onEnd = null)
    {
        m_StartTelop.Play(null, false, onEnd);
    }

    /// <summary>
    /// 警告演出を再生する。
    /// </summary>
    [Obsolete("どこにも使用しないはずです。")]
    public void PlayWarningTelop(Action onEnd = null)
    {
        m_WarningTelop.Play(null, false, onEnd);
    }

    /// <summary>
    /// ゲームクリア演出を再生する。
    /// </summary>
    public void PlayClearTelop(Action onEnd = null)
    {
        m_ClearTelop.Play(null, false, onEnd);
    }

    /// <summary>
    /// 結果UIを表示することに先立ってメインビューを隠すアニメーションを再生する。
    /// </summary>
    public void PlayMainViewHideAnimationBeforeShowResult()
    {
        m_MainViewAnimator.Play(TO_HACKING, 0);
    }

    /// <summary>
    /// 結果UIを表示する。
    /// </summary>
    public void ShowResult()
    {
        m_ResultIndicator.PlayResult();
        m_IsShowResult = true;
    }
}
