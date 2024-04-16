/*-------------------------------------------------
* CharacterControlScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/04/16
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

    // アニメーションの名前
    private const string RUN_ANIMATION = "Run";

    // Layerの名前
    protected const string ENEMY_LAYER = "Enemy";

    #endregion

    #region フィールド変数

    [SerializeField, Header("移動速度"), Min(0f)]
    private float _moveMaxSpeed = 0f;

    [SerializeField, Header("振り向き速度"), Min(0f)]
    private float _rotationSpeed = 0f;

    [SerializeField, Header("加速速度"), Min(0f)]
    private float _accelSpeed = 0f;

    [SerializeField, Header("ブレーキ速度"), Min(0f)]
    private float _brakeSpeed = 0f;

    [SerializeField, Header("重力が最大になるまで加える力"), Min(0f)]
    private float _gravityAddPower = 0f;

    [SerializeField, Header("重力回転速度"), Min(0f)]
    private float _gravityRotationSpeed = 0f;

    [SerializeField, Header("無重力時間"), Min(0f)]
    private float _zeroGravityTime = 0f;

    [SerializeField, Header("惑星変更時間"), Min(0f)]
    private float _planetChangeTime = 0f;

    [SerializeField, Header("足の座標")]
    protected Transform _legTransform = default;

    [SerializeField, Header("キャラクターのHP"), Min(0f)]
    protected int _hp = 0;

    // 移動速度
    private float _moveSpeed = 0f;

    // 無重力時間(初期化用)
    private float _initZeroGravityTime = 0f;

    // 惑星変更時間(初期化用)
    private float _initPlanetChangeTime = 0f;

    // 重力の強さ
    private float _gravityPower = 0f;

    // 惑星変更判定
    private bool _isChangePlanet = false;

    // 進行方向
    private Vector3 _moveDirection = default;

    // ブラックホールクラス
    private BlackHoleScript _blackHoleScript = default;

    // 重力方向
    private Vector3 _gravityDirection = Vector3.down;

    // 自分のTransform
    protected Transform _myTransform = default;

    // キャラクターの子オブジェクト
    protected Transform _child = default;

    // キャラクターのアニメーション
    protected Animator _characterAnimator = default;

    // 入力クラス
    protected InputScript _inputScript = default;

    // 着地判定
    protected bool _isGround = false;

    // 現在いる惑星
    protected PlanetScript _nowPlanet = default;

    // 惑星管理クラス
    protected PlanetManagerScript _planetManagerScript = default;

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {
        // 自分のTransformを設定
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
        _initZeroGravityTime = _zeroGravityTime;

        // 惑星変更時間
        _initPlanetChangeTime = _planetChangeTime;

        // 今いる惑星を設定
        _nowPlanet = _planetManagerScript.SetNowPlanet(
                _myTransform.position, _nowPlanet, ref _isChangePlanet);

        // 初期化
        OnInitialize();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected virtual void OnInitialize() { }

    /// <summary>
    /// キャラクター更新処理
    /// </summary>
    public virtual void UpdateCharacter()
    {
        // 今いる惑星を設定
        _nowPlanet
            = _planetManagerScript.SetNowPlanet(
                _myTransform.position, _nowPlanet, ref _isChangePlanet);       

        if (_nowPlanet != null)
        {
            // 着地判定を設定
            _isGround = IsGround();
        }

        UpdateGravityDirection();

        // 惑星変更判定
        if (_nowPlanet != null && !_isChangePlanet)
        {
            RotateGravity();
        }
        else
        {
            RotateChangePlanet();

            _planetChangeTime -= Time.deltaTime;

            // 時間経過したら
            if (_planetChangeTime <= 0f)
            {
                // 惑星変更判定を初期化
                _isChangePlanet = false;

                // 惑星変更時間を初期化
                _planetChangeTime = _initPlanetChangeTime;
            }
        }

        // 入力取得
        Vector2 moveInput = _inputScript.InputMove();

        // 入力判定
        if (moveInput != Vector2.zero)
        {
            MoveCalculation(moveInput);

            // 走るアニメーション
            _characterAnimator.SetBool(RUN_ANIMATION, true);
        }
        else if (moveInput == Vector2.zero)
        {
            BrakeCalculation();

            _characterAnimator.SetBool(RUN_ANIMATION, false);
        }

        // 移動
        _myTransform.position
            += _moveDirection * _moveSpeed * Time.deltaTime;

        LookForward();
    }

    /// <summary>
    /// HPを減らす処理
    /// </summary>
    /// <param name="damage">ダメージ(プラスの値)</param>
    public virtual void DownHp(int damage)
    {
        _hp -= damage;
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

            // 無重力時間を初期化
            _zeroGravityTime = _initZeroGravityTime;
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
        _moveDirection
            = ((_myTransform.forward * moveInput.x) + (_myTransform.right * moveInput.y));
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
    }

    /// <summary>
    /// 重力落下処理
    /// </summary>
    protected void FallInGravity()
    {
        // 着地判定
        if (!_isGround)
        {
            // 重力をなめらかに加える
            _gravityPower
                = AddGravityPower(_gravityPower, _planetManagerScript.GravityMaxPower, _gravityAddPower);

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
        // 重力の方向にキャラクターの足が向くように回転
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
        if (_moveDirection != Vector3.zero)
        {
            // 入力された方向を向くようにキャラクターを回転
            Quaternion forwardRotate
                = Quaternion.LookRotation(_moveDirection, -_gravityDirection);

            // なめらかに回転させる
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
        // 重力の方向にキャラクターの足が向くように回転
        Quaternion gravityRotation
            = Quaternion.FromToRotation(-_myTransform.up, _gravityDirection)
            * _myTransform.rotation;

        // なめらかに回転させる
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

        // 惑星よりも内側だったら
        if (distance <= _nowPlanet.PlanetRadius)
        {
            // 着地修正
            FixGround(distance);

            return true;
        }

        return false;
    }

    /// <summary>
    /// 着地修正処理
    /// </summary>
    /// <param name="distance">惑星までの距離</param>
    private void FixGround(float distance)
    {
        // 地面にめり込んだ距離を計算
        float difference = _nowPlanet.PlanetRadius - distance;

        // めり込んだ分を修正
        _myTransform.position += difference * _myTransform.up;
    }

    /// <summary>
    /// 重力を加える処理
    /// </summary>
    /// <param name="nowPower">現在の力</param>
    /// <param name="maxPower">最大の力</param>
    /// <param name="addPower">加える力</param>
    /// <returns>重力</returns>
    private float AddGravityPower(float nowPower, float maxPower, float addPower)
    {
        float gravityPower = 0f;

        if (nowPower <= maxPower)
        {
            gravityPower = nowPower + addPower * Time.deltaTime;
        }

        return gravityPower;
    }
}