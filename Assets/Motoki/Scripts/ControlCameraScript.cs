/*-------------------------------------------------
* ControlCameraScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/05
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// カメラを制御するクラス
/// </summary>
public class ControlCameraScript : MonoBehaviour 
{

	#region フィールド変数

	// 自分のTransform
	private Transform _myTransform = default;

	// プレイヤーのTransform
	private Transform _player = default;

	private Vector3 _offset = default;

	#endregion

	/// <summary>
    /// 更新前処理
    /// </summary>
	private void Start () 
	{
		// 自分のTransformを設定
		_myTransform = transform;

		_player = GameObject.FindGameObjectWithTag("Player").transform;

		_offset = _myTransform.position - _player.position;
	}
	
	/// <summary>
    /// 更新処理
    /// </summary>
	private void Update () 
	{
		CameraControl();
	}

	/// <summary>
	/// カメラを制御する処理
	/// </summary>
	private void CameraControl()
    {
		//_offset = _player.position - _myTransform.position;
		Vector3 cameraPosition = _player.position + _offset;

		// 座標を設定
		_myTransform.position = cameraPosition;

		_myTransform.LookAt(_player);
    }
}