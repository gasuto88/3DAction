/*-------------------------------------------------
* EnemyInputScript.cs
* 
* 作成日　2024/03/06
* 更新日　2024/03/06
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// 敵入力クラス
/// </summary>
public class EnemyInputScript : InputScript
{

    #region フィールド変数

    private EnemyControlScript _enemyControlScript = default;

    #endregion


    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {
        // Scriptを取得
        _enemyControlScript = GetComponent<EnemyControlScript>();
    }

    /// <summary>
    /// 移動入力処理
    /// </summary>
    /// <returns></returns>
    public override Vector2 InputMove()
    {
        // 相手の方向を設定
        Vector3 targetDirection = UnifyOneNumber(_enemyControlScript.TargetDirection);
        
        // Vector2に変換
        Vector2 inputDirection = (Vector2.right * targetDirection.z) + (Vector2.up * targetDirection.x);

        Debug.LogError(inputDirection);
        return inputDirection;
    }

    public override bool IsJumpButtonDown()
    {
        return false;
    }

    /// <summary>
    /// 数字を１に統一する処理
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private Vector3 UnifyOneNumber(Vector3 direction)
    {
        if(direction.x < 0)
        {
            direction.x = -1;
        }
        else if(0 < direction.x)
        {
            direction.x = 1;
        }
        if(direction.z < 0)
        {
            direction.z = -1;
        }
        else if(0 < direction.z)
        {
            direction.z = 1;
        }
        return direction;
    }
}