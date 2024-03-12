/*-------------------------------------------------
* EnemyControlScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/29
*
* 作成者　本木大地
-------------------------------------------------*/

using UnityEngine;

/// <summary>
/// 敵を制御するクラス
/// </summary>
public class EnemyControlScript : CharacterControlScript
{

    #region 定数

    private const int HALF = 2;

    private const float MARGIN_DISTANCE = 1f;

    // アニメーションの名前
    private const string DAMAGE_FLAG_NAME = "Damage";

    #endregion

    #region フィールド変数

    [SerializeField,Header("視野角"),Range(0,360)]
    private float _viewAngle = 0f;

    [SerializeField, Header("最大距離"), Range(0, 1000)]
    private float _maxDistance = 0f;

    [SerializeField,Header("追尾時間"),Range(0,100)]
    private float _chaseCoolTime = 0f;

    [SerializeField, Header("追尾距離"), Range(0, 100)]
    private float _chaseDistance = 0f;

    [SerializeField,Header("待機時間"),Range(0,100)]
    private float _idleCoolTime = 0f;

    private Transform _playerTransform = default;

    private Vector3 _targetDirection = default;

    private Vector3 _randomPosition = default;

    // 追尾時間
    private float _chaseTime = 0f;

    // 待機時間
    private float _idleTime = 0f;

    private EnemyState _enemyState = EnemyState.PATROL;

    private PatrolState _patrolState = PatrolState.START;

    private enum EnemyState
    {
        IDLE,
        PATROL,
        CHASE      
    }

    private enum PatrolState
    {
        START,
        MOVE,
        END
    }

    #endregion

    #region プロパティ

    public Vector3 TargetDirection { get => _targetDirection; set => _targetDirection = value; }

    #endregion

    protected override void OnInitialize()
    {
        // プレイヤーを取得
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // 追尾時間を設定
        _chaseTime = _chaseCoolTime;

        // 待機時間を設定
        _idleTime = _idleCoolTime;
    }

    /// <summary>
    /// キャラクター制御処理
    /// </summary>
    public override void CharacterControl()
    {
        base.CharacterControl();

        _enemyState = EnemyStateMachine();

        switch (_enemyState)
        {
            // 待機状態
            case EnemyState.IDLE:

                _targetDirection = Vector3.zero;

                break;

            // 探索状態
            case EnemyState.PATROL:

                Patrol();

                break;
            // 追尾状態
            case EnemyState.CHASE:

                // 相手の方向を設定
                _targetDirection = (_playerTransform.position - _myTransform.position).normalized;
                
                break;
        }

        FallInGravity();
    }

    /// <summary>
    /// 敵の状態管理処理
    /// </summary>
    /// <returns>敵の状態</returns>
    private EnemyState EnemyStateMachine()
    {
        EnemyState stateTemp = _enemyState;

        //  視界判定
        if (IsVision())
        {
            stateTemp = EnemyState.CHASE;

            return stateTemp;
        }

        switch (_enemyState)
        {
            // 待機状態
            case EnemyState.IDLE:

                _idleTime -= Time.deltaTime;

                // 時間経過したら
                if(_idleTime <= 0f)
                {
                    // 経過時間を初期化
                    _idleTime = _idleCoolTime;

                    stateTemp = EnemyState.PATROL;
                }

                break;

            // 探索状態
            case EnemyState.PATROL:

                // 到着判定
                if (IsArriveTarget())
                {
                    _patrolState = PatrolState.START;

                    stateTemp = EnemyState.IDLE;
                }

                break;
            // 追尾状態
            case EnemyState.CHASE:

                // 相手の距離
                float targetDistance = _targetDirection.magnitude;

                _chaseTime -= Time.deltaTime;

                // 時間経過したら
                // 距離が離れたら
                if(_chaseTime <= 0f 
                    || _chaseDistance < targetDistance)
                {
                    _chaseTime = _chaseCoolTime;

                    stateTemp = EnemyState.PATROL;
                }

                break;
        }

        return stateTemp;
    }

    /// <summary>
    /// 探索処理
    /// </summary>
    private void Patrol()
    {
        switch (_patrolState)
        {
            // 開始状態
            case PatrolState.START:

                if (_nowPlanet != null)
                {
                    // ランダムな座標を設定
                    _randomPosition = RandomPlanetPosition();

                    _patrolState = PatrolState.MOVE;
                }
                break;
            // 移動状態
            case PatrolState.MOVE:

                _targetDirection = (_randomPosition - _myTransform.position).normalized;

                break;
        }
    }

    /// <summary>
    /// 視界判定処理
    /// </summary>
    /// <returns>視界判定</returns>
    private bool IsVision()
    {
        //  相手の方向を設定
        Vector3 targetDirection = _playerTransform.position - _myTransform.position;

        // 相手までの距離を設定
        float targetDistance = targetDirection.magnitude;

        // コサイン計算
        float cosHalf = Mathf.Cos(_viewAngle / HALF * Mathf.Deg2Rad);

        // 内積計算
        float innerProduct = Vector3.Dot(_child.forward, targetDirection.normalized);

        //  視界判定
        if(cosHalf < innerProduct
            && targetDistance < _maxDistance)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// ランダムな方向を計算する処理
    /// </summary>
    /// <returns>ランダムな方向</returns>
    private Vector3 RandomDirection()
    {
        Vector3 randomDirection = Vector3.zero;

        for (int i = 0; randomDirection == Vector3.zero; i++)
        {         
            if(50 < i)
            {
                randomDirection = Vector3.one;
                break;
            }
            randomDirection.x = Random.Range(-1f, 1f);
            randomDirection.y = Random.Range(-1f, 1f);
            randomDirection.z = Random.Range(-1f, 1f);
        }

        randomDirection = randomDirection.normalized;

        return randomDirection;
    }

    /// <summary>
    /// 惑星上のランダムな座標を計算する処理
    /// </summary>
    /// <returns>ランダムな座標</returns>
    private Vector3 RandomPlanetPosition()
    {
        
        Vector3 randomPosition 
            = _nowPlanet.PlanetTransform.position 
            + RandomDirection() * (_nowPlanet.PlanetRadius);

        return randomPosition;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        if (_randomPosition != null)
        {
            Gizmos.DrawCube(_randomPosition, Vector3.one);
        }
    }

    /// <summary>
    /// 目標地点の到着判定
    /// </summary>
    /// <returns>到着判定</returns>
    private bool IsArriveTarget()
    {
        // 目標地点に入ったら
        if((_randomPosition.x - MARGIN_DISTANCE < _myTransform.position.x
            && _myTransform.position.x < _randomPosition.x + MARGIN_DISTANCE)
            && (_randomPosition.y - MARGIN_DISTANCE < _myTransform.position.y
            && _myTransform.position.y < _randomPosition.y + MARGIN_DISTANCE)
            && (_randomPosition.z - MARGIN_DISTANCE < _myTransform.position.z
            && _myTransform.position.z < _randomPosition.z + MARGIN_DISTANCE))
        {
            return true;
        }

        return false;
    }

    public override void DownHp(int damage)
    {
        _hp -= damage;

        if (_hp <= 0)
        {
            _characterAnimator.SetBool(DAMAGE_FLAG_NAME, true);
        }
    }

}