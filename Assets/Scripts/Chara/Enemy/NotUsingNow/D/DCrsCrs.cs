﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DCrsCrs : System.Object
{

    [SerializeField, Tooltip("直線軌道の交差弾の弾幕1")]
    private UDOmn1 uDCrs1;

    [SerializeField, Tooltip("直線軌道の交差弾の弾幕2")]
    private UDOmn1 uDCrs2;


    public void Updates(BattleRealEnemyBase enemyController, float time)
    {
        uDCrs1.Updates(enemyController, time);
        uDCrs2.Updates(enemyController, time);
    }
}