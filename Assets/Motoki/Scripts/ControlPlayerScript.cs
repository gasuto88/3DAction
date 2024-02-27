/*-------------------------------------------------
* PlayerScript.cs
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
public class ControlPlayerScript : MonoBehaviour
{

    #region 定数

    private const string PLANET = "Planet";
    private const string CUBE = "Cube";

    #endregion

    #region フィールド変数

    [SerializeField, Header("足元の座標")]
    private Transform _legTransform = default;

    [SerializeField, Header("体の大きさ")]
    private Vector3 _bodySize = default;

    [SerializeField,Header("足元の大きさ")]
    private Vector3 _legSize = default;

    private Transform _myTransform = default;

    private RaycastHit _playerHit = default;

    private RaycastHit[] _playerHits = default;

    private float _rayDistance = 1f;

    private Collider[] colliders = new Collider[2];

    private MoveScript _moveScript = default;

    private JumpScript _jumpScript = default;

    private GravityScript _gravityScript = default;

    #endregion

    #region 定数

    public RaycastHit PlayerHit { get => _playerHit; set => _playerHit = value; }

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {
        _myTransform = transform;

        // Scriptを取得
        _moveScript = GetComponent<MoveScript>();
        _jumpScript = GetComponent<JumpScript>();
        _gravityScript
            = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityScript>();
    }

    /// <summary>
    /// プレイヤーを制御する処理
    /// </summary>
    public void ControlPlayer()
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

    /// <summary>
	/// 着地判定
	/// </summary>
	public bool IsGround()
    {
        // 惑星までの距離を設定
        //float distance
        //    = _gravityScript.DistanceToPlanet(_gravityScript.Planet.position, _legTransform.position);

        //if (distance <= _gravityScript.PlanetRadius + 0.1f)
        //{
        //    return true;
        //}

        if (Physics.CheckBox(_myTransform.position + _myTransform.up * 0.6f, _legSize,
            _myTransform.rotation, LayerMask.GetMask(PLANET)))
        {
            return true;
        }
        return false;
    }

    public bool IsCollision()
    {
        _playerHits = Physics.BoxCastAll(_myTransform.position + _myTransform.up * 0.6f, _bodySize,
             _myTransform.forward, _myTransform.rotation,_rayDistance, LayerMask.GetMask(PLANET));

        if (0 < _playerHits.Length)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + transform.up * 0.6f, _bodySize);
    }

}