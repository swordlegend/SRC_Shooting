﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルのリアルモードの処理を管理する。
/// </summary>
public class BattleRealManager : ControllableObject
{
    public enum E_BATTLE_REAL_STATE
    {
        START,
        BEFORE_BEGIN_GAME,
        BEFORE_BEGIN_GAME_PERFORMANCE,
        BEGIN_GAME,
        GAME,
        DEAD,
        BEFORE_BOSS_BATTLE_PERFORMANCE,
        TRANSITION_TO_HACKING,
        STAY_HACKING,
        TRANSITION_FROM_HACKING,
        BEFORE_GAME_CLEAR_PERFORMANCE,
        GAME_CLEAR,
        GAME_OVER,
        END,
    }

    #region Field

    private BattleRealParamSet m_ParamSet;

    private StateMachine<E_BATTLE_REAL_STATE> m_StateMachine;

    public BattleRealInputManager InputManager { get; private set; }

    public BattleRealTimerManager TimerManager { get; private set; }
    public BattleRealEventManager EventManager { get; private set; }
    public BattleRealPlayerManager PlayerManager { get; private set; }
    public BattleRealEnemyManager EnemyManager { get; private set; }
    public BattleRealBulletManager BulletManager { get; private set; }
    public BattleRealItemManager ItemManager { get; private set; }

    #endregion

    public static BattleRealManager Instance => BattleManager.Instance.RealManager;

