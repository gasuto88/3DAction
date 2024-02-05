/*-------------------------------------------------
* JumpScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/05
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

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
	private float _baseTime = default;

	// 自分のTransform
	private Transform _myTransform = default;

	// 足元のTransform
	private Transform _legTransform = default;

	// 入力クラス
	private InputScript _inputScript = default;

	// 惑星の半径
	private float _planetRadius = 0f;

	// 経過時間
	private float _time = 0f;

	// 惑星のTransform
	private Transform _planet = default;

	// タイマーの状態
	private TimerState _timerState = TimerState.End;

	private enum TimerState
    {
		Execute,
		End
    }

	#endregion

	/// <summary>
	/// 更新前処理
	/// </summary>
	private void Start () 
	{
		// 自分のTransformを設定
		_myTransform = transform;

		_legTransform = _myTransform;

		_legTransform.position -= _myTransform.up * _myTransform.localScale.y / 2;

		// InputScripを取得
		_inputScript = GetComponent<InputScript>();

		// 惑星を取得
		_planet = GameObject.FindGameObjectWithTag("Planet").transform;

		// 惑星の半径
		_planetRadius = _planet.lossyScale.x / 2;

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
		// 入力判定
		// 着地判定
		if (_inputScript.IsSpace()
			&& IsGround())
		{
			// タイマー開始
			TimerStart();
		}

		// タイマー判定
        if (Timer() == TimerState.Execute)
        {
			_myTransform.position += Vector3.up * _jumpSpeed * Time.deltaTime;
		}
    }

	/// <summary>
	/// 着地判定
	/// </summary>
	private bool IsGround()
	{
		// 惑星までの距離を設定
		float distance = Vector3.Distance(_planet.position, _legTransform.position);

		if (distance < _planetRadius)
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// タイマー処理
	/// </summary>
	/// <returns>終了判定</returns>
	private TimerState Timer()
    {
		if(_time <= 0f)
        {
			_timerState = TimerState.End;
		}

		_time -= Time.deltaTime;

		return _timerState;
	}

	/// <summary>
	/// タイマー開始処理
	/// </summary>
	private void TimerStart()
    {
		// 時間を設定
		_time = _baseTime;

		_timerState = TimerState.Execute;
    }
}