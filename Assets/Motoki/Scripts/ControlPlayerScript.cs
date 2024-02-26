/*-------------------------------------------------
* PlayerScript.cs
* 
* 作成日　2024/02/15
* 更新日　2024/02/15
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

    #endregion

    #region フィールド変数

    [SerializeField, Header("足元の座標")]
    private Transform _legTransform = default;

    [SerializeField,Header("体の大きさ")]
    private Vector3 _bodySize = default;

    private Transform _myTransform = default;

    private RaycastHit[] _playerHit = default;

    private float _rayDistance = 1f;

    private Collider[] colliders = new Collider[2];

	private MoveScript _moveScript = default;

	private JumpScript _jumpScript = default;

    private GravityScript _gravityScript = default;

    #endregion


    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {
        _myTransform = transform;

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
        if (_gravityScript.Planet != null)
        {
            _moveScript.Move();
            _jumpScript.Jump();
            GravityBehavior();
        }
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
        float distance
            = _gravityScript.DistanceToPlanet(_gravityScript.Planet.position, _legTransform.position);

        if (distance <= _gravityScript.PlanetRadius + 0.1f)
        {
            return true;
        }

        //_playerHit = Physics.BoxCastAll(
        //    _myTransform.position + _myTransform.up * 1.6f, _bodySize, _myTransform.forward,
        //     _myTransform.rotation, _rayDistance, LayerMask.GetMask(PLANET));

        ////colliders = Physics.OverlapBox(
        ////    _myTransform.position + _myTransform.up * 1.6f, _bodySize,
        ////    _myTransform.rotation, LayerMask.GetMask(PLANET));

        ////Physics.BoxCast(_myTransform.position + _myTransform.up * 1.6f, _bodySize, _myTransform.forward,
        ////    out _playerHit, _myTransform.rotation, _rayDistance, LayerMask.GetMask(PLANET))

        

        //if (0 < _playerHit.Length)
        //{
        //    foreach(RaycastHit hit in _playerHit)
        //    {
        //        _gravityScript.GravityDirection = hit.normal;
        //        Debug.LogWarning(hit.normal);
        //    }
            
            
        //    return true;
        //}

        return false;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position + transform.up * 1.6f, _bodySize);
    //}

}