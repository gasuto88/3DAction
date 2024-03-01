/*-------------------------------------------------
* PlayerControlScript.cs
* 
* 作成日　2024/02/15
* 更新日　2024/02/27
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// プレイヤーを制御するクラス
/// </summary>
public class PlayerControlScript : CharacterControlScript
{

	#region 定数

	private const string JUMP = "Jump";

	// 半分
	private const int HALF = 2;

	#endregion

	#region フィールド定数

	[SerializeField, Header("ジャンプ力"), Range(0, 1000)]
	private float _jumpMaxPower = 0f;

	[SerializeField, Header("ジャンプ時間"), Range(0, 5)]
	private float _jumpBaseTime = 0f;

	// 最大ジャンプ力
	private float _jumpPower = 0f;

	// タイマー
	private float _jumpTime = 0f;

	// タイマーの中間
	private float _halfTime = 0f;

	private Animator _playerAnimator = default;

	protected JumpState _jumpState = JumpState.START;

	public enum JumpState
	{
		START,
		JUMP,
		END
	}

	#endregion

	#region プロパティ

	public JumpState JumpType { get => _jumpState; set => _jumpState = value; }

	#endregion


	protected override void Start()
    {
		base.Start();
		// タイマーの中間を設定
		_halfTime = _jumpBaseTime / HALF;

		_inputScript = GetComponent<InputScript>();

		// 移動アニメーションを取得
		_moveAnim = GetComponent<Animator>();
	}

    public override void CharacterControl()
    {
		// 入力取得
		Vector2 moveInput = _inputScript.InputMove();

		Move(moveInput);

		Jump();
		
		//SetNearPlanet();
    }

	/// <summary>
	/// ジャンプ処理
	/// </summary>
	protected void Jump()
	{
		switch (_jumpState)
		{
			// 開始状態
			case JumpState.START:

				// 入力判定
				// 着地判定
				if (_inputScript.IsJumpButtonDown()
					&& IsGround())
				{
					_jumpState = JumpState.JUMP;

					_moveAnim.SetBool(JUMP, true);

					_jumpPower = 0f;

					// タイマーの初期化
					_jumpTime = _jumpBaseTime;
				}
				break;

			// ジャンプ状態
			case JumpState.JUMP:

				_jumpPower = DemandJumpPower(_jumpPower, _jumpMaxPower, _jumpTime, _jumpBaseTime);

				// 上方向に移動
				_myTransform.position
					+= _myTransform.up * _jumpPower * Time.deltaTime;

				_jumpTime -= Time.deltaTime;

				// タイマーが終了したら
				if (_jumpTime <= 0f)
				{
					_jumpState = JumpState.END;
				}

				break;

			// 終了状態
			case JumpState.END:

				// メモ 着地クールタイム

				_moveAnim.SetBool(JUMP, false);

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
	private float DemandJumpPower(float jumpPower, float jumpMaxPower, float time, float baseTime)
	{
		if (_halfTime < _jumpTime)
		{
			jumpPower += (jumpMaxPower / baseTime) * Time.deltaTime;
		}
		else if (_jumpTime <= _halfTime)
		{
			jumpPower -= (jumpMaxPower / baseTime) * Time.deltaTime;
		}

		if (jumpMaxPower < jumpPower)
		{
			jumpPower = jumpMaxPower;
		}
		else if (jumpPower < 0f)
		{
			jumpPower = 0f;
		}
		return jumpPower;
	}

}