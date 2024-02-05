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

	// 自分のTransform
	private Transform _myTransform = default;

	// 惑星のTransform
	private Transform _planet = default;

	// 重力の方向
	private Vector3 _gravityDirection = default;	

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
		 _gravityDirection = _planet.position - _myTransform.position;

		// 重力
		_myTransform.position += _gravityDirection * _gravityPower * Time.deltaTime;
	}

	/// <summary>
	/// 重力回転処理
	/// </summary>
	private void RotateGravity()
    {
		// 惑星の方向を設定
		//_gravityDirection = _planet.position - _myTransform.position;

		//_myTransform.LookAt(_planet);
    }

	/// <summary>
	/// 着地判定
	/// </summary>
	private bool IsGround()
	{
		// 惑星までの距離を設定
		float distance = Vector3.Distance(_planet.position, _myTransform.position);
		
		if (distance < _planetRadius)
		{
			return true;
		}
		return false;
	}
}