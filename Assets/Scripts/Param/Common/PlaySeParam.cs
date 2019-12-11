﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// SE再生用のパラメータ
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sound/Play SE", fileName = "param.play_se.asset")]
public class PlaySeParam : ScriptableObject
{
    [SerializeField]
    private E_CUE_SHEET m_CueSheet;
    public E_CUE_SHEET CueSheet => m_CueSheet;

    [SerializeField]
    private string m_SeName;
    public string SeName => m_SeName;
}