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

	// 地面
	private const string GROUND = "Ground";

	#endregion

	#region フィールド変数

	[SerializeField,Header("ジャンプ速度"),Range(0,1000)]
	private float _jumpSpeed = default;

	[SerializeField,Header("ジャンプ時間"),Range(0,5)]
	private float _jumpTime = default;

	// 自分のTransform
	private Transform _myTransform = default;

	// 入力クラス
	private InputScript _inputScript = default;

	// ジャンプ判定
	private bool isJump = false;

	// タイマークラス
	private TimerScript _timerScript = default;

	// 重力クラス
	private GravityScript _gravityScript = default;

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

		// タイマーを生成
		_timerScript = new TimerScript(_jumpTime,TimerScript.TimerState.END);
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
		// ジャンプ判定
		// 入力判定
		// 着地判定
		if (!isJump
			&& _inputScript.IsSpace()
			&& _gravityScript.IsGround())
		{
			isJump = true;

			// タイマーの初期化
			_timerScript.TimerReset();
		}
		else if (isJump)
		{
			if (_inputScript.IsSpace()
				&& _timerScript.Execute() == TimerScript.TimerState.EXECUTE)
			{
				_myTransform.position += _myTransform.up * _jumpSpeed * Time.deltaTime;
			}
			else if (!_inputScript.IsSpace()
				|| _timerScript.Execute() == TimerScript.TimerState.END)
            {
				isJump = false;
            }
			
		}
    }
}