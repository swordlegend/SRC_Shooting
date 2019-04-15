﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Battle専用のシングルトン。
/// </summary>
public class BattleSingletonMonoBehavior<T> : BattleControllableMonoBehavior where T : MonoBehaviour
{

    public static T Instance {
        get;
        private set;
    }

    /// <summary>
    /// Unityで制御される生成直後に呼び出される処理。
    /// </summary>
    protected override void Awake()
    {
        if (CheckExistInstance())
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = GetComponent<T>();
            OnAwake();
        }
    }

    /// <summary>
    /// Unityで制御される破棄直前に呼び出される処理。
    /// </summary>
    protected override void OnDestroy()
    {
        OnDestroyed();
        Instance = null;
    }

    /// <summary>
    /// このクラスのインスタンスが存在するかどうかを取得する。
    /// </summary>
    public static bool CheckExistInstance()
    {
        return Instance;
    }
}
