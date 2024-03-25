/*-------------------------------------------------
* MushroomScript.cs
* 
* 作成日　2024/03/14
* 更新日　2024/03/14
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// マッシュルームクラス
/// </summary>
public class MushroomScript : EnemyControlScript
{
    /// <summary>
    /// 敵を制御する処理
    /// </summary>
    protected override void EnemyControl()
    {
        // 重力
        FallInGravity();
    }
}