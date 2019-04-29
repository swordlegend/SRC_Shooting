﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherController : PlayerController
{
	[SerializeField, Range( 0f, 1f )]
	private float m_ShotInterval;

	private float initialShotInterval;

	[SerializeField]
	private float m_ShotIntervalDecrease;

	private float shotDelay;

	[SerializeField]
	private Transform[] m_MainShotPosition;

	[SerializeField]
	private Transform[] m_SubShotLv1Position;

	private bool m_SubShotLv1CanShot;

	[SerializeField]
	private Transform[] m_SubShotLv2Position;

	private bool m_SubShotLv2CanShot;

	[SerializeField]
	private float m_SubShotLv2Radius;

	[SerializeField]
	private float m_SubShotLv2Speed;

	private float m_SubShotLv2AngleDeg;

	[SerializeField]
	private bool m_IsSpinTurn;


	protected override void Awake()
	{
		initialShotInterval = m_ShotInterval;
		OnAwake();
	}

	public override void OnUpdate()
	{
		shotDelay += Time.deltaTime;
		base.OnUpdate();
		UpdateShotLevel( GetLevel() );
		UpdateSubShot();
	}

	public override void ShotBullet()
	{
		if( shotDelay >= m_ShotInterval )
		{
			for( int i = 0; i < m_MainShotPosition.Length; i++ )
			{
				var shotParam = new BulletShotParam( this );
				shotParam.Position = m_MainShotPosition[i].transform.position - transform.parent.position;
                shotParam.OrbitalIndex = 0;
				BulletController.ShotBullet( shotParam );
			}

			if( m_SubShotLv1CanShot )
			{
				for( int i = 0; i < m_SubShotLv1Position.Length; i++ )
				{
					var shotParam = new BulletShotParam( this );
					shotParam.Position = m_SubShotLv1Position[i].transform.position - transform.parent.position;
                    shotParam.OrbitalIndex = 0;
					BulletController.ShotBullet( shotParam );
				}
			}

			if( m_SubShotLv2CanShot )
			{
				for( int i = 0; i < m_SubShotLv2Position.Length; i++ )
				{
					var shotParam = new BulletShotParam( this );
                    shotParam.Position = m_SubShotLv2Position[i].transform.position - transform.parent.position;
					shotParam.BulletIndex = 1;
					shotParam.OrbitalIndex = 1;
					BulletController.ShotBullet( shotParam );
				}

			}

			shotDelay = 0;
		}
	}

	public void UpdateShotLevel( int level )
	{
		if( level >= 3 )
		{
			m_SubShotLv1CanShot = true;
			m_SubShotLv2CanShot = true;
			m_ShotInterval = initialShotInterval - m_ShotIntervalDecrease * 2;
		}
		else if( level >= 2 )
		{
			m_SubShotLv2CanShot = false;
			m_SubShotLv1CanShot = true;
			m_ShotInterval = initialShotInterval - m_ShotIntervalDecrease * 2;
		}
		else
		{
			m_SubShotLv1CanShot = false;
			m_SubShotLv2CanShot = false;
			m_ShotInterval = initialShotInterval;
		}

		for( int i = 0; i < m_SubShotLv1Position.Length; i++ )
		{
			m_SubShotLv1Position[i].gameObject.SetActive( m_SubShotLv1CanShot );
		}

		for( int i = 0; i < m_SubShotLv2Position.Length; i++ )
		{
			m_SubShotLv2Position[i].gameObject.SetActive( m_SubShotLv2CanShot );
		}
	}




	private void UpdateSubShot()
	{
		m_SubShotLv2AngleDeg += m_SubShotLv2Speed * Time.deltaTime;
		m_SubShotLv2AngleDeg %= Mathf.PI * 2;
		float unitAngle = Mathf.PI * 2 / m_SubShotLv2Position.Length;

		for( int i = 0; i < m_SubShotLv2Position.Length; i++ )
		{
			float angle = unitAngle * i + m_SubShotLv2AngleDeg;

			if( m_IsSpinTurn )
			{
				angle *= ( i % 2 == 0 ? -1 : 1 );
			}

			float x = m_SubShotLv2Radius * Mathf.Cos( -angle );
			float z = m_SubShotLv2Radius * Mathf.Sin( -angle );

			m_SubShotLv2Position[i].GetComponent<Transform>().localPosition = new Vector3( x, 0, z );
			m_SubShotLv2Position[i].GetComponent<Transform>().LookAt( transform );
		}
	}
}