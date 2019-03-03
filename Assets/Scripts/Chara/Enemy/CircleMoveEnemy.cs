﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 円運動をしながら定期的に弾を撃ってくる敵。
/// </summary>
public class CircleMoveEnemy : EnemyController
{

	[SerializeField]
	private Material m_Normal;

	[SerializeField]
	private Material m_HitMate;

	[SerializeField]
	private Renderer m_Renderer;

	[SerializeField]
	private Vector3 m_BasePos;

	[SerializeField]
	private float m_Radius;

	[SerializeField]
	private float m_AngleSpeed;

	[SerializeField]
	private float m_ShotInterval;

	[SerializeField]
	private float m_ShotPosOffset;

	[SerializeField]
	private float m_BulletSpeed = 10;

	[SerializeField]
	private float m_DeadDistance = 500;

	private float m_NowRad;

	private float m_ShotTime;

	protected override void Awake()
	{
        base.Awake();
		m_ShotTime = 0;
	}

	public override void OnUpdate()
	{
		m_NowRad += m_AngleSpeed * Time.deltaTime;
		m_NowRad %= Mathf.PI * 2;

		Vector3 pos = new Vector3( Mathf.Cos( m_NowRad ), 0, Mathf.Sin( m_NowRad ) ) * m_Radius + m_BasePos;
		transform.position = pos;

		Vector3 angle = transform.eulerAngles;
		angle.y = -( m_NowRad * Mathf.Rad2Deg ) + 90;
		transform.eulerAngles = angle;

		if( m_ShotTime < 0f )
		{
			m_ShotTime = m_ShotInterval;
			ShotBullet( 0, 0 );
		}
		else
		{
			m_ShotTime -= Time.deltaTime;
		}

		m_Renderer.material = m_Normal;
	}

	public override void OnSuffer( Bullet bullet, CollisionManager.ColliderData colliderData )
	{
		base.OnSuffer( bullet, colliderData );
		m_Renderer.material = m_HitMate;
		//Debug.LogError( 1111111 );
	}
}
