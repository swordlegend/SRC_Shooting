﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの複数の動作を持つボスの動きを定義したコントローラ
/// </summary>
public class BattleRealBoss : BattleRealEnemyController
{
    public enum E_PHASE
    {
        START,
        ATTACK,
        DOWN,
        HACKING_SUCCESS,
        CHANGE_ATTACK,
        DEAD,
        RESCUE,
        END,
    }

    private const string DOWN_KEY = "Down";
    private const string HACKING_SUCCESS_KEY = "Hacking";

    private const string DAMAGE_COLLIDER_NAME = "Damage Collider";

    #region Field

    protected BattleRealBossParamSet m_BossParamSet;

    protected StateMachine<E_PHASE> m_StateMachine;
    protected BattleRealBossBehaviorParamSet[] m_AttackParamSets;
    protected BattleRealBossBehaviorParamSet[] m_DownParamSets;

    protected List<BattleRealBossBehavior> m_AttackBehaviors;
    protected List<BattleRealBossBehavior> m_DownBehaviors;

    protected BattleRealBossBehavior m_CurrentAttack;
    protected BattleRealBossBehavior m_CurrentDown;

    protected Transform m_DamageCollider;

    protected int m_AttackPhase;
    protected int m_DownPhase;
    protected float m_DownHp;
    protected int m_HackingSuccessCount;
    protected List<float> m_ChangeAttackHpRates;

    #endregion

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();

        if (BehaviorParamSet is BattleRealBossParamSet paramSet)
        {
            m_BossParamSet = paramSet;
            m_AttackParamSets = paramSet.AttackParamSets;
            m_DownParamSets = paramSet.DownParamSets;
            m_ChangeAttackHpRates = paramSet.ChangeAttackHpRates;
        }
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_IsLookMoveDir = false;
        m_WillDestroyOnOutOfEnemyField = false;
        IsBoss = true;

        m_AttackBehaviors = new List<BattleRealBossBehavior>();
        m_DownBehaviors = new List<BattleRealBossBehavior>();

