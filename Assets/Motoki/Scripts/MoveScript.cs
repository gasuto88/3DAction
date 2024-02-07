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
    private const float HALFINPUT = 0.5f;

    #endregion

    #region フィールド変数

    [SerializeField,Header("移動速度"),Range(0,100)]
	private float _moveMaxSpeed = 0f;

	[SerializeField,Header("振り向き速度"),Range(0,100)]
	private float _rotationSpeed = 0f;

	[SerializeField, Header("加速速度"), Range(0, 100)]
	private float _accelSpeed = 0f;

	//[SerializeField,Header("移動時間"),Range(0,10)]
	//private float _moveBaseTime = 0f;

	// 移動速度
	private float _moveSpeed = 0f;

	//// タイマー
	//private float _moveTime = 0f;

	// 自分のTransform
	private Transform _myTransform = default;

	// 自分の座標
	private Vector3 _myPosition = default;

	// プレイヤーの子
	private Transform _child = default;

	// 移動アニメーション
	private Animator _moveAnim = default;

	// 入力クラス
	private InputScript _inputScript = default;

	// 移動状態
	private MoveState _moveState = MoveState.MOVE;

	/// STAY 待機
	/// WALK 歩く
	/// RUN 走る
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

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start () 
	{
		// 自分のTransformを設定
		_myTransform = transform;

		// 自分の座標を設定
		_myPosition = _myTransform.position;

		// 子を取得
		_child = _myTransform.GetChild(0);
 
		_moveAnim = GetComponent<Animator>();

		// InputScriptを取得
		_inputScript = GetComponent<InputScript>();
	}
	
	/// <summary>
    /// 更新処理
    /// </summary>
	private void Update () 
	{
		Move();
	}

	/// <summary>
	/// 移動処理
	/// </summary>
	private void Move()
    {
		// 入力取得
		Vector2 moveInput = _inputScript.InputMove();

		// 入力されたら
		if (0 == moveInput.x && 0 == moveInput.y)
		{
			_moveAnim.SetBool(RUN, false);

			_moveState = MoveState.STAY;
		}

		// 移動状態
		switch (_moveState)
        {
            // 待機
            case MoveState.STAY:

				// 入力されたら
				if(0 != moveInput.x || 0 != moveInput.y)
                {
					_moveSpeed = 0f;

					_moveAnim.SetBool(RUN,true);

					_moveState = MoveState.MOVE;
				}

                break;
			
			// 移動
			case MoveState.MOVE:

                if (_moveSpeed <= _moveMaxSpeed)
                {
                    _moveSpeed += _accelSpeed;
                }

				// 移動方向を計算
				Vector3 moveVector = (_myTransform.forward * moveInput.x) + (_myTransform.right * moveInput.y);

				// 移動
				_myTransform.position 
					+= moveVector * _moveSpeed * Time.deltaTime;

				// 移動方向を向く
				LookForward(moveVector);

				break;
        }
    }

	/// <summary>
	/// 前を向く処理
	/// </summary>
	private void LookForward(Vector3 forward)
    {
		Quaternion forwardRotate 
			= Quaternion.FromToRotation(_child.forward, forward) * _child.rotation;

		// 回転を設定
		_child.rotation = Quaternion.Slerp(_child.rotation, forwardRotate, _rotationSpeed * Time.deltaTime);
    }
}