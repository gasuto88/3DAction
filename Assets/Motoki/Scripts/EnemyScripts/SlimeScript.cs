/*-------------------------------------------------
* SlimeScript.cs
* 
* 作成日　2024/03/14
* 更新日　2024/03/14
*
* 作成者　本木大地
-------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : EnemyControlScript
{

    #region 定数

    private const float DISTANCE_TO_GROUND = 2f;

    // アニメーションの名前
    private const string JUMP_FLAG_NAME = "Jump";

    // 半分
    private const int HALF = 2;

    #endregion

    #region フィールド変数

    [SerializeField, Header("ジャンプ力"), Range(0, 1000)]
    private float _jumpMaxPower = 0f;

    [SerializeField, Header("ジャンプ時間"), Range(0, 5)]
    private float _jumpCoolTime = 0f;

    // 最大ジャンプ力
    private float _jumpPower = 0f;

    // タイマー
    private float _jumpTime = 0f;

    // タイマーの中間
    private float _halfTime = 0f;   

    private JumpState _jumpState = JumpState.START;

    private enum JumpState
    {
        IDLE,
        START,
        JUMP,
        END
    }

    #endregion

    protected override void OnInit()
    {
        // タイマーの中間を設定
        _halfTime = _jumpCoolTime / HALF;

        isJump = true;
    }

    protected override void EnemyControl()
    {
        if (_jumpState != JumpState.JUMP)
        {
            // 重力落下
            FallInGravity();
        }

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
    /// ジャンプ用着地判定
    /// </summary>
    /// <returns>着地判定</returns>
    private bool IsJumpGround()
    {
        // 惑星までの距離を設定
        float distance
            = _planetManagerScript.DistanceToPlanet(
                _nowPlanet.transform.position,
                _legTransform.position - _myTransform.up * DISTANCE_TO_GROUND);

        if (distance <= _nowPlanet.PlanetRadius)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// ジャンプ初期化処理
    /// </summary>
    private void JumpInit()
    {
        _characterAnimator.SetBool(JUMP_FLAG_NAME, true);

        _jumpPower = 0f;

        // タイマーの初期化
        _jumpTime = _jumpCoolTime;
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Jump()
    {
        _jumpPower
            = CalculationJumpPower(_jumpPower, _jumpMaxPower, _jumpTime, _jumpCoolTime);

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
                    && _nowPlanet != null && IsJumpGround())
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

                if (isGround)
                {
                    stateTemp = JumpState.IDLE;
                }
                break;
        }

        // ジャンプ状態
        return stateTemp;
    }

    /// <summary>
    /// ジャンプ計算処理
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