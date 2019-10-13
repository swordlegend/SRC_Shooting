﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRealUiManager : ControllableMonoBehavior
{

    [SerializeField]
    private CanvasGroup m_CanvasGroup;

    public void SetAlpha(float normalizedAlpha)
    {
        m_CanvasGroup.alpha = normalizedAlpha;
    }
}