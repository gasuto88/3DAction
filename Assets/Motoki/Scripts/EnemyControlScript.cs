/*-------------------------------------------------
* EnemyControlScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/29
*
* 作成者　本木大地
-------------------------------------------------*/

using UnityEngine;

public class EnemyControlScript : MonoBehaviour 
{

    #region 定数

    private const string PLANET = "Planet";
    private const string CUBE = "Cube";

    #endregion

    #region フィールド変数

    [SerializeField, Header("足元の大きさ")]
    private Vector3 _legSize = default;

    private Transform _myTransform = default;

    private float _rayDistance = 1f;

    private Collider[] colliders = new Collider[2];

    private CharacterControlScript _moveScript = default;

    private JumpScript _jumpScript = default;

    private GravityScript _gravityScript = default;

    #endregion

    #region 定数
    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {
        _myTransform = transform;

        // Scriptを取得
        _moveScript = GetComponent<CharacterControlScript>();
        _jumpScript = GetComponent<JumpScript>();
        _gravityScript
            = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityScript>();
    }

    /// <summary>
    /// プレイヤーを制御する処理
    /// </summary>
    public void EnemyControl()
    {
        _moveScript.Move();
        _jumpScript.Jump();
        GravityBehavior();
    }

    /// <summary>
    /// 重力挙動処理
    /// </summary>
    private void GravityBehavior()
    {
        _myTransform.position += _gravityScript.Gravity(_myTransform.position);
        _myTransform.rotation = _gravityScript.GravityRotate(_myTransform);
    }
}