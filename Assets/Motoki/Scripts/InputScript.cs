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
/// 入力クラス
/// </summary>
public abstract class InputScript : MonoBehaviour 
{
    /// <summary>
    /// 移動入力判定処理
    /// </summary>
    /// <returns>Vector2の入力情報</returns>
    public abstract Vector2 InputMove();

    /// <summary>
    /// ジャンプ入力判定処理
    /// </summary>
    /// <returns>入力判定</returns>
    public abstract bool IsJumpButtonDown();
}