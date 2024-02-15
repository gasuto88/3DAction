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

	private ControlPlayerScript _controlPlayerScript = default;

	private GravityScript _gravityScript = default;

	private ControlCameraScript _controlCameraScript = default;

	#endregion

	/// <summary>
    /// 更新前処理
    /// </summary>
	private void Start () 
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		_controlPlayerScript = player.GetComponent<ControlPlayerScript>();

		_gravityScript = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityScript>();

		_controlCameraScript 
			= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ControlCameraScript>();
	}
	
	/// <summary>
    /// 更新処理
    /// </summary>
	private void Update () 
	{
		_controlPlayerScript.ControlPlayer();
		_gravityScript.SetNearPlanet();
		_controlCameraScript.ControlCamera();
		
	}
}