/*-------------------------------------------------
* CameraControlScript.cs
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
public class CameraControlScript : MonoBehaviour 
{

	#region 定数

	private const string PLANET = "Planet";

    #endregion

    #region フィールド変数

	// 自分のTransform
	private Transform _myTransform = default;

	// プレイヤーのTransform
	private Transform _player = default;

	private Vector3 _offset = default;

	private Vector3 _cameraPosition = default;

    #endregion

    #region プロパティ

	public Transform CameraTransform { get => _myTransform; set => _myTransform = value;}

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

		_cameraPosition = _player.position + _offset;

		_myTransform.position = _cameraPosition;
	}

	/// <summary>
	/// カメラを制御する処理
	/// </summary>
	public void ControlCamera()
    {
		_myTransform.LookAt(_player);
    }
}