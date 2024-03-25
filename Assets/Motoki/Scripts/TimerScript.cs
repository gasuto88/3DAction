/*-------------------------------------------------
* TimerScript.cs
* 
* 作成日　2024/02/06
* 更新日　2024/02/06
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// タイマークラス
/// </summary>
public class TimerScript
{
	/// <summary>
	/// 初期化処理
	/// </summary>
	/// <param name="time">計測時間</param>
	/// <param name="timerState">タイマーの状態</param>
	public TimerScript(float time, TimerState timerState)
	{
		this._time = time;
		this._baseTime = time;
		this._timerState = timerState;
	}

	#region フィールド変数

	// タイマーの経過時間
	private float _time = 0f;

	// 計測時間
	private float _baseTime = 0f;

	// タイマーの状態
	private TimerState _timerState = TimerState.EXECUTE;

	public enum TimerState
    {
		EXECUTE,
		END
    }

	#endregion

	/// <summary>
	/// 実行処理
	/// </summary>
	/// <returns></returns>
	public TimerState Execute()
    {
		// タイマーが終了したら
		if(_time <= 0f)
        {
			_timerState = TimerState.END;
        }

		_time -= Time.deltaTime;

		return _timerState;
    }

	/// <summary>
	/// タイマーの初期化
	/// </summary>
    public void TimerReset()
    {
		_time = _baseTime;
		_timerState = TimerState.EXECUTE;
    }
}