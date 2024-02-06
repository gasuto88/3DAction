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
    /// Spaceキーの入力判定処理
    /// </summary>
    /// <returns>入力判定</returns>
	public bool IsSpace()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return true;
        }
        return false;
    }
}