    public BattleRealManager(BattleRealParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_BATTLE_REAL_STATE>();

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.START)
        {
            OnStart = StartOnStart,
            OnUpdate = UpdateOnStart,
            OnLateUpdate = LateUpdateOnStart,
            OnFixedUpdate = FixedUpdateOnStart,
            OnEnd = EndOnStart,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME)
        {
            OnStart = StartOnBeforeBeginGame,
            OnUpdate = UpdateOnBeforeBeginGame,
            OnLateUpdate = LateUpdateOnBeforeBeginGame,
            OnFixedUpdate = FixedUpdateOnBeforeBeginGame,
            OnEnd = EndOnBeforeBeginGame,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME_PERFORMANCE)
        {
            OnStart = StartOnBeforeBeginGamePerformance,
            OnUpdate = UpdateOnBeforeBeginGamePerformance,
            OnLateUpdate = LateUpdateOnBeforeBeginGamePerformance,
            OnFixedUpdate = FixedUpdateOnBeforeBeginGamePerformance,
            OnEnd = EndOnBeforeBeginGamePerformance,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEGIN_GAME)
        {
            OnStart = StartOnBeginGame,
            OnUpdate = UpdateOnBeginGame,
            OnLateUpdate = LateUpdateOnBeginGame,
            OnFixedUpdate = FixedUpdateOnBeforeBeginGame,
            OnEnd = EndOnBeginGame,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.GAME)
        {
            OnStart = StartOnGame,
            OnUpdate = UpdateOnGame,
            OnLateUpdate = LateUpdateOnGame,
            OnFixedUpdate = FixedUpdateOnGame,
            OnEnd = EndOnGame,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.DEAD)
        {
            OnStart = StartOnDead,
            OnUpdate = UpdateOnDead,
            OnLateUpdate = LateUpdateOnDead,
            OnFixedUpdate = FixedUpdateOnDead,
            OnEnd = EndOnDead,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEFORE_BOSS_BATTLE_PERFORMANCE)
        {
            OnStart = StartOnBeforeBossBattlePerformance,
            OnUpdate = UpdateOnBeforeBossBattlePerformance,
            OnLateUpdate = LateUpdateOnBeforeBossBattlePerformance,
            OnFixedUpdate = FixedUpdateOnBeforeBossBattlePerformance,
            OnEnd = EndOnBeforeBossBattlePerformance,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.TRANSITION_TO_HACKING)
        {
            OnStart = StartOnTransitionToHacking,
            OnUpdate = UpdateOnTransitionToHacking,
            OnLateUpdate = LateUpdateOnTransitionToHacking,
            OnFixedUpdate = FixedUpdateOnTransitionToHacking,
            OnEnd = EndOnTransitionToHacking,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.STAY_HACKING)
        {
            OnStart = StartOnStayHacking,
            OnUpdate = UpdateOnStayHacking,
            OnLateUpdate = LateUpdateOnStayHacking,
            OnFixedUpdate = FixedUpdateOnStayHacking,
            OnEnd = EndOnStayHacking,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.TRANSITION_FROM_HACKING)
        {
            OnStart = StartOnTransitionFromHacking,
            OnUpdate = UpdateOnTransitionFromHacking,
            OnLateUpdate = LateUpdateOnTransitionFromHacking,
            OnFixedUpdate = FixedUpdateOnTransitionFromHacking,
            OnEnd = EndOnTransitionFromHacking,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEFORE_GAME_CLEAR_PERFORMANCE)
        {
            OnStart = StartOnBeforeGameClearPerformance,
            OnUpdate = UpdateOnBeforeGameClearPerformance,
            OnLateUpdate = LateUpdateOnBeforeGameClearPerformance,
            OnFixedUpdate = FixedUpdateOnBeforeGameClearPerformance,
            OnEnd = EndOnBeforeGameClearPerformance,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.GAME_CLEAR)
        {
            OnStart = StartOnGameClear,
            OnUpdate = UpdateOnGameClear,
            OnLateUpdate = LateUpdateOnGameClear,
            OnFixedUpdate = FixedUpdateOnGameClear,
            OnEnd = EndOnGameClear,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.GAME_OVER)
        {
            OnStart = StartOnGameOver,
            OnUpdate = UpdateOnGameOver,
            OnLateUpdate = LateUpdateOnGameOver,
            OnFixedUpdate = FixedUpdateOnGameOver,
            OnEnd = EndOnGameOver,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.END)
        {
            OnStart = StartOnEnd,
            OnUpdate = UpdateOnEnd,
            OnLateUpdate = LateUpdateOnEnd,
            OnFixedUpdate = FixedUpdateOnEnd,
            OnEnd = EndOnEnd,
        });

        InputManager = new BattleRealInputManager();
        TimerManager = new BattleRealTimerManager();
        EventManager = new BattleRealEventManager(m_ParamSet.EventTriggerParamSet);
        PlayerManager = new BattleRealPlayerManager(m_ParamSet.PlayerManagerParamSet);
        EnemyManager = new BattleRealEnemyManager();
        BulletManager = new BattleRealBulletManager();
        ItemManager = new BattleRealItemManager();

        InputManager.OnInitialize();
        TimerManager.OnInitialize();
        EventManager.OnInitialize();
        PlayerManager.OnInitialize();
        EnemyManager.OnInitialize();
        BulletManager.OnInitialize();
        ItemManager.OnInitialize();

        m_StateMachine.Goto(E_BATTLE_REAL_STATE.START);
    }

    public override void OnFinalize()
    {
        ItemManager.OnFinalize();
        BulletManager.OnFinalize();
        EnemyManager.OnFinalize();
        PlayerManager.OnFinalize();
        EventManager.OnFinalize();
        TimerManager.OnFinalize();
        InputManager.OnFinalize();

        m_StateMachine.OnFinalize();

        base.OnFinalize();
    }

    public void OnEnableObject()
    {
        //TimerManager.OnEnableObject();
        //EventManager.OnEnableObject();
        //PlayerManager.OnEnableObject();
        //EnemyManager.OnEnableObject();
        //BulletManager.OnEnableObject();
        //ItemManager.OnEnableObject();
    }

    public void OnDisableObject()
    {
        //ItemManager.OnDisableObject();
        //BulletManager.OnDisableObject();
        //EnemyManager.OnDisableObject();
        //PlayerManager.OnDisableObject();
        //EventManager.OnDisableObject();
        //TimerManager.OnDisableObject();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_StateMachine.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        m_StateMachine.OnLateUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        m_StateMachine.OnFixedUpdate();
    }

    #endregion

    #region Start State

    private void StartOnStart()
    {
        InputManager.OnStart();
        TimerManager.OnStart();
        EventManager.OnStart();
        PlayerManager.OnStart();
        EnemyManager.OnStart();
        BulletManager.OnStart();
        ItemManager.OnStart();

        Debug.Log(1);

        m_StateMachine.Goto(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME);
    }

    private void UpdateOnStart()
    {

    }

    private void LateUpdateOnStart()
    {

    }

    private void FixedUpdateOnStart()
    {

    }

    private void EndOnStart()
    {

    }

    #endregion

    #region Before Begin Game State

    private void StartOnBeforeBeginGame()
    {
        Debug.Log(2);
        m_StateMachine.Goto(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME_PERFORMANCE);
    }

    private void UpdateOnBeforeBeginGame()
    {

    }

    private void LateUpdateOnBeforeBeginGame()
    {

    }

    private void FixedUpdateOnBeforeBeginGame()
    {

    }

    private void EndOnBeforeBeginGame()
    {

    }

    #endregion

    #region Before Begin Game Performance State

    private void StartOnBeforeBeginGamePerformance()
    {
        Debug.Log(3);
        m_StateMachine.Goto(E_BATTLE_REAL_STATE.BEGIN_GAME);
    }

    private void UpdateOnBeforeBeginGamePerformance()
    {

    }

    private void LateUpdateOnBeforeBeginGamePerformance()
    {

    }

    private void FixedUpdateOnBeforeBeginGamePerformance()
    {

    }

    private void EndOnBeforeBeginGamePerformance()
    {

    }

    #endregion

    #region Begin Game State

    private void StartOnBeginGame()
    {
        m_StateMachine.Goto(E_BATTLE_REAL_STATE.GAME);
    }

    private void UpdateOnBeginGame()
    {

    }

    private void LateUpdateOnBeginGame()
    {

    }

    private void FixedUpdateOnBeginGame()
    {

    }

    private void EndOnBeginGame()
    {

    }

    #endregion

    #region Game State

    private void StartOnGame()
    {
        InputManager.RegisterInput();
    }

    private void UpdateOnGame()
    {
        InputManager.OnUpdate();
        PlayerManager.OnUpdate();
    }

    private void LateUpdateOnGame()
    {
        PlayerManager.OnLateUpdate();
    }

    private void FixedUpdateOnGame()
    {
        PlayerManager.OnFixedUpdate();
    }

    private void EndOnGame()
    {
        InputManager.RemoveInput();
    }

    #endregion

    #region Dead State

    private void StartOnDead()
    {

    }

    private void UpdateOnDead()
    {

    }

    private void LateUpdateOnDead()
    {

    }

    private void FixedUpdateOnDead()
    {

    }

    private void EndOnDead()
    {

    }

    #endregion

    #region Before Boss Battle Performance State

    private void StartOnBeforeBossBattlePerformance()
    {

    }

    private void UpdateOnBeforeBossBattlePerformance()
    {

    }

    private void LateUpdateOnBeforeBossBattlePerformance()
    {

    }

    private void FixedUpdateOnBeforeBossBattlePerformance()
    {

    }

    private void EndOnBeforeBossBattlePerformance()
    {

    }

    #endregion

    #region Transition To Hacking State

    private void StartOnTransitionToHacking()
    {

    }

    private void UpdateOnTransitionToHacking()
    {

    }

    private void LateUpdateOnTransitionToHacking()
    {

    }

    private void FixedUpdateOnTransitionToHacking()
    {

    }

    private void EndOnTransitionToHacking()
    {

    }

    #endregion

    #region Stay Hacking State

    private void StartOnStayHacking()
    {

    }

    private void UpdateOnStayHacking()
    {

    }

    private void LateUpdateOnStayHacking()
    {

    }

    private void FixedUpdateOnStayHacking()
    {

    }

    private void EndOnStayHacking()
    {

    }

    #endregion

    #region Transition From Hacking State

    private void StartOnTransitionFromHacking()
    {

    }

    private void UpdateOnTransitionFromHacking()
    {

    }

    private void LateUpdateOnTransitionFromHacking()
    {

    }

    private void FixedUpdateOnTransitionFromHacking()
    {

    }

    private void EndOnTransitionFromHacking()
    {

    }

    #endregion

    #region Before Game Clear Performance State

    private void StartOnBeforeGameClearPerformance()
    {

    }

    private void UpdateOnBeforeGameClearPerformance()
    {

    }

    private void LateUpdateOnBeforeGameClearPerformance()
    {

    }

    private void FixedUpdateOnBeforeGameClearPerformance()
    {

    }

    private void EndOnBeforeGameClearPerformance()
    {

    }

    #endregion

    #region Game Clear State

    private void StartOnGameClear()
    {

    }

    private void UpdateOnGameClear()
    {

    }

    private void LateUpdateOnGameClear()
    {

    }

    private void FixedUpdateOnGameClear()
    {

    }

    private void EndOnGameClear()
    {

    }

    #endregion

    #region Game Over State

    private void StartOnGameOver()
    {

    }

    private void UpdateOnGameOver()
    {

    }

    private void LateUpdateOnGameOver()
    {

    }

    private void FixedUpdateOnGameOver()
    {

    }

    private void EndOnGameOver()
    {

    }

    #endregion

    #region End State

    private void StartOnEnd()
    {

    }

    private void UpdateOnEnd()
    {

    }

    private void LateUpdateOnEnd()
    {

    }

    private void FixedUpdateOnEnd()
    {

    }

    private void EndOnEnd()
    {

    }

    #endregion
}
