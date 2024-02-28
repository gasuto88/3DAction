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

	#region 定数

	private const string PLANET = "Planet";

    #endregion

    #region フィールド変数

    [SerializeField,Header("カメラの回転速度"),Range(0,200)]
	private float _cameraRotationSpeed = 0f;

	[SerializeField, Header("カメラの旋回速度"), Range(0, 200)]
	private float _cameraTurningSpeed = 0f;

	[SerializeField,Header("プレイヤーの頭の座標")]
	private Transform _headTransform = default;

	[SerializeField, Header("プレイヤーの足の座標")]
	private Transform _legTransform = default;

	// 自分のTransform
	private Transform _myTransform = default;

	// プレイヤーのTransform
	private Transform _player = default;

	private Vector3 _offset = default;

	private Vector3 _cameraPosition = default;

	private bool isTurning = false;

	private GravityScript _gravityScript = default;

	private MoveScript _moveScript = default;

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

		_moveScript = _player.GetComponent<MoveScript>();

		_gravityScript
			= GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityScript>();

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