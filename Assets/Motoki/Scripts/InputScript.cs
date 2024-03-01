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


    #endregion

    #region フィールド変数

    #endregion

    /// <summary>
    /// 移動の入力
    /// </summary>
    /// <returns></returns>
    public virtual Vector2 InputMove()
    {
        return default;
    }

    /// <summary>
    /// ジャンプボタンを押した判定
    /// </summary>
    /// <returns>入力判定</returns>
	public virtual bool IsJumpButtonDown()
    {
        return false;
    }

    /// <summary>
    /// ジャンプボタンを離した判定
    /// </summary>
    /// <returns>入力判定</returns>
    public bool IsJumpButtonUp()
    {
        return false;
    }
}