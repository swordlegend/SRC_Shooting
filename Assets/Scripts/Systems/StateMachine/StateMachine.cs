﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ステートマシン。
/// </summary>
public class StateMachine<T>
{
    private Dictionary<T, State<T>> m_States;

    public State<T> CurrentState { get; private set; }
    private State<T> m_PreState;
    private State<T> m_NextState;

    public StateMachine()
    {
        m_States = new Dictionary<T, State<T>>();
        CurrentState = null;
        m_PreState = null;
        m_NextState = null;
    }

    public void OnFinalize()
    {
        foreach (var state in m_States.Values)
        {
            state.OnFinalize();
        }

        m_States.Clear();
        m_States = null;
    }

    public void OnUpdate()
    {
        if (CurrentState != m_NextState)
        {
            ProcessChangeState();
        }

        if (CurrentState != null)
        {
            EventUtility.SafeInvokeAction(CurrentState.OnUpdate);
        }
    }

    public void OnLateUpdate()
    {
        if (CurrentState != null)
        {
            EventUtility.SafeInvokeAction(CurrentState.OnLateUpdate);
        }
    }

    public void OnFixedUpdate()
    {
        if (CurrentState != null)
        {
            EventUtility.SafeInvokeAction(CurrentState.OnFixedUpdate);
        }
    }

    public void AddState(State<T> state)
    {
        if (state == null)
        {
            return;
        }

        m_States.Add(state.Key, state);
    }

    public void Goto(T key)
    {
        if (m_States == null)
        {
            return;
        }

        if (!m_States.ContainsKey(key))
        {
            return;
        }

        m_NextState = m_States[key];
    }

    private void ProcessChangeState()
    {
        if (CurrentState != null)
        {
            m_PreState = CurrentState;
            EventUtility.SafeInvokeAction(m_PreState.OnEnd);
        }

        if (m_NextState != null)
        {
            CurrentState = m_NextState;
            EventUtility.SafeInvokeAction(CurrentState.OnStart);
        }
    }
}