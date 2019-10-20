﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// リアルモードのプレイヤーの残機を表示する
/// (今のところは数字で)
/// </summary>
public class RealLastIndicator : MonoBehaviour
{
    [SerializeField]
    private Text m_OutText;

    private IntReactiveProperty m_Last;

    // Start is called before the first frame update
    void Start()
    {
        if(BattleRealPlayerManager.Instance != null){
            RegisterScore();
        }else{
            BattleRealPlayerManager.OnStartAction += RegisterScore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleRealManager.Instance == null){
            return;
        }
    }

    private void RegisterScore(){
        m_Last = new IntReactiveProperty(0);
        m_Last.SubscribeToText(m_OutText);
    }
}
