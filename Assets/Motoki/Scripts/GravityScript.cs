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

	#endregion

	#region フィールド変数

	[SerializeField,Header("重力の強さ"),Range(0,100)]
	private float _gravityPower = 0f;

	[SerializeField,Header("重力の回転"),Range(0,100)]
	private float _rotationSpeed = 0f;

	[SerializeField, Header("足元の座標")]
	private Transform _legTransform = default;

	// 自分のTransform
	private Transform _myTransform = default;

	// 惑星のTransform
	private Transform _planet = default;

	// 惑星の半径
	private float _planetRadius = default;

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
		_planetRadius = _planet.localScale.x / 2;
	}

	/// <summary>
	/// 更新処理
	/// </summary>
    private void Update()
    {		
		// 着地判定
		if (!IsGround())
		{
			Gravity();
		}

		RotateGravity();
	}

	/// <summary>
	/// 重力処理
	/// </summary>
    private void Gravity()
    {
		// 惑星の方向を設定
		Vector3 direction = _planet.position - _legTransform.position;

		// 重力
		_myTransform.position += direction * _gravityPower * Time.deltaTime;
	}

	/// <summary>
	/// 重力回転処理
	/// </summary>
	private void RotateGravity()
    {
		// 惑星の方向を設定
		Vector3 direction = _planet.position - _legTransform.position;
		
		// 重力の角度を設定
		Quaternion gravityRotation 
			= Quaternion.FromToRotation(-_myTransform.up, direction) * _myTransform.rotation;

		// 角度を設定
		_myTransform.rotation = gravityRotation;
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
	private float DistanceToPlanet(Vector3 planet,Vector3 myPoint)
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
}