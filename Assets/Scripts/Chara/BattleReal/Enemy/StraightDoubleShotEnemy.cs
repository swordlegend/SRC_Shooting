﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 直線移動する敵のコントローラ。
/// </summary>
public class StraightDoubleShotEnemy : StraightEnemy
{
	[Header( "Double Shot Param" )]

	// 二発目を発射するまでの間隔
	[SerializeField]
	private float m_DoubleShotInterval;

	public override void SetArguments( string param )
	{
		base.SetArguments( param );

		m_DoubleShotInterval = m_ParamSet.FloatParam["DSI"];
	}

	protected override void OnShot( EnemyShotParam param, bool isLookPlayer = true )
	{
		// 最初
		int num = param.Num;
		float angle = param.Angle;
		var spreadAngles = GetBulletSpreadAngles( num, angle );
		var shotParam = new BulletShotParam( this );

		for( int i = 0; i < num; i++ )
		{
			var bullet = BulletController.ShotBullet( shotParam );
			bullet.SetRotation( new Vector3( 0, spreadAngles[i], 0 ), E_RELATIVE.RELATIVE );
		}

		var timer = Timer.CreateTimer( E_TIMER_TYPE.SCALED_TIMER, 0, m_DoubleShotInterval );
		timer.SetTimeoutCallBack( () =>
		{
			// 二回目
			num = param.Num;
			angle = param.Angle;
			spreadAngles = GetBulletSpreadAngles( num, angle );
			shotParam = new BulletShotParam( this );
			shotParam.OrbitalIndex = 0;

			for( int i = 0; i < num; i++ )
			{
				var bullet = BulletController.ShotBullet( shotParam );
				bullet.SetRotation( new Vector3( 0, spreadAngles[i], 0 ), E_RELATIVE.RELATIVE );
			}
		} );

        RegistTimer("Double", timer);
    }
}
