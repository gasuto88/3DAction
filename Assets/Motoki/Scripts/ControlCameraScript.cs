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

	private GravityScript _gravityScript = default;

	private MoveScript _moveScript = default;

	#endregion

	/// <summary>
    /// 更新前処理
    /// </summary>
	private void Start () 
	{
		// 自分のTransformを設定
		_myTransform = transform;

		_player = GameObject.FindGameObjectWithTag("Player").transform;

		_moveScript = _player.GetComponent<MoveScript>();

		_gravityScript = _player.GetComponent<GravityScript>();

		_offset = _myTransform.position - _player.position;		
	}

	/// <summary>
	/// カメラを制御する処理
	/// </summary>
	public void ControlCamera()
    {
		Vector3 cameraPosition = _player.position;
		
		_myTransform.position = cameraPosition + _offset;


    }
}