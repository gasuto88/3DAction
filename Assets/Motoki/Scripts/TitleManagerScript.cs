/*-------------------------------------------------
* TitleManagerScript.cs
* 
* 作成日　2024/03/15
* 更新日　2024/03/15
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManagerScript : MonoBehaviour 
{

    #region 定数

    private const string JUMP_BUTTON_NAME = "Jump";

    #endregion

    #region フィールド変数

    [SerializeField,Header("遷移シーン")]
    private string _sceneName = default;

    #endregion
	
	/// <summary>
    /// 更新処理
    /// </summary>
	private void Update () 
	{
        if (IsStartButton())
        {
            SceneManager.LoadScene(_sceneName);
        }
	}

    /// <summary>
    /// 開始入力判定
    /// </summary>
    /// <returns>入力判定</returns>
	private bool IsStartButton()
    {
        if (Input.GetButtonDown(JUMP_BUTTON_NAME))
        {
            return true;
        }

        return false;
    }
}