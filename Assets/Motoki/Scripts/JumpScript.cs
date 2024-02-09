/*-------------------------------------------------
* JumpScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/05
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// ジャンプクラス
/// </summary>
public class JumpScript : MonoBehaviour 
{

	#region 定数

	// 半分
	private const int HALF = 2;

	private const string JUMP = "Jump";

	#endregion

	#region フィールド変数

	[SerializeField,Header("ジャンプ力"),Range(0,1000)]
	private float _jumpMaxPower = 0f;

	[SerializeField,Header("ジャンプ時間"),Range(0,5)]
	private float _jumpBaseTime = 0f;



	// 最大ジャンプ力
	private float _jumpPower = 0f;

	// タイマー
	private float _jumpTime = 0f;


	// タイマーの中間
	private float _halfTime = 0f;

	// 自分のTransform
	private Transform _myTransform = default;

	// 入力クラス
	private InputScript _inputScript = default;

	// 重力クラス
	private GravityScript _gravityScript = default;

	private Animator _playerAnimator = default;

	private JumpState _jumpState = JumpState.START;

	public enum JumpState
    {
		START,
		JUMP,
		END
    }

    #endregion

    #region 定数

	public JumpState JumpType { get => _jumpState; set => _jumpState = value; }

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start () 
	{
		// 自分のTransformを設定
		_myTransform = transform;

		// Scriptを取得
		_inputScript = GetComponent<InputScript>();
		_gravityScript = GetComponent<GravityScript>();

		// タイマーの中間を設定
		_halfTime = _jumpBaseTime / HALF;

		_playerAnimator = GetComponent<Animator>();
	}
	
	/// <summary>
    /// 更新処理
    /// </summary>
	private void Update () 
	{
		Jump();
	}

	/// <summary>
	/// ジャンプ処理
	/// </summary>
	private void Jump()
    {
        switch (_jumpState)
        {
			// 開始状態
            case JumpState.START:

				// 入力判定
				// 着地判定
				if (_inputScript.IsJumpButtonDown()
					&& _gravityScript.IsGround())
                {
					_jumpState = JumpState.JUMP;

					_playerAnimator.SetBool(JUMP, true);

					_jumpPower = 0f;

					// タイマーの初期化
					_jumpTime = _jumpBaseTime;
				}
					break;

			// ジャンプ状態
            case JumpState.JUMP:

				_jumpPower = DemandJumpPower(_jumpPower,_jumpMaxPower,_jumpTime,_jumpBaseTime);

				// 上方向に移動
				_myTransform.position 
					+= _myTransform.up * _jumpPower * Time.deltaTime;

				_jumpTime -= Time.deltaTime;

				// タイマーが終了したら
				if(_jumpTime <= 0f)
                {
					_jumpState = JumpState.END;
                }

				break;

			// 終了状態
            case JumpState.END:

				// メモ 着地クールタイム

				_playerAnimator.SetBool(JUMP, false);

				_jumpState = JumpState.START;

				break;
        }
    }

	/// <summary>
	/// ジャンプの強さを求める処理
	/// </summary>
	/// <param name="time">経過時間</param>
	/// <param name="baseTime">設定時間</param>
	/// <returns>ジャンプの強さ</returns>
	private float DemandJumpPower(float jumpPower,float jumpMaxPower,float time,float baseTime)
    {

        if(_halfTime < _jumpTime)
        {
			jumpPower += (jumpMaxPower / baseTime) * Time.deltaTime;
        }
		else if (_jumpTime <= _halfTime)
		{
			jumpPower -= (jumpMaxPower / baseTime) * Time.deltaTime;
		}


		if(jumpMaxPower < jumpPower)
        {
			jumpPower = jumpMaxPower;
        }
		else if(jumpPower < 0f)
        {
			jumpPower = 0f;
        }
		Debug.Log(jumpPower);
		return jumpPower;
	}
}