        m_StateMachine = new StateMachine<E_PHASE>();

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.START)
        {
            m_OnStart = StartOnStart,
            m_OnUpdate = UpdateOnStart,
            m_OnLateUpdate = LateUpdateOnStart,
            m_OnFixedUpdate = FixedUpdateOnStart,
            m_OnEnd = EndOnStart,
        });

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.ATTACK)
        {
            m_OnStart = StartOnAttack,
            m_OnUpdate = UpdateOnAttack,
            m_OnLateUpdate = LateUpdateOnAttack,
            m_OnFixedUpdate = FixedUpdateOnAttack,
            m_OnEnd = EndOnAttack,
        });

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.DOWN)
        {
            m_OnStart = StartOnDown,
            m_OnUpdate = UpdateOnDown,
            m_OnLateUpdate = LateUpdateOnDown,
            m_OnFixedUpdate = FixedUpdateOnDown,
            m_OnEnd = EndOnDown,
        });

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.HACKING_SUCCESS)
        {
            m_OnStart = StartOnHackingSuccess,
            m_OnUpdate = UpdateOnHackingSuccess,
            m_OnLateUpdate = LateUpdateOnHackingSuccess,
            m_OnFixedUpdate = FixedUpdateOnHackingSuccess,
            m_OnEnd = EndOnHackingSuccess,
        });

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.CHANGE_ATTACK)
        {
            m_OnStart = StartOnChangeAttack,
            m_OnUpdate = UpdateOnChangeAttack,
            m_OnLateUpdate = LateUpdateOnChangeAttack,
            m_OnFixedUpdate = FixedUpdateOnChangeAttack,
            m_OnEnd = EndOnChangeAttack,
        });

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.DEAD)
        {
            m_OnStart = StartOnDead,
            m_OnUpdate = UpdateOnDead,
            m_OnLateUpdate = LateUpdateOnDead,
            m_OnFixedUpdate = FixedUpdateOnDead,
            m_OnEnd = EndOnDead,
        });

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.RESCUE)
        {
            m_OnStart = StartOnRescue,
            m_OnUpdate = UpdateOnRescue,
            m_OnLateUpdate = LateUpdateOnRescue,
            m_OnFixedUpdate = FixedUpdateOnRescue,
            m_OnEnd = EndOnRescue,
        });

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.END)
        {
            m_OnStart = StartOnEnd,
            m_OnUpdate = UpdateOnEnd,
            m_OnLateUpdate = LateUpdateOnEnd,
            m_OnFixedUpdate = FixedUpdateOnEnd,
            m_OnEnd = EndOnEnd,
        });

        InitializeAttackBehaviors();
        InitializeDownBehaviors();

        m_DamageCollider = transform.Find(DAMAGE_COLLIDER_NAME, false);
        BattleRealManager.Instance.OnTransitionToReal += OnTransitionToReal;

        RequestChangeState(E_PHASE.START);
    }

    public override void OnFinalize()
    {
        BattleRealManager.Instance.OnTransitionToReal -= OnTransitionToReal;
        m_DamageCollider = null;

        if (m_DownBehaviors != null)
        {
            foreach (var b in m_DownBehaviors)
            {
                b.OnFinalize();
            }
            m_DownBehaviors.Clear();
            m_DownBehaviors = null;
        }

        if (m_AttackBehaviors != null)
        {
            foreach (var b in m_AttackBehaviors)
            {
                b.OnFinalize();
            }
            m_AttackBehaviors.Clear();
            m_AttackBehaviors = null;
        }

        m_StateMachine.OnFinalize();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
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

    private BattleRealBossBehavior CreateBehavior(BattleRealBossBehaviorParamSet bossBehaviorParamSet)
    {
        if (bossBehaviorParamSet == null)
        {
            return null;
        }

        Type type = null;
        try
        {
            type = Type.GetType(bossBehaviorParamSet.BehaviorClass);
        }
        catch (Exception)
        {
            return null;
        }

        if (type == null || !type.IsSubclassOf(typeof(BattleRealBossBehavior)))
        {
            return null;
        }

        var cstr = type.GetConstructor(new[] { typeof(BattleRealEnemyController), typeof(BattleRealBossBehaviorParamSet) });
        if (cstr == null)
        {
            return null;
        }

        return cstr.Invoke(new object[] { this, bossBehaviorParamSet }) as BattleRealBossBehavior;
    }

    private void InitializeAttackBehaviors()
    {
        for (int i = 0; i < m_AttackParamSets.Length; i++)
        {
            var param = m_AttackParamSets[i];
            var behavior = CreateBehavior(param);
            if (behavior == null)
            {
                Debug.LogError("ボスの振る舞いを生成できませんでした。Type:" + param.BehaviorClass);
            }

            behavior.OnInitialize();
            m_AttackBehaviors.Add(behavior);
        }
    }

    private void InitializeDownBehaviors()
    {
        for (int i = 0; i < m_DownParamSets.Length; i++)
        {
            var param = m_DownParamSets[i];
            var behavior = CreateBehavior(param);
            if (behavior == null)
            {
                Debug.LogError("ボスの振る舞いを生成できませんでした。Type:" + param.BehaviorClass);
            }

            behavior.OnInitialize();
            m_DownBehaviors.Add(behavior);
        }
    }

    public void RequestChangeState(E_PHASE state)
    {
        m_StateMachine.Goto(state);
    }

    #region Start State

    private void StartOnStart()
    {
        m_AttackPhase = 0;
        m_DownPhase = 0;
        m_CurrentAttack = m_AttackBehaviors[m_AttackPhase];
        m_CurrentDown = m_DownBehaviors[m_DownPhase];

        m_DownHp = m_BossParamSet.DownHp;
        m_HackingSuccessCount = 0;

        transform.position = new Vector3(0, 0, 1);

        RequestChangeState(E_PHASE.ATTACK);
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

    #region Attack State

    private void StartOnAttack()
    {
        StartOutRingAnimation();
        GetCollider().SetEnableCollider(m_DamageCollider, true);
        m_CurrentAttack?.OnStart();
    }

    private void UpdateOnAttack()
    {
        m_CurrentAttack?.OnUpdate();
    }

    private void LateUpdateOnAttack()
    {
        m_CurrentAttack?.OnLateUpdate();
    }

    private void FixedUpdateOnAttack()
    {
        m_CurrentAttack?.OnFixedUpdate();
    }

    private void EndOnAttack()
    {
        m_CurrentAttack?.OnEnd();
    }

    #endregion

    #region Down State

    private void StartOnDown()
    {
        GetCollider().SetEnableCollider(m_DamageCollider, false);
        BattleRealBulletManager.Instance.CheckPoolAllEnemyBullet();
        AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Down");

        var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 5);
        timer.SetTimeoutCallBack(() =>
        {
            timer = null;
            RequestChangeState(E_PHASE.ATTACK);
        });
        RegistTimer(DOWN_KEY, timer);

        m_CurrentDown?.OnStart();
    }

    private void UpdateOnDown()
    {
        m_CurrentDown?.OnUpdate();
    }

    private void LateUpdateOnDown()
    {
        m_CurrentDown?.OnLateUpdate();
    }

    private void FixedUpdateOnDown()
    {
        m_CurrentDown?.OnFixedUpdate();
    }

    private void EndOnDown()
    {
        m_CurrentDown?.OnEnd();
    }

    #endregion

    #region Hacking Success State

    private void StartOnHackingSuccess()
    {
        if (m_HackingSuccessCount >= m_BossParamSet.HackingCompleteNum)
        {
            RequestChangeState(E_PHASE.RESCUE);
            return;
        }

        m_HackingSuccessCount++;
        DestroyTimer(DOWN_KEY);
        var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 5);
        timer.SetTimeoutCallBack(() =>
        {
            DestroyTimer(HACKING_SUCCESS_KEY);
            RequestChangeState(E_PHASE.CHANGE_ATTACK);
        });
        RegistTimer(HACKING_SUCCESS_KEY, timer);

        BattleRealItemManager.Instance.CreateItem(transform.position, m_BossParamSet.ItemParam);
    }

    private void UpdateOnHackingSuccess()
    {

    }

    private void LateUpdateOnHackingSuccess()
    {

    }

    private void FixedUpdateOnHackingSuccess()
    {

    }

    private void EndOnHackingSuccess()
    {

    }

    #endregion

    #region Change Attack State

    private void StartOnChangeAttack()
    {
        m_AttackPhase = Mathf.Min(m_AttackPhase + 1, m_AttackBehaviors.Count - 1);
        m_CurrentAttack = m_AttackBehaviors[m_AttackPhase];
        m_DownHp = m_BossParamSet.DownHp;
        RequestChangeState(E_PHASE.ATTACK);
    }

    private void UpdateOnChangeAttack()
    {

    }

    private void LateUpdateOnChangeAttack()
    {

    }

    private void FixedUpdateOnChangeAttack()
    {

    }

    private void EndOnChangeAttack()
    {

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

    #region Rescue State

    private void StartOnRescue()
    {
        BattleManager.Instance.GameClear();
    }

    private void UpdateOnRescue()
    {

    }

    private void LateUpdateOnRescue()
    {

    }

    private void FixedUpdateOnRescue()
    {

    }

    private void EndOnRescue()
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

    protected override void OnEnterSufferBullet(HitSufferData<BulletController> sufferData)
    {
        base.OnEnterSufferBullet(sufferData);

        var colliderType = sufferData.SufferCollider.Transform.ColliderType;
        if (colliderType == E_COLLIDER_TYPE.CRITICAL)
        {
            var currentState = m_StateMachine.CurrentState.Key;
            m_DownHp -= 1;
            if (m_DownHp <= 0 && currentState == E_PHASE.ATTACK)
            {
                m_DownHp = m_BossParamSet.DownHp;
                RequestChangeState(E_PHASE.DOWN);
            }
        }
    }

    protected override void OnEnterSufferChara(HitSufferData<CharaController> sufferData)
    {
        base.OnEnterSufferChara(sufferData);

        var sufferType = sufferData.SufferCollider.Transform.ColliderType;
        switch (sufferType)
        {
            case E_COLLIDER_TYPE.CRITICAL:
                break;
            case E_COLLIDER_TYPE.ENEMY_HACKING:
                var currentState = m_StateMachine.CurrentState.Key;
                if (currentState == E_PHASE.DOWN)
                {
                    var colliderType = sufferData.HitCollider.Transform.ColliderType;
                    if (colliderType == E_COLLIDER_TYPE.PLAYER_HACKING)
                    {
                        BattleHackingManager.Instance.SetHackingLevel(m_AttackPhase);
                        BattleManager.Instance.RequestChangeState(E_BATTLE_STATE.TRANSITION_TO_HACKING);
                    }
                }
                break;
        }
    }

    public override void Dead()
    {
        base.Dead();
        BattleManager.Instance.GameClear();
    }

    private void OnTransitionToReal()
    {
        var currentState = m_StateMachine.CurrentState.Key;
        if (currentState == E_PHASE.DOWN)
        {
            if (BattleHackingManager.Instance.IsHackingSuccess)
            {
                RequestChangeState(E_PHASE.HACKING_SUCCESS);
            }
            else
            {
                RequestChangeState(E_PHASE.ATTACK);
            }
        }
    }
}