/*-------------------------------------------------
* EnemyInputScript.cs
* 
* 作成日　2024/03/06
* 更新日　2024/03/06
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

public class EnemyInputScript : InputScript
{

    #region フィールド変数
    #endregion

    public override Vector2 InputMove()
    {
        return Vector2.zero;
    }

    public override bool IsJumpButtonDown()
    {
        return false;
    }
}