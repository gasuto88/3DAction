/*-------------------------------------------------
* InputScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/05
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// 入力判定クラス
/// </summary>
public class InputScript : MonoBehaviour 
{
    #region 定数

    private const string HORIZONTAL = "Horizontal";

    private const string VERTICAL = "Vertical";

    private const string JUMP = "Jump";

    #endregion

    #region フィールド変数

    [SerializeField,Header("入力のデッドゾーン"),Range(0,1)]
    private float _deadZoon = 0f;

    #endregion

    /// <summary>
    /// 移動の入力
    /// </summary>
    /// <returns></returns>
    public Vector2 InputMove()
    {
        // 入力取得
        float inputX = Input.GetAxisRaw(HORIZONTAL);
        float inputY = Input.GetAxisRaw(VERTICAL);

        // デッドゾーンだったら
        if (inputX <= _deadZoon && -_deadZoon <= inputX)
        {
            inputX = 0f;
        }
        if(inputY <= _deadZoon && -_deadZoon <= inputY)
        {
            inputY = 0f;
        }

        // 入力方向を設定
        Vector2 input = Vector2.up * inputX + Vector2.right * inputY;

        return input;
    }

    /// <summary>
    /// ジャンプボタンを押した判定
    /// </summary>
    /// <returns>入力判定</returns>
	public bool IsJumpButtonDown()
    {
        if (Input.GetButtonDown(JUMP))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// ジャンプボタンを離した判定
    /// </summary>
    /// <returns>入力判定</returns>
    public bool IsJumpButtonUp()
    {
        if (Input.GetButtonUp(JUMP))
        {
            return true;
        }
        return false;
    }
}