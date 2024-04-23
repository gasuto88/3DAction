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

    #region 定数

    private const float MARGIN_DIRECTION = 0.02f;

    #endregion

    #region フィールド変数

    // 敵を制御するクラス
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
    /// 移動入力判定処理
    /// </summary>
    /// <returns>Vector2の入力情報</returns>
    public override Vector2 InputMove()
    {
        // 相手の方向を設定
        Vector3 targetDirection = _enemyControlScript.TargetDirection;

        // ローカルに変換
        Vector3 localDirection = transform.InverseTransformDirection(targetDirection);
        
        // Vector2に変換
        Vector2 inputDirection = (Vector2.right * localDirection.z) + (Vector2.up * localDirection.x);

        return inputDirection;
    }

    /// <summary>
    /// ジャンプ入力判定
    /// </summary>
    /// <returns>入力判定</returns>
    public override bool IsJumpButtonDown()
    {
        return _enemyControlScript.IsJump;
    }
}