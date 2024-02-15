/*-------------------------------------------------
* MoveScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/05
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// 移動クラス
/// </summary>
public class MoveScript : MonoBehaviour 
{
	#region 定数

	// 地面
	private const string GROUND = "Ground";

	private const string RUN = "Run";

    // 半分の入力値
    private const float HALF_INPUT = 0.5f;

    #endregion

    #region フィールド変数

    [SerializeField,Header("移動速度"),Range(0,100)]
	private float _moveMaxSpeed = 0f;

	[SerializeField,Header("振り向き速度"),Range(0,500)]
	private float _rotationSpeed = 0f;

	[SerializeField, Header("加速速度"), Range(0, 100)]
	private float _accelSpeed = 0f;

	[SerializeField, Header("ブレーキ速度"), Range(0, 100)]
	private float _brakeSpeed = 0f;

	[SerializeField,Header("無入力時間"),Range(0,5)]
	private float _noInputMaxTime = 0f;

	// 移動速度
	private float _moveSpeed = 0f;

	// ブレーキ終了判定
	private bool isBrake = default;

	// 無入力時間
	private float _noInputTime = 0f;

	// 自分のTransform
	private Transform _myTransform = default;

	// 進行方向
	private Vector3 _moveVector = default; 

	private Vector3 _beforePosition = default;

	// プレイヤーの子
	private Transform _child = default;

	// 移動アニメーション
	private Animator _moveAnim = default;

	// 入力クラス
	private InputScript _inputScript = default;

	// 重力クラス
	private GravityScript _gravityScript = default;

	// 移動状態
	private MoveState _moveState = MoveState.MOVE;

	/// <summary>
	/// 移動状態
	/// </summary>
	private enum MoveState
    {
		STAY,
		MOVE,
		STOP
    }

    #endregion

    #region プロパティ

	public Vector3 MoveVector { get => _moveVector; set => _moveVector = value; }

	public Transform MyTransform { get => _myTransform; set => _myTransform = value; }

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start () 
	{
		// 自分のTransformを設定
		_myTransform = transform;

		_beforePosition = _myTransform.position;

		// 子を取得
		_child = _myTransform.GetChild(0);

		// 移動アニメーションを取得
		_moveAnim = GetComponent<Animator>();

		// InputScriptを取得
		_inputScript = GetComponent<InputScript>();

		// GravityScriptを取得
		_gravityScript
			= GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityScript>();

		_noInputTime = _noInputMaxTime;
	}

	/// <summary>
	/// 移動処理
	/// </summary>
	public void Move()
    {
		// 入力取得
		Vector2 moveInput = _inputScript.InputMove();
		
		// 入力されたら
		if (0 != moveInput.x || 0 != moveInput.y)
		{
			if (_moveSpeed <= _moveMaxSpeed)
			{
				// 加速
				_moveSpeed += _accelSpeed * Time.deltaTime;
			}

			// 移動方向を計算
			_moveVector = ((_myTransform.forward * moveInput.x) + (_myTransform.right * moveInput.y)).normalized;	

			_moveAnim.SetBool(RUN, true);
		}
		// 入力されなかったら
		else if (0 == moveInput.x && 0 == moveInput.y)
		{
			// 減速
			_moveSpeed -= _brakeSpeed * Time.deltaTime;

			if (_moveSpeed < 0.1f)
			{
				_moveSpeed = 0f;
			}

			_moveAnim.SetBool(RUN, false);
		}

		// ブレーキ
		//Brake();

		// 進行方向を向く
		LookForward();

		// 移動
		_myTransform.position
			+= _moveVector * _moveSpeed * Time.deltaTime;
	}

	/// <summary>
	/// 進行方向を向く処理
	/// </summary>
	private void LookForward()
	{
        if (_moveVector != Vector3.zero)
        {
            Quaternion forwardRotate = Quaternion.LookRotation(_moveVector, -_gravityScript.GravityDirection);

            _child.rotation = Quaternion.Slerp(_child.rotation, forwardRotate, _rotationSpeed * Time.deltaTime);
        }
    }

	/// <summary>
	/// ブレーキ処理
	/// </summary>
	private void Brake()
    {
		if (isBrake)
		{
			Debug.Log("ブレーキ");
			// 減速
			_moveSpeed -= _brakeSpeed * Time.deltaTime;

			if (_moveSpeed < 0.1f)
			{
				isBrake = false;

				_moveSpeed = 0f;
			}
		}
	}
}