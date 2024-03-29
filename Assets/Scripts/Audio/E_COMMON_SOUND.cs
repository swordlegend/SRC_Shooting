﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 汎用音の列挙型
/// </summary>
public enum E_COMMON_SOUND
{
    /// <summary>
    /// 決定SE
    /// </summary>
    SYSTEM_OK,

    /// <summary>
    /// フォーカス移動SE
    /// </summary>
    SYSTEM_CURSOR,

    /// <summary>
    /// キャンセルSE
    /// </summary>
    SYSTEM_CANCEL,

    /// <summary>
    /// リザルトUIのスライドSE
    /// </summary>
    SYSTEM_SLIDE,

    /// <summary>
    /// シューティングパート開始ボタンSE
    /// </summary>
    SYSTEM_START,

    /// <summary>
    /// ハッキングモードへ遷移する時のSE
    /// </summary>
    HACK_START,

    /// <summary>
    /// ハッキングモードから戻ってくる時のSE
    /// </summary>
    HACK_END,

    /// <summary>
    /// プレイヤーのエナジーストックが1つ増えた時のSE
    /// </summary>
    PLAYER_POWER_UP,

    /// <summary>
    /// ステージのゲームクリアBGM
    /// </summary>
    GAME_CLEAR,

    /// <summary>
    /// ゲームオーバーBGM
    /// </summary>
    GAME_OVER,

    /// <summary>
    /// 敵のダメージSE
    /// </summary>
    ENEMY_DAMAGE_01,

    /// <summary>
    /// リアルモードのボスのダウンSE
    /// </summary>
    BOSS_DOWN,

    /// <summary>
    /// リアルモードのボスのダウン復帰SE
    /// </summary>
    BOSS_DOWN_RETURN,

    /// <summary>
    /// プレイヤー通常弾SE その1
    /// </summary>
    PLAYER_SHOT_01,

    /// <summary>
    /// ハッキングのプレイヤー通常弾SE
    /// </summary>
    PLAYER_HACKING_SHOT,

    /// <summary>
    /// プレイヤーチャージSE その1
    /// </summary>
    PLAYER_CHARGE_01,

    /// <summary>
    /// プレイヤーレーザーSE
    /// </summary>
    PLAYER_LASER,

    /// <summary>
    /// プレイヤーボムSE
    /// </summary>
    PLAYER_BOMB,

    /// <summary>
    /// プレイヤー武器タイプ切替SE
    /// </summary>
    PLAYER_WEAPON_CHANGE,

    /// <summary>
    /// アイテム獲得SE
    /// </summary>
    PLAYER_GET_ITEM,

    /// <summary>
    /// 敵の中級発射SE その1
    /// </summary>
    ENEMY_SHOT_MEDIUM_01,

    /// <summary>
    /// 敵の中級発射SE その2
    /// </summary>
    ENEMY_SHOT_MEDIUM_02,

    /// <summary>
    /// ボスのチャージ時のSE
    /// </summary>
    BOSS_CHARGE,

    /// <summary>
    /// ハッキングが成功した時のSE
    /// </summary>
    BOSS_HACK_SUCCESS,

    /// <summary>
    /// ハッキングが失敗した時のSE
    /// </summary>
    BOSS_HACK_FAILURE,

    /// <summary>
    /// ハッキングのボスを倒した時のSE
    /// </summary>
    HACKING_BOSS_BREAK,

    /// <summary>
    /// インゲームのメニューを開いた時のSE
    /// </summary>
    INGAME_MENU_OPEN,

    /// <summary>
    /// インゲームのメニューを閉じた時のSE
    /// </summary>
    INGAME_MENU_CLOSE,

    /// <summary>
    /// 敵の発射SE その1
    /// </summary>
    ENEMY_HACK_SHOT_SMALL_01,

    /// <summary>
    /// 敵の中級発射SE その1
    /// </summary>
    ENEMY_HACK_SHOT_MEDIUM_01,

    /// <summary>
    /// 敵の中級発射SE その2
    /// </summary>
    ENEMY_HACK_SHOT_MEDIUM_02,

    /// <summary>
    /// チャージショットを放った瞬間のSE
    /// </summary>
    PLAYER_CHARGE_SHOT,

    /// <summary>
    /// プレイヤーが最大レベルに到達した時のSE
    /// </summary>
    PLAYER_LEVEL_MAX,
}
