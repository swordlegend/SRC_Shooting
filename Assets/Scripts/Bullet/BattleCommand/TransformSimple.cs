﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の物理的な状態を表すクラス。
/// </summary>
public class TransformSimple : object
{
    /// <summary>
    /// 位置
    /// </summary>
    public Vector2 Position { get; private set; }

    /// <summary>
    /// 回転角度
    /// </summary>
    public float Angle { get; private set; }

    /// <summary>
    /// 大きさ
    /// </summary>
    public float Scale { get; private set; }

    /// <summary>
    /// 不透明度
    /// </summary>
    public float Opacity { get; private set; }

    /// <summary>
    /// 衝突判定があるかどうか
    /// </summary>
    public bool CanCollide { get; private set; }


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public TransformSimple(
        Vector2 position,
        float angle,
        float scale,
        float opacity,
        bool canCollide
        )
    {
        Position = position;
        Angle = angle;
        Scale = scale;
        Opacity = opacity;
        CanCollide = canCollide;
    }
}





///// <summary>
///// コンストラクタ（クローン用）
///// </summary>
//public TransformSimple(TransformSimple transform) : this(transform.m_Position, transform.m_Angle, transform.m_Scale)
//{

//}


//public override string ToString()
//{
//    return m_Position.ToString() + ", " + m_Angle.ToString() + ", " + m_Scale.ToString();
//}
