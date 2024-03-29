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

    // 地面からの距離
    private const float DISTANCE_TO_GROUND = 2f;

    // レイヤーの名前
    private const string STAR_LAYER_NAME = "Star";

    // アニメーションの名前
    private const string JUMP_FLAG_NAME = "Jump";
    private const string DEATH_FLAG_NAME = "Death";

    // 半分
    private const int HALF = 2;

    #endregion

    #region フィールド定数

    [SerializeField, Header("ジャンプ力"), Range(0, 1000)]
    private float _jumpMaxPower = 0f;

    [SerializeField, Header("ジャンプ時間"), Range(0, 5)]
    private float _jumpCoolTime = 0f;

    [SerializeField, Header("点滅時間"), Range(0, 10)]
    private float _flashCoolTime = 0f;

    [SerializeField, Header("点滅間隔時間"), Range(0, 10)]
    private float _flashIntervalCoolTime = 0f;

    [SerializeField, Header("ジャンプダメージ"), Range(0, 10)]
    private int _jumpDamage = 0;

    [SerializeField, Header("衝突で受けるダメージ"), Range(0, 10)]
    private int _collisionDamage = 0;

    [SerializeField, Header("当たり判定の大きさ")]
    private Vector3 _halfSize = default;

    // 最大ジャンプ力
    private float _jumpPower = 0f;

    // タイマー
    private float _jumpTime = 0f;

    // タイマーの中間
    private float _halfTime = 0f;

    // 点滅時間
    private float _flashTime = 0f;

    // 点滅間隔時間
    private float _flashIntervalTime = 0f;

    // 衝突判定
    private bool isCollision = false;

    // ダメージ判定
    private bool isDamage = false;

    // プレイヤーのMeshRenderer
    private SkinnedMeshRenderer _playerMeshRenderer = default;

    // UI制御クラス
    private UIControlScript _uiControlScript = default;

    // ゲーム管理クラス
    private GameManagerScript _gameManagerScript = default;

    // ジャンプ状態
    private JumpState _jumpState = JumpState.START;

    // ダメージ状態
    private DamageState _damageState = DamageState.OFF;

    private enum JumpState
    {
        IDLE,
        START,
        JUMP,
        END
    }

    private enum DamageState
    {
        ON,
        OFF
    }

    #endregion

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected override void OnInitialize()
    {
        // タイマーの中間を設定
        _halfTime = _jumpCoolTime / HALF;

        // 点滅時間を設定
        _flashTime = _flashCoolTime;

        // 点滅感覚時間を設定
        _flashIntervalTime = _flashIntervalCoolTime;

        // プレイヤーのSkinnedMeshRendererを取得
        _playerMeshRenderer = _myTransform.Find("Player(Mesh)").GetComponent<SkinnedMeshRenderer>();

        // Script取得
        _uiControlScript
            = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<UIControlScript>();
        _gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    /// <summary>
    /// キャラクター制御処理
    /// </summary>
    public override void CharacterControl()
    {
        base.CharacterControl();

        // スターの衝突判定
        if (IsCollisionStar())
        {
            // ゲームクリア
            _gameManagerScript.GameClear();
        }

        if (_jumpState != JumpState.JUMP)
        {
            // 重力落下
            FallInGravity();
        }

        // ジャンプ状態を取得
        _jumpState = JumpStateMachine();

        // 敵の衝突
        CollisionEnemy();

        // ダメージ判定
        if (isDamage)
        {
            // ダメージ点滅処理
            FlashingDamage();
        }

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

                isCollision = false;

                // ジャンプアニメーションを初期化
                _characterAnimator.SetBool(JUMP_FLAG_NAME, false);

                break;
        }
    }

    /// <summary>
    /// スター衝突判定処理
    /// </summary>
    /// <returns>衝突判定</returns>
    private bool IsCollisionStar()
    {
        Collider[] enemyColliders
            = Physics.OverlapBox(_myTransform.position + _myTransform.up * 0.5f, _halfSize,
            _myTransform.rotation, LayerMask.GetMask(STAR_LAYER_NAME));

        if (0 < enemyColliders.Length)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 衝突対象を求める処理
    /// </summary>
    /// <returns>衝突対象</returns>
    private Transform CollisionEnemyDirection()
    {
        Collider[] enemyColliders
            = Physics.OverlapBox(_myTransform.position + _myTransform.up * 0.5f, _halfSize,
            _myTransform.rotation, LayerMask.GetMask(ENEMY_LAYER_NAME));

        if (0 < enemyColliders.Length)
        {
            return enemyColliders[0].transform;
        }

        return null;
    }

    /// <summary>
    /// 敵の衝突処理
    /// </summary>
    private void CollisionEnemy()
    {
        // 衝突対象を設定
        Transform enemyTransform = CollisionEnemyDirection();

        // 衝突判定
        if (!isDamage && !isCollision
            && enemyTransform != null)
        {
            isCollision = true;

            // 衝突方向を計算
            Vector3 enemyDirection
                = (enemyTransform.position - _legTransform.position).normalized;

            // 衝突角度を計算
            float collisionAngle = Vector3.Angle(_myTransform.up, enemyDirection);

            // 踏みつけ判定
            if (110f < collisionAngle)
            {
                _jumpState = JumpState.START;

                enemyTransform.GetComponent<CharacterControlScript>().DownHp(_jumpDamage);
            }
            // ダメージ判定
            else　if(collisionAngle <= 110f)
            {
                DownHp(_collisionDamage);

                if (0 < _hp)
                {
                    isDamage = true;
                }
            }
        }
    }

    /// <summary>
    /// HPを減らす処理
    /// </summary>
    /// <param name="damage">ダメージ</param>
    public override void DownHp(int damage)
    {
        if (0 < _hp)
        {
            _hp -= damage;
        }

        // HP表示
        _uiControlScript.DisplayHpUI(_hp);

        if (_hp <= 0)
        {
            _characterAnimator.SetBool(DEATH_FLAG_NAME, true);
            // ゲームオーバー
            _gameManagerScript.GameOver();
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

                // 着地判定
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

    /// <summary>
    /// ダメージ点滅処理
    /// </summary>
    private void FlashingDamage()
    {
        _flashIntervalTime -= Time.deltaTime;

        switch (_damageState)
        {
            case DamageState.ON:

                _playerMeshRenderer.enabled = true;

                if (_flashIntervalTime <= 0f)
                {
                    _flashIntervalTime = _flashIntervalCoolTime;

                    _damageState = DamageState.OFF;
                }

                break;

            case DamageState.OFF:

                _playerMeshRenderer.enabled = false;

                if (_flashIntervalTime <= 0f)
                {
                    _flashIntervalTime = _flashIntervalCoolTime;

                    _damageState = DamageState.ON;
                }

                break;
        }

        _flashTime -= Time.deltaTime;

        if (_flashTime <= 0f)
        {
            _flashTime = _flashCoolTime;
            _playerMeshRenderer.enabled = true;
            _damageState = DamageState.OFF;
            isDamage = false;
            isCollision = false;
        }
    }
}