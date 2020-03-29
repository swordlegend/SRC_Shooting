﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主に、BattleRealEnemyBehaviorUnitにおいて割り込みで強制終了したい時に追加するもの。
/// </summary>
public abstract class BattleRealEnemyBehaviorInterruptEndFunc : ScriptableObject
{
    public abstract bool IsInterruptEnd(BattleRealEnemyBase enemy, BattleRealEnemyBehaviorUnit unit);
}