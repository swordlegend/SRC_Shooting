﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniInheritance : DanmakuAbstract
{

    // 発射間隔
    [SerializeField]
    private float m_ShotInterval;

    // way数
    [SerializeField]
    private int m_Way;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed = 10;


    // 本体の位置とオイラー角を更新する
    protected override void UpdateSelf()
    {

    }


    // 現在のあるべき発射回数を計算する(小数)
    protected override float CalcNowShotNum()
    {
        return Time.time / m_ShotInterval;
    }


    // 発射時刻を計算する
    protected override float CalcLaunchTime()
    {
        return m_ShotInterval * realShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected override void ShotBullets(float launchTime, float dTime)
    {

        float distance = m_BulletSpeed * dTime;

        // ランダムな角度
        float rad0 = Random.Range(0, Mathf.PI * 2);

        for (int i = 0; i < m_Way; i++)
        {
            // 1つの弾の角度
            float rad = rad0 + Mathf.PI * 2 * i / m_Way;

            // その弾の発射角度
            Vector3 eulerAngles;
            eulerAngles = CalcEulerAngles(rad);

            // 発射された弾の現在の位置
            Vector3 pos = transform.position;
            pos += new Vector3(distance * Mathf.Cos(rad), 0, distance * Mathf.Sin(rad));

            // 弾を撃つ
            BulletShotParam bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, eulerAngles, transform.localScale);
            BulletController.ShotBullet(bulletShotParam);
        }
    }
}