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
    /// 上入力判定処理
    /// </summary>
    /// <returns>入力判定</returns>
	public bool IsUp()
    {
        if (Input.GetKey(KeyCode.W))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 下入力判定処理
    /// </summary>
    /// <returns>入力判定</returns>
	public bool IsDown()
    {
        if (Input.GetKey(KeyCode.S))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 右入力判定処理
    /// </summary>
    /// <returns>入力判定</returns>
	public bool IsRight()
    {
        if (Input.GetKey(KeyCode.D))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 左入力判定処理
    /// </summary>
    /// <returns>入力判定</returns>
	public bool IsLeft()
    {
        if (Input.GetKey(KeyCode.A))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// ジャンプの入力判定処理
    /// </summary>
    /// <returns>入力判定</returns>
	public bool IsJump()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetButton(JUMP))
        {
            return true;
        }
        return false;
    }
}