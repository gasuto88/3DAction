/*-------------------------------------------------
* GravityScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/05
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// 重力クラス
/// </summary>
public class GravityScript : MonoBehaviour
{
    #region 定数

    // 地面
    private const string GROUND = "Ground";

    // 半分
    private const int HALF = 2;

    #endregion

    #region フィールド変数

    [SerializeField, Header("重力の強さ"), Range(0, 100)]
    private float _gravityMaxPower = 0f;

    [SerializeField, Header("重力が最大になるまでの速度"), Range(0, 100)]
    private float _gravityMaxSpeed = 0f;

    [SerializeField, Header("重力回転速度"), Range(0, 100)]
    private float _gravityRotationSpeed = 0f;

    [SerializeField, Header("重力変更時の回転速度"), Range(0, 100)]
    private float _gravityChangeRotaionSpeed = 0f;

    [SerializeField, Header("重力を受ける範囲"), Range(0, 1000)]
    private float _gravityScope = 0f;

    [SerializeField,Header("重力変更時間"),Range(0,10)]
    private float _gravityChangeTime = 0f;

    private float _gravityPower = 0f;

    private int _gravityNumber = 0;

    private float _rotationSpeedTemp = 0f;

    // 自分のTransform
    private Transform _myTransform = default;

    // プレイヤーのTransform
    private Transform _player = default;

    // 惑星のTransform
    private Transform _planet = default;

    // 惑星の数
    private int _planetCount = 0;

    // 惑星の半径
    private float _planetRadius = 0f;

    // 惑星までの距離
    //private float _nearPlanetDistance = 10000f;

    // ジャンプクラス
    private JumpScript _jumpScript = default;

    // 移動クラス
    private MoveScript _moveScript = default;

    // タイマー
    private TimerScript _timerScript = default;

    // 重力方向
    private Vector3 _gravityDirection = default;

    // 惑星
    private Transform[] _planets = default;

    private float[] _planetRadiuses = default;

    private ControlPlayerScript _controlPlayerScript = default;

    #endregion

    #region プロパティ

    public float GravityPower { get => _gravityPower; set => _gravityPower = value; }

    public Vector3 GravityDirection { get => _gravityDirection; set => _gravityDirection = value; }

    public Transform Planet { get => _planet; set => _planet = value; }

    public float PlanetRadius { get => _planetRadius; set => _planetRadius = value; }

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {
        // 自分のTransformを設定
        _myTransform = transform;

        // Scriptを取得
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _jumpScript = player.GetComponent<JumpScript>();
        _moveScript = player.GetComponent<MoveScript>();
        _controlPlayerScript = player.GetComponent<ControlPlayerScript>();

        _gravityDirection = Vector3.down;
        // 惑星の数を設定
        _planetCount = _myTransform.childCount;
        _planets = new Transform[_planetCount];
        _planetRadiuses = new float[_planetCount];

        for (int i = 0; i < _planetCount; i++)
        {
            // 惑星を取得
            _planets[i] = _myTransform.GetChild(i);
        }

        for (int k = 0; k < _planetCount; k++)
        {
            // 惑星の半径を設定
            _planetRadiuses[k] = _planets[k].localScale.x / HALF;
        }

        _planet = GameObject.FindGameObjectWithTag("Planet").transform;

        _timerScript = new TimerScript(_gravityChangeTime,TimerScript.TimerState.END);

        _rotationSpeedTemp = _gravityRotationSpeed;
    }

    /// <summary>
    /// 重力処理
    /// </summary>
    public Vector3 Gravity(Vector3 targetPosition)
    {
        Vector3 gravity = default;

        // 着地判定
        if (!_controlPlayerScript.IsGround()
            && _jumpScript.JumpType != JumpScript.JumpState.JUMP)
        {
            // 惑星の方向を設定
            _gravityDirection = _planet.position - targetPosition;

            _gravityPower = UpGravityPower(_gravityPower, _gravityMaxPower, _gravityMaxSpeed);

            // 重力
            gravity += _gravityDirection * _gravityPower * Time.deltaTime;
        }
        else
        {
            _gravityPower = 0f;
        }

        return gravity;
    }

    /// <summary>
    /// 重力回転処理
    /// </summary>
    public Quaternion GravityRotate(Transform target)
    {
        // 惑星の方向を設定
        _gravityDirection = _planet.position - target.position;

        // 重力の回転を設定
        Quaternion gravityRotation
            = Quaternion.FromToRotation(-target.up, _gravityDirection) * target.rotation;

        return Quaternion.Slerp(target.rotation,gravityRotation,_gravityRotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 近い惑星を設定する処理
    /// </summary>
    public void SetNearPlanet()
    {
        if (_timerScript.Execute() == TimerScript.TimerState.END)
        {
            float nearPlanetDistance = 100000f;

            _gravityRotationSpeed = _rotationSpeedTemp;

            for (int i = 0; i < _planetCount; i++)
            {
                // 距離を計算
                float distance = DistanceToPlanet(_planets[i].position, _moveScript.MyTransform.position);

                if (distance < _gravityScope + _planetRadiuses[i])
                {
                    // 惑星までの距離を設定
                    nearPlanetDistance = distance - _planetRadiuses[i];

                    _gravityNumber = i;
                }
            }

            if (_planet != _planets[_gravityNumber])
            {
                // 惑星を設定
                _planet = _planets[_gravityNumber];

                // 惑星の半径を設定
                _planetRadius = _planetRadiuses[_gravityNumber];

                _timerScript.TimerReset();

                _gravityRotationSpeed = _gravityChangeRotaionSpeed;
            }
        }
    }

    /// <summary>
    /// 惑星までの距離を求める処理
    /// </summary>
    /// <param name="planet">惑星の中心座標</param>
    /// <param name="myPoint">自分の座標</param>
    /// <returns>距離</returns>
    public float DistanceToPlanet(Vector3 planet, Vector3 myPoint)
    {
        // 2乗計算
        float squareA = (myPoint.x - planet.x) * (myPoint.x - planet.x);
        float squareB = (myPoint.y - planet.y) * (myPoint.y - planet.y);
        float squareC = (myPoint.z - planet.z) * (myPoint.z - planet.z);

        float radiusSquare = squareA + squareB + squareC;

        // 2乗をルート化する
        float distance = Mathf.Sqrt(radiusSquare);

        return distance;
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