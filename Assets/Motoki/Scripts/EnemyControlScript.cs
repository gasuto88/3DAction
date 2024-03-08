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

    private Transform _target = default;

    private Vector3 _targetDirection = default;

    // 追尾時間
    private float _chaseTime = default;

    private EnemyState _enemyState = EnemyState.PATROL;

    private enum EnemyState
    {
        PATROL,
        CHASE,
        DEATH
    }

    #endregion

    #region プロパティ

    public Vector3 TargetDirection { get => _targetDirection; set => _targetDirection = value; }

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // プレイヤー取得
        _target = GameObject.FindGameObjectWithTag("Player").transform;

        // 追尾時間を設定
        _chaseTime = _chaseCoolTime;
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
            // 探索状態
            case EnemyState.PATROL:

                Patrol();

                break;
            // 追尾状態
            case EnemyState.CHASE:

                Chase();

                break;
            // 死亡状態
            case EnemyState.DEATH:

                Death();

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

        switch (_enemyState)
        {
            // 探索状態
            case EnemyState.PATROL:

                //  視界判定
                if (IsVision())
                {
                    stateTemp = EnemyState.CHASE;
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
            // 死亡状態
            case EnemyState.DEATH:
                break;
        }

        return stateTemp;
    }

    /// <summary>
    /// 探索処理
    /// </summary>
    private void Patrol()
    {

    }

    /// <summary>
    /// 追尾処理
    /// </summary>
    private void Chase()
    {
        // 相手の方向を設定
        _targetDirection = _target.position - _myTransform.position;        
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    private void Death()
    {

    }

    /// <summary>
    /// 視界判定処理
    /// </summary>
    /// <returns>視界判定</returns>
    private bool IsVision()
    {
        //  相手の方向を設定
        Vector3 targetDirection = _target.position - _myTransform.position;

        // 相手までの距離を設定
        float targetDistance = targetDirection.magnitude;

        // コサイン計算
        float cosHalf = Mathf.Cos(_viewAngle / HALF * Mathf.Deg2Rad);

        // 内積計算
        float innerProduct = Vector3.Dot(_myTransform.forward, targetDirection.normalized);

        //  視界判定
        if(cosHalf < innerProduct
            && targetDistance < _maxDistance)
        {
            return true;
        }

        return false;
    }

}