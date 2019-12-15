﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 複数ステージを跨ぐようなバトルデータを保持する。
/// </summary>
public class BattleData
{
    #region Field

    private BattleRealPlayerLevelParamSet m_PlayerLevelParamSet;

    /// <summary>
    /// ゲームモード
    /// </summary>
    public E_GAME_MODE GameMode { get; private set; }

    /// <summary>
    /// ステージ
    /// </summary>
    public E_STAGE Stage { get; private set; }

    /// <summary>
    /// 残機
    /// </summary>
    public int PlayerLife { get; private set; }

    /// <summary>
    /// 同じ挑戦条件のベストスコア
    /// </summary>
    public double BestScore { get; private set; }

    /// <summary>
    /// 現在スコア
    /// </summary>
    public double Score { get; private set; }

    /// <summary>
    /// 現在レベル
    /// </summary>
    public int Level { get; private set; }

    /// <summary>
    /// 現在EXP
    /// </summary>
    public int Exp { get; private set; }

    /// <summary>
    /// チャージしきったエナジーの数
    /// </summary>
    public int EnergyCount { get; private set; }

    /// <summary>
    /// チャージ中のエナジー
    /// </summary>
    public float EnergyCharge { get; private set; }

    /// <summary>
    /// ハッキング成功回数
    /// </summary>
    public int HackingSucceedCount { get; private set; }

    /// <summary>
    /// 残機の最大保持数
    /// </summary>
    public int MaxPlayerLife { get; private set; }

    /// <summary>
    /// 最大レベル
    /// </summary>
    public int MaxLevel { get; private set; }

    /// <summary>
    /// エナジーの最大保持数
    /// </summary>
    public int MaxEnergyCount { get; private set; }

    /// <summary>
    /// エナジー1つ分とみなすエナジーチャージ量
    /// </summary>
    public float MaxEnergyCharge { get; private set; }

    /// <summary>
    /// ハッキングに成功した時のスコア単価
    /// </summary>
    public float HackingSuccessBonus { get; private set; }

    #endregion

    public BattleData(BattleRealPlayerLevelParamSet playerLevelParamSet)
    {
        m_PlayerLevelParamSet = playerLevelParamSet;

        GameMode = E_GAME_MODE.STORY;
        Stage = E_STAGE.NORMAL_1;
    }

    public void ResetData(E_STAGE stage)
    {
        if (m_PlayerLevelParamSet == null)
        {
            Debug.LogError("PlayerLevelParamSetがありません。");
            return;
        }

        // 定数の初期化
        var defData = m_PlayerLevelParamSet.CommonDefData;
        MaxPlayerLife = defData.MaxPlayerLifeNum;
        MaxLevel = defData.MaxLevel;
        MaxEnergyCount = defData.MaxEnergyCount;
        MaxEnergyCharge = defData.MaxEnergyCharge;
        HackingSuccessBonus = defData.HackingSuccessBonus;

        // 初期値が共通なものを初期化
        BestScore = PlayerRecordManager.Instance.GetTopRecord().m_FinalScore;
        Score = 0;
        Exp = 0;
        EnergyCharge = 0;
        HackingSucceedCount = 0;

        // ステージに応じて初期値が異なるものを初期化
        switch (stage)
        {
            case E_STAGE.EASY_0:
            case E_STAGE.NORMAL_0:
            case E_STAGE.HARD_0:
            case E_STAGE.HADES_0:
                InitData(m_PlayerLevelParamSet.Stage0InitData);
                break;
            default:
                InitData(m_PlayerLevelParamSet.Stage1InitData);
                break;
        }
    }

    private void InitData(BattleInitData initData)
    {
        if (initData == null)
        {
            Debug.LogError("initDataがありません。");
            return;
        }

        PlayerLife = initData.InitPlayerLife;
        Level = initData.InitLevel;
        EnergyCount = initData.InitEnergyCount;
    }

    public BattleRealPlayerLevel GetCurrentLevelParam()
    {
        return m_PlayerLevelParamSet.PlayerLevels[Level];
    }

    #region Player Life

    public void AddPlayerLife(int num)
    {
        PlayerLife += num;
    }

    public void DecreasePlayerLife()
    {
        PlayerLife = Mathf.Max(PlayerLife - 1, 0);
    }

    #endregion

    #region Score

    public void AddScore(double score)
    {
        Score += score;
    }

    #endregion

    #region BestScore

    public void ResetBestScore()
    {
        BestScore = PlayerRecordManager.Instance.GetTopRecord().m_FinalScore;
    }

    public void UpdateBestScore(double score)
    {
        BestScore = score;
    }

    #endregion

    #region Level

    #endregion

    #region Exp

    public void AddExp(int exp)
    {
        var levelNum = m_PlayerLevelParamSet.PlayerLevels.Length;

        // スコア増加(レベルMAXの時)
        if (Level == levelNum - 1)
        {
            AddScore(exp);
            return;
        }

        var addedExp = Exp + exp;
        var expParamSet = GetCurrentLevelParam();

        while (addedExp >= expParamSet.NecessaryExpToLevelUpNextLevel)
        {
            if (Level < levelNum - 1)
            {
                addedExp %= expParamSet.NecessaryExpToLevelUpNextLevel;
                Level++;

                // レベルMAXになった時
                if (Level >= levelNum - 1)
                {
                    var powerUpSe = BattleRealPlayerManager.Instance.ParamSet.PowerUpSe;
                    AudioManager.Instance.Play(powerUpSe);
                }
            }

            expParamSet = GetCurrentLevelParam();
        }

        if (Level == levelNum - 1)
        {
            addedExp = GetCurrentLevelParam().NecessaryExpToLevelUpNextLevel;
        }

        Exp = addedExp;
    }

    #endregion

    #region Energy

    public void AddEnergyCharge(float charge)
    {
        var addedCharge = EnergyCharge + charge;
        while (addedCharge >= MaxEnergyCharge)
        {
            if (EnergyCount < 10)
            {
                AddEnergyCount(1);
                addedCharge %= MaxEnergyCharge;
            }
            else
            {
                AddScore(addedCharge);
                break;
            }
        }

        if (EnergyCount == 10)
        {
            EnergyCharge = MaxEnergyCharge;
        }
        else
        {
            EnergyCharge = addedCharge;
        }
    }

    public void AddEnergyCount(int num)
    {
        if (num < 1)
        {
            return;
        }
        EnergyCount += num;
    }

    public void ConsumeEnergyCount(int num)
    {
        if (num < 1)
        {
            return;
        }
        EnergyCount -= num;
    }

    #endregion

    #region Hacking Succeed Count

    public void OnHackingResult(bool isHackingSuccess)
    {
        if (isHackingSuccess)
        {
            HackingSucceedCount++;
            if (HackingSucceedCount >= 1)
            {
                AddScore(HackingSuccessBonus * HackingSucceedCount);
            }
        }
        else
        {
            HackingSucceedCount = 0;
        }
    }

    #endregion
}
