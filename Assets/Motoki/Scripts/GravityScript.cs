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

	[SerializeField,Header("重力の強さ"),Range(0,100)]
	private float _gravityMaxPower = 0f;

	[SerializeField, Header("重力が最大になるまでの速度"), Range(0, 100)]
	private float _gravityMaxSpeed = 0f;

	[SerializeField, Header("足元の座標")]
	private Transform _legTransform = default;

	private float _gravityPower = 0f;

	// 自分のTransform
	private Transform _myTransform = default;

	// 惑星のTransform
	private Transform _planet = default;

	// 惑星の半径
	private float _planetRadius = default;

	// ジャンプクラス
	private JumpScript _jumpScript = default;

	// 重力方向
	private Vector3 _gravityDirection = default;

	// 惑星
	private Transform[] _planets = default;

	#endregion

	#region プロパティ

	public float GravityPower { get => _gravityPower; set => _gravityPower = value; }

	public Vector3 GravityDirection { get => _gravityDirection; set => _gravityDirection = value; }

	public Transform Planet { get => _planet; set => _planet = value; }

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start () 
	{
		// 自分のTransformを設定
		_myTransform = transform;

		// 惑星を取得
		_planet = GameObject.FindGameObjectWithTag("Planet").transform;

		// 惑星の半径
		_planetRadius = _planet.localScale.x / HALF;

		_jumpScript = GetComponent<JumpScript>();

		_planets = new Transform[_planet.childCount];

		for (int i = 0; i < _planet.childCount; i++)
		{
			_planets[i] = _planet.GetChild(i);
		}
	}

	/// <summary>
	/// 重力処理
	/// </summary>
    public void Gravity()
    {
		// 着地判定
		if (!IsGround()
			&& _jumpScript.JumpType != JumpScript.JumpState.JUMP)
		{
			// 惑星の方向を設定
			_gravityDirection = _planet.position - _myTransform.position;

			_gravityPower = UpGravityPower(_gravityPower, _gravityMaxPower, _gravityMaxSpeed);

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
	public void RotateGravity()
    {
		// 惑星の方向を設定
		_gravityDirection = _planet.position - _myTransform.position;

        // 重力の角度を設定
        Quaternion gravityRotation
            = Quaternion.FromToRotation(-_myTransform.up, _gravityDirection) * _myTransform.rotation;

        // 角度を設定
        _myTransform.rotation = gravityRotation;
    }

	public void SelectPlanet()
	{
		//foreach (Transform planet in _planets)
		//{
		//	float distance = DistanceToPlanet(planet.position, _moveScript.MyTransform.position);
		//	//if (distance)
		//	//{

		//	//}
		//}
	}

	/// <summary>
	/// 着地判定
	/// </summary>
	public bool IsGround()
	{
		// 惑星までの距離を設定
		float distance = DistanceToPlanet(_planet.position,_legTransform.position);
		
		if (distance < _planetRadius)
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// 惑星までの距離を求める処理
	/// </summary>
	/// <param name="planet">惑星の中心座標</param>
	/// <param name="myPoint">自分の座標</param>
	/// <returns>距離</returns>
	public float DistanceToPlanet(Vector3 planet,Vector3 myPoint)
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

	private float UpGravityPower(float power,float MaxPower,float Speed)
    {
		if(power <= MaxPower)
        {
			power += Speed * Time.deltaTime;
		}
		
		return power;
    }
}