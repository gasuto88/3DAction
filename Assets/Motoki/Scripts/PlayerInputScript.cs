/*-------------------------------------------------
* PlayerInputScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/03/01
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// プレイヤー入力クラス
/// </summary>
public class PlayerInputScript : InputScript 
{

    #region 定数

    private const string HORIZONTAL = "Horizontal";

    private const string VERTICAL = "Vertical";

    private const string JUMP = "Jump";

    #endregion

    #region フィールド変数

    [SerializeField, Header("入力のデッドゾーン"), Range(0, 1)]
    private float _deadZoon = 0f;

    #endregion

    public override Vector2 InputMove()
    {
        // 入力取得
        float inputX = Input.GetAxisRaw(HORIZONTAL);
        float inputY = Input.GetAxisRaw(VERTICAL);

        // デッドゾーンだったら
        if (inputX <= _deadZoon && -_deadZoon <= inputX)
        {
            inputX = 0f;
        }
        if (inputY <= _deadZoon && -_deadZoon <= inputY)
        {
            inputY = 0f;
        }

        // 入力方向を設定
        Vector2 input = Vector2.up * inputX + Vector2.right * inputY;

        return input;
    }

    public override bool IsJumpButtonDown()
    {
        if (Input.GetButtonDown(JUMP))
        {
            return true;
        }
        return false;
    }
}