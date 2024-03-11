/*-------------------------------------------------
* CharacterControlScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/29
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// キャラクターを制御するクラス
/// </summary>
public class CharacterControlScript : MonoBehaviour
{
    #region 定数

    // タグの名前
    private const string PLANET = "Planet";

    // アニメーション
    private const string RUN = "Run";

    #endregion

    #region フィールド変数

    [SerializeField, Header("移動速度"), Range(0, 100)]
    private float _moveMaxSpeed = 0f;

    [SerializeField, Header("振り向き速度"), Range(0, 500)]
    private float _rotationSpeed = 0f;

    [SerializeField, Header("加速速度"), Range(0, 100)]
    private float _accelSpeed = 0f;

    [SerializeField, Header("ブレーキ速度"), Range(0, 100)]
    private float _brakeSpeed = 0f;

    [SerializeField, Header("重力が最大になるまでの速度"), Range(0, 100)]
    private float _gravityMaxSpeed = 0f;

    [SerializeField, Header("重力回転速度"), Range(0, 400)]
    private float _gravityRotationSpeed = 0f;

    [SerializeField, Header("無重力時間"), Range(0, 10)]
    private float _zeroGravityCoolTime = 0f;

    [SerializeField, Header("惑星変更時間"), Range(0, 10)]
    private float _planetChangeCoolTime = 0f;

    [SerializeField,Header("足の座標")]
    protected Transform _legTransform = default;

    // 移動速度
    private float _moveSpeed = 0f;

    private float _zeroGravityTime = 0f;

    private float _planetChangeTime = default;

    // 自分のTransform
    protected Transform _myTransform = default;

    // 進行方向
    private Vector3 _moveVector = default;

    // プレイヤーの子
    protected Transform _child = default;

    // キャラクターのアニメーション
    protected Animator _characterAnimator = default;

    // 入力クラス
    protected InputScript _inputScript = default;

    private float _gravityPower = 0f;

    // 着地判定
    protected bool isGround = false;

    // 惑星変更判定
    private bool isChangePlanet = false;

    // 現在いる惑星
    protected PlanetScript _nowPlanet = default;

    protected PlanetManagerScript _planetManagerScript = default;

    private BlackHoleScript _blackHoleScript = default;

    // 重力方向
    private Vector3 _gravityDirection = Vector3.down;

    #endregion

    #region プロパティ

    public bool IsChangePlanet { get => isChangePlanet; set => isChangePlanet = value; }

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    protected virtual void Start()
    {
        _myTransform = transform;

        // 子を取得
        _child = _myTransform.Find("Avatar").transform;

        // キャラクターのアニメーションを取得
        _characterAnimator = GetComponent<Animator>();

        // Scriptを取得
        _inputScript = GetComponent<InputScript>();
        _planetManagerScript
            = GameObject.FindGameObjectWithTag("Planet")
            .GetComponent<PlanetManagerScript>();
        _blackHoleScript
            = GameObject.FindGameObjectWithTag("BlackHole")
            .GetComponent<BlackHoleScript>();

        // 無重力時間を設定
        _zeroGravityTime = _zeroGravityCoolTime;

        // 惑星変更時間
        _planetChangeTime = _planetChangeCoolTime;

        // 今いる惑星を設定
        _nowPlanet = _planetManagerScript.SetNowPlanet(
                _myTransform.position, _nowPlanet, ref isChangePlanet);
    }

    /// <summary>
    /// キャラクター制御処理
    /// </summary>
    public virtual void CharacterControl()
    {
        // ブラックホール判定
        if (_blackHoleScript.IsCollisionBlackHole(_myTransform.position))
        {
            Debug.LogError("死んだ");
            // GameManager側でゲームオーバー
            return;
        }
  
        // 今いる惑星を設定
        _nowPlanet 
            = _planetManagerScript.SetNowPlanet(
                _myTransform.position,_nowPlanet,ref isChangePlanet);

        if (_nowPlanet != null)
        {
            isGround = IsGround();
        }

        UpdateGravityDirection();
        
        if (_nowPlanet != null && !isChangePlanet)
        {
            // 重力回転
            RotateGravity();
        }
        else if (isChangePlanet)
        {
            RotateChangePlanet();

            _planetChangeTime -= Time.deltaTime;

            if(_planetChangeTime <= 0f)
            {
                IsChangePlanet = false;

                _planetChangeTime = _planetChangeCoolTime;
            }
        }      

        // 進行方向を向く
        LookForward();

        // 入力取得
        Vector2 moveInput = _inputScript.InputMove();

        if (moveInput != Vector2.zero)
        {
            MoveCalculation(moveInput);
        }
        else if (moveInput == Vector2.zero)
        {
            BrakeCalculation();
        }

        // 移動
        _myTransform.position
            += _moveVector * _moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 重力方向を更新する処理
    /// </summary>
    protected void UpdateGravityDirection()
    {
        if (_nowPlanet != null)
        {
            // 惑星の方向を設定
            _gravityDirection
                = (_nowPlanet.PlanetTransform.position - _myTransform.position).normalized;

            _zeroGravityTime = _zeroGravityCoolTime;
        }
        else if (_nowPlanet == null)
        {
            _zeroGravityTime -= Time.deltaTime;

            if (_zeroGravityTime <= 0f)
            {
                // ブラックホールの方向を設定
                _gravityDirection
                    = (_blackHoleScript.PlanetTransform.position
                    - _myTransform.position).normalized * _blackHoleScript.Gravity;
            }
        }
    }

    /// <summary>
    /// 移動計算処理
    /// </summary>
    protected void MoveCalculation(Vector2 moveInput)
    {
        if (_moveSpeed <= _moveMaxSpeed)
        {
            // 加速
            _moveSpeed += _accelSpeed * Time.deltaTime;
        }

        // 移動方向を計算
        _moveVector
            = ((_myTransform.forward * moveInput.x) + (_myTransform.right * moveInput.y));

        _characterAnimator.SetBool(RUN, true);
    }

    /// <summary>
    /// ブレーキ計算処理
    /// </summary>
    protected void BrakeCalculation()
    {
        // 減速
        _moveSpeed -= _brakeSpeed * Time.deltaTime;

        if (_moveSpeed < 0.1f)
        {
            _moveSpeed = 0f;
        }

        _characterAnimator.SetBool(RUN, false);
    }

    /// <summary>
    /// 重力落下処理
    /// </summary>
    protected void FallInGravity()
    {
        // 着地判定
        if (!isGround)
        {
            _gravityPower 
                = UpGravityPower(_gravityPower, _planetManagerScript.GravityMaxPower, _gravityMaxSpeed);

            // 重力
            _myTransform.position += _gravityDirection * _gravityPower * Time.deltaTime;
        }
        else
        {
            _gravityPower = 0f;
        }
    }

    /// <summary>
    /// 重力回転処理
    /// </summary>
    private void RotateGravity()
    {
        Quaternion gravityRotation
            = Quaternion.FromToRotation(-_myTransform.up, _gravityDirection)
            * _myTransform.rotation;

        _myTransform.rotation = gravityRotation;
    }

    /// <summary>
    /// 進行方向を向く処理
    /// </summary>
    private void LookForward()
    {
        if (_moveVector != Vector3.zero)
        {
            Quaternion forwardRotate 
                = Quaternion.LookRotation(_moveVector, -_gravityDirection);

            _child.rotation
                = Quaternion.Slerp(
                    _child.rotation, forwardRotate, _rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 惑星変更時の回転処理
    /// </summary>
    private void RotateChangePlanet()
    {
        Quaternion gravityRotation
            = Quaternion.FromToRotation(-_myTransform.up, _gravityDirection)
            * _myTransform.rotation;

        _myTransform.rotation
            = Quaternion.Slerp(_myTransform.rotation, gravityRotation,
            _gravityRotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 着地判定
    /// </summary>
    protected bool IsGround()
    {
        // 惑星までの距離を設定
        float distance
            = _planetManagerScript.DistanceToPlanet(
                _nowPlanet.transform.position, _legTransform.position);

        if(distance <= _nowPlanet.PlanetRadius)
        {
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// 重力を増やす処理
    /// </summary>
    /// <param name="power"></param>
    /// <param name="MaxPower"></param>
    /// <param name="Speed"></param>
    /// <returns></returns>
    private float UpGravityPower(float power, float MaxPower, float Speed)
    {
        if (power <= MaxPower)
        {
            power += Speed * Time.deltaTime;
        }

        return power;
    }
}