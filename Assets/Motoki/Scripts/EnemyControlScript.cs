/*-------------------------------------------------
* EnemyControlScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/29
*
* 作成者　本木大地
-------------------------------------------------*/

using UnityEngine;

public class EnemyControlScript : CharacterControlScript
{

    #region 定数

    #endregion

    #region フィールド変数

    #endregion

    #region 定数
    #endregion

    public override void CharacterControl()
    {
        base.CharacterControl();

        FallInGravity();
    }

}