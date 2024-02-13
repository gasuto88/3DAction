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

	private MoveScript _moveScript = default;

	private JumpScript _jumpScript = default;

	private GravityScript _gravityScript = default;

	private SelectPlanetScript _selectPlanetScript = default;

	private ControlCameraScript _controlCameraScript = default;

	#endregion

	/// <summary>
    /// 更新前処理
    /// </summary>
	private void Start () 
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		_moveScript = player.GetComponent<MoveScript>();
		_jumpScript = player.GetComponent<JumpScript>();
		_gravityScript = player.GetComponent<GravityScript>();
		_controlCameraScript 
			= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ControlCameraScript>();

		_selectPlanetScript = GetComponent<SelectPlanetScript>();

	}
	
	/// <summary>
    /// 更新処理
    /// </summary>
	private void Update () 
	{
		_moveScript.Move();
		_jumpScript.Jump();
		_gravityScript.Gravity();
		_gravityScript.RotateGravity();
		_gravityScript.SelectPlanet();
		_controlCameraScript.ControlCamera();
		
	}
}