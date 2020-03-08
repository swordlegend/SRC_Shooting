﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾を発射する時のパラメータの変数を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/shotParam/variable", fileName = "ShotParamOperationVariable", order = 0)]
[System.Serializable]
public class ShotParamOperationVariable : ScriptableObject
{

    /// <summary>
    /// 弾の見た目の種類
    /// </summary>
    //[UnityEngine.Serialization.FormerlySerializedAs("BulletIndex")]
    [SerializeField]
    private OperationIntVariable m_BulletIndex;
    public OperationIntVariable BulletIndex
    {
        set { m_BulletIndex = value; }
        get { return m_BulletIndex; }
    }

    /// <summary>
    /// 発射位置
    /// </summary>
    [UnityEngine.Serialization.FormerlySerializedAs("Position")]
    [SerializeField]
    private OperationVector2Variable m_Position;
    public OperationVector2Variable Position
    {
        set { m_Position = value; }
        get { return m_Position; }
    }

    /// <summary>
    /// 発射角度
    /// </summary>
    [UnityEngine.Serialization.FormerlySerializedAs("Angle")]
    [SerializeField]
    private OperationFloatVariable m_Angle;
    public OperationFloatVariable Angle
    {
        set { m_Angle = value; }
        get { return m_Angle; }
    }

    /// <summary>
    /// 大きさ
    /// </summary>
    [UnityEngine.Serialization.FormerlySerializedAs("Scale")]
    [SerializeField]
    private OperationFloatVariable m_Scale;
    public OperationFloatVariable Scale
    {
        set { m_Scale = value; }
        get { return m_Scale; }
    }

    /// <summary>
    /// 速度ベクトル
    /// </summary>
    [UnityEngine.Serialization.FormerlySerializedAs("Velocity")]
    [SerializeField]
    private OperationVector2Variable m_Velocity;
    public OperationVector2Variable Velocity
    {
        set { m_Velocity = value; }
        get { return m_Velocity; }
    }

    /// <summary>
    /// 回転速度
    /// </summary>
    [UnityEngine.Serialization.FormerlySerializedAs("AngleSpeed")]
    [SerializeField]
    private OperationFloatVariable m_AngleSpeed;
    public OperationFloatVariable AngleSpeed
    {
        set { m_AngleSpeed = value; }
        get { return m_AngleSpeed; }
    }

    /// <summary>
    /// 大きさの変化速度
    /// </summary>
    [UnityEngine.Serialization.FormerlySerializedAs("ScaleSpeed")]
    [SerializeField]
    private OperationFloatVariable m_ScaleSpeed;
    public OperationFloatVariable ScaleSpeed
    {
        set { m_ScaleSpeed = value; }
        get { return m_ScaleSpeed; }
    }


    /// <summary>
    /// 具体的な発射パラメータの値をセットする。
    /// </summary>
    public void SetValue(ShotParam shotParam)
    {
        BulletIndex.Value = shotParam.BulletIndex;
        Position.Value = shotParam.Position;
        Angle.Value = shotParam.Angle;
        Scale.Value = shotParam.Scale;
        Velocity.Value = shotParam.Velocity;
        AngleSpeed.Value = shotParam.AngleSpeed;
        ScaleSpeed.Value = shotParam.ScaleSpeed;
    }
}





//public ShotParam(int bulletIndex, OperationVector2Base position, float velocityRad, float speed)
//{
//    BulletIndex = bulletIndex;
//    Position = position;
//    Angle = velocityRad;
//    Speed = speed;
//}


//public ShotParam() : this(0, new OperationVector2Init(Vector2.zero), 0, 0)
//{

//}


//public ShotParam(ShotParam shotParam) : this(shotParam.BulletIndex, new OperationVector2Init(shotParam.Position.GetResult()), shotParam.Angle, shotParam.Speed)
//{

//}


///// <summary>
///// 初速度の大きさ
///// </summary>
//[SerializeField]
//public OperationFloatVariable Speed;