/*-------------------------------------------------
* GameManagerScript.cs
* 
* 作成日　2024/02/13
* 更新日　2024/02/13
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

public class GameManagerScript : MonoBehaviour 
{

    #region フィールド変数

    private CharacterControlScript[] _characterControlScripts = default;

    private StarScript _starScript = default;

	#endregion

	/// <summary>
    /// 更新前処理
    /// </summary>
	private void Start () 
	{
        _characterControlScripts = GameObject.FindObjectsOfType<CharacterControlScript>();

        _starScript = GameObject.FindGameObjectWithTag("Star").GetComponent<StarScript>();
    }
	
	/// <summary>
    /// 更新処理
    /// </summary>
	private void Update () 
	{
        foreach (CharacterControlScript script in _characterControlScripts)
        {
            script.CharacterControl();
        }

        _starScript.Rotate();
	}


}