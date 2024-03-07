/*-------------------------------------------------
* EnemyControlScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/29
*
* 作成者　本木大地
-------------------------------------------------*/

using UnityEngine;

public class EnemyControlScript : CharacterControlScript
{

    #region 定数

    #endregion

    #region フィールド変数

    private EnemyState _enemyState = EnemyState.PATROL;

    private enum EnemyState
    {
        PATROL,
        CHASE,
        DEATH
    }

    #endregion

    #region 定数
    #endregion

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

    private EnemyState EnemyStateMachine()
    {
        EnemyState stateTemp = _enemyState;

        switch (_enemyState)
        {
            case EnemyState.PATROL:
                break;
            case EnemyState.CHASE:
                break;
            case EnemyState.DEATH:
                break;
        }

        return stateTemp;
    }

    private void Patrol()
    {

    }

    private void Chase()
    {

    }

    private void Death()
    {

    }
}