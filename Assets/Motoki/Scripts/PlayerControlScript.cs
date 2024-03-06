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

    private const string JUMP_FLAG_NAME = "Jump";

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

    protected JumpState _jumpState = JumpState.START;

    public enum JumpState
    {
        IDLE,
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
    }

    public override void CharacterControl()
    {
        base.CharacterControl();

        // ジャンプ状態を取得
        _jumpState = JumpStateMachine();

        switch (_jumpState)
        {
            // 開始状態
            case JumpState.START:

                JumpInit();

                break;

            // ジャンプ状態
            case JumpState.JUMP:

                Jump();

                break;

            // 終了状態
            case JumpState.END:

                _characterAnimator.SetBool(JUMP_FLAG_NAME, false);

                break;
        }
    }

    /// <summary>
    /// ジャンプ初期化処理
    /// </summary>
    private void JumpInit()
    {
        _characterAnimator.SetBool(JUMP_FLAG_NAME, true);

        _jumpPower = 0f;

        // タイマーの初期化
        _jumpTime = _jumpBaseTime;
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Jump()
    {
        _jumpPower = CalculationJumpPower(_jumpPower, _jumpMaxPower, _jumpTime, _jumpBaseTime);

        // 上方向に移動
        _myTransform.position
            += _myTransform.up * _jumpPower * Time.deltaTime;
    }

    /// <summary>
    /// ジャンプ状態管理処理
    /// </summary>
    /// <returns>ジャンプ状態</returns>
    private JumpState JumpStateMachine()
    {
        JumpState stateTemp = _jumpState;

        switch (stateTemp)
        {
            // 待機状態
            case JumpState.IDLE:

                // 入力判定
                // 着地判定
                if (_inputScript.IsJumpButtonDown()
                    && isGround)
                {
                    stateTemp = JumpState.START;
                }
                break;

            // 開始状態
            case JumpState.START:

                stateTemp = JumpState.JUMP;

                break;

            // ジャンプ状態
            case JumpState.JUMP:

                _jumpTime -= Time.deltaTime;

                // タイマーが終了したら
                if (_jumpTime <= 0f)
                {
                    stateTemp = JumpState.END;
                }

                break;

            // 終了状態
            case JumpState.END:

                stateTemp = JumpState.IDLE;

                break;
        }
        
        // ジャンプ状態
        return stateTemp;
    }

    /// <summary>
    /// ジャンプ力を計算する処理
    /// </summary>
    /// <param name="jumpPower">ジャンプ力/param>
    /// <param name="jumpMaxPower">最大ジャンプ力</param>
    /// <param name="time">経過時間</param>
    /// <param name="baseTime">設定時間</param>
    /// <returns>ジャンプ力</returns>
    private float CalculationJumpPower(
        float jumpPower, float jumpMaxPower, float time, float baseTime)
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