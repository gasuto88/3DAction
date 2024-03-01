/*-------------------------------------------------
* CharacterControlScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/29
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// キャラクターを制御するクラス
/// </summary>
public class CharacterControlScript : MonoBehaviour 
{
	#region 定数

	// 地面
	private const string GROUND = "Ground";

	
	// タグの名前
	private const string PLANET = "Planet";
	private const string SPHERE = "Sphere";
	private const string CUBE = "Cube";

	// アニメーション
	private const string RUN = "Run";
	private const string JUMP = "Jump";

	// 半分
	private const int HALF = 2;

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

	[SerializeField, Header("重力の強さ"), Range(0, 100)]
	private float _gravityMaxPower = 0f;

	[SerializeField, Header("重力が最大になるまでの速度"), Range(0, 100)]
	private float _gravityMaxSpeed = 0f;

	[SerializeField, Header("重力回転速度"), Range(0, 400)]
	private float _gravityRotationSpeed = 0f;

	[SerializeField, Header("重力変更時の回転速度"), Range(0, 100)]
	private float _gravityChangeRotaionSpeed = 0f;

	[SerializeField, Header("重力を受ける範囲"), Range(0, 1000)]
	private float _gravityScope = 0f;

	[SerializeField, Header("重力変更時間"), Range(0, 10)]
	private float _gravityChangeTime = 0f;

	[SerializeField, Header("足元の大きさ")]
	private Vector3 _legSize = default;

	// 移動速度
	private float _moveSpeed = 0f;

	// ブレーキ終了判定
	private bool isBrake = default;

	// 無入力時間
	private float _noInputTime = 0f;

	// 自分のTransform
	protected Transform _myTransform = default;

	// 進行方向
	private Vector3 _moveVector = default;

	private Vector3 _beforePosition = default;

	// プレイヤーの子
	private Transform _child = default;

	// 移動アニメーション
	protected Animator _moveAnim = default;

	// 入力クラス
	protected InputScript _inputScript = default;

	private CameraControlScript _controlCameraScript = default;

	private Animator _playerAnimator = default;

	private float _gravityPower = 0f;

	private int _gravityNumber = 0;

	private float _rotationSpeedTemp = 0f;

	// 惑星のTransform
	private Transform _planet = default;

	// 惑星の数
	private int _planetCount = 0;

	// 惑星の半径
	private float _planetRadius = 0f;

	// 惑星までの距離
	//private float _nearPlanetDistance = 10000f;

	// 移動クラス
	private CharacterControlScript _moveScript = default;

	// タイマー
	private TimerScript _timerScript = default;

	// 重力方向
	private Vector3 _gravityDirection = Vector3.down;

	// 惑星
	private Transform[] _planets = default;

	private float[] _planetRadiuses = default;

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

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    protected virtual void Start () 
	{
		// 自分のTransformを設定
		_myTransform = transform;

		_beforePosition = _myTransform.position;

		// 子を取得
		_child = GameObject.FindGameObjectWithTag("Avatar").transform;

        // 移動アニメーションを取得
        _moveAnim = GetComponent<Animator>();

        // Scriptを取得
       _inputScript = GetComponent<InputScript>();

        _controlCameraScript 
			= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControlScript>();

		_noInputTime = _noInputMaxTime;

		Transform planet = GameObject.FindGameObjectWithTag("Planet").transform;

		// 惑星の数を設定
		_planetCount = planet.childCount;
		_planets = new Transform[_planetCount];
		_planetRadiuses = new float[_planetCount];

		for (int i = 0; i < _planetCount; i++)
		{
			// 惑星を取得
			_planets[i] = planet.GetChild(i);
		}

		for (int k = 0; k < _planetCount; k++)
		{
			// 惑星の半径を設定
			_planetRadiuses[k] = _planets[k].localScale.x / HALF;
		}

		_timerScript = new TimerScript(_gravityChangeTime, TimerScript.TimerState.END);

		_rotationSpeedTemp = _gravityRotationSpeed;

		//SetNearPlanet();
	}

	public virtual void CharacterControl()
    {
		
	}

	/// <summary>
	/// 移動処理
	/// </summary>
	protected void Move(Vector2 moveInput)
    {	
		// 入力されたら
		if (0 != moveInput.x || 0 != moveInput.y)
		{
			if (_moveSpeed <= _moveMaxSpeed)
			{
				// 加速
				_moveSpeed += _accelSpeed * Time.deltaTime;
			}

			// 移動方向を計算
			_moveVector = ((_myTransform.forward * moveInput.x) + (_myTransform.right * moveInput.y));	

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
		Debug.Log(_planet);
		if (_planet != null)
		{
			// 惑星の方向を設定
			_gravityDirection = (_planet.position - _myTransform.position).normalized;
		}
		if (_timerScript.Execute() == TimerScript.TimerState.END)
		{
			// 重力回転
			RotateGravity();
		}
		// 重力落下
		FallInGravity();

		// 進行方向を向く
		LookForward();

		// 移動
		_myTransform.position
			+= _moveVector * _moveSpeed * Time.deltaTime;
	}

	/// <summary>
	/// 重力落下処理
	/// </summary>
	private void FallInGravity()
    {
        //if (_planet != null)
        //{
        //    // 惑星の方向を設定
        //    _gravityDirection = (_planet.position - _myTransform.position).normalized;
        //}
        //else
        //{
        //    _gravityDirection = Vector3.down;
        //}

        // 着地判定
        if (!IsGround())
        {
            _gravityPower = UpGravityPower(_gravityPower, _gravityMaxPower, _gravityMaxSpeed);

            // 重力
            _myTransform.position += _gravityDirection * _gravityPower * Time.deltaTime;
        }
        else
        {
            _gravityPower = 0f;
        }
    }

    /// <summary>
    /// 重力回転処理
    /// </summary>
    private void RotateGravity()
    {
		// 重力の回転を設定
		Quaternion gravityRotation
			= Quaternion.FromToRotation(-_myTransform.up, _gravityDirection) * _myTransform.rotation;

		_myTransform.rotation = gravityRotation;
	}

	/// <summary>
	/// 進行方向を向く処理
	/// </summary>
	private void LookForward()
	{
        if (_moveVector != Vector3.zero)
        {
            Quaternion forwardRotate = Quaternion.LookRotation(_moveVector, -_gravityDirection);

			_child.rotation 
				= Quaternion.Slerp(_child.rotation, forwardRotate, _rotationSpeed * Time.deltaTime);
        }
    }

	/// <summary>
	/// 惑星変更したときの回転処理
	/// </summary>
	private void RotateChangePlanet()
    {
		//// 重力の回転を設定
		//Quaternion gravityRotation
		//	= Quaternion.LookRotation(_gravityDirection, _myTransform.up);
		Quaternion gravityRotation 
			= Quaternion.FromToRotation(_myTransform.forward, _gravityDirection)
			* _myTransform.rotation;

		_myTransform.rotation 
			= Quaternion.Slerp(_myTransform.rotation, gravityRotation,
			_gravityRotationSpeed * Time.deltaTime);
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < _planetCount; i++)
		{
			Gizmos.DrawWireSphere(_planets[i].position, _planetRadiuses[i] + _gravityScope);
		}
	}

	/// <summary>
	/// 着地判定
	/// </summary>
	public bool IsGround()
	{
		if (Physics.CheckBox(_myTransform.position + _myTransform.up * 0.6f, _legSize,
			_myTransform.rotation, LayerMask.GetMask(PLANET)))
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// 重力を増やす処理
	/// </summary>
	/// <param name="power"></param>
	/// <param name="MaxPower"></param>
	/// <param name="Speed"></param>
	/// <returns></returns>
	private float UpGravityPower(float power, float MaxPower, float Speed)
	{
		if (power <= MaxPower)
		{
			power += Speed * Time.deltaTime;
		}

		return power;
	}

	/// <summary>
	/// 惑星までの距離を求める処理
	/// </summary>
	/// <param name="planet">惑星の中心座標</param>
	/// <param name="myPoint">自分の座標</param>
	/// <returns>距離</returns>
	public float DistanceToPlanet(Vector3 planet, Vector3 myPoint)
	{
		// 2乗計算
		float squareA = (myPoint.x - planet.x) * (myPoint.x - planet.x);
		float squareB = (myPoint.y - planet.y) * (myPoint.y - planet.y);
		float squareC = (myPoint.z - planet.z) * (myPoint.z - planet.z);

		float radiusSquare = squareA + squareB + squareC;

		// 2乗をルート化する
		float distance = Mathf.Sqrt(radiusSquare);

		return distance;
	}
}