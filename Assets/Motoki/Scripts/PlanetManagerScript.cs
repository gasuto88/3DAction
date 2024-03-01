/*-------------------------------------------------
* PlanetManagerScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/03/01
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

public class PlanetManagerScript : MonoBehaviour 
{

	#region フィールド変数

	// 惑星の数
	private int _planetCount = 0;

	// 惑星
	private Transform[] _planets = default;

	private float[] _planetRadiuses = default;

	#endregion

	/// <summary>
	/// 更新前処理
	/// </summary>
	private void Start () 
	{
		Transform planet = GameObject.FindGameObjectWithTag("Planet").transform;

		// 惑星の数を設定
		_planetCount = planet.childCount;
		_planets = new Transform[_planetCount];
		_planetRadiuses = new float[_planetCount];

		for (int i = 0; i < _planetCount; i++)
		{
			// 惑星を取得
			_planets[i] = planet.GetChild(i);
		}

		for (int k = 0; k < _planetCount; k++)
		{
			// 惑星の半径を設定
			//_planetRadiuses[k] = _planets[k].localScale.x / HALF;
		}
	}

	/// <summary>
	/// 近い惑星を設定する処理
	/// </summary>
	protected void SetNearPlanet()
	{
		
			//float nearPlanetDistance = float.MaxValue;

			//int gravityIndex = -1;

			//for (gravityIndex = 0; gravityIndex < _planetCount; gravityIndex++)
			//{
			//	// 距離を計算
			//	float distance
			//		= DistanceToPlanet(_planets[gravityIndex].position, _myTransform.position);

			//	if (distance < _gravityScope + _planetRadiuses[gravityIndex]
			//		&& distance - _planetRadiuses[gravityIndex] < nearPlanetDistance)
			//	{
			//		// 惑星までの距離を設定
			//		nearPlanetDistance = distance - _planetRadiuses[gravityIndex];

			//		Debug.Log(gravityIndex);
			//	}
			//}
			//if (gravityIndex <= -1)
			//{
			//	_planet = null;
			//}
			//else if (_planet != _planets[gravityIndex])
			//{
			//	// 惑星を設定
			//	_planet = _planets[gravityIndex];

			//	// 惑星の半径を設定
			//	_planetRadius = _planetRadiuses[gravityIndex];

			//	_timerScript.TimerReset();
			//}
	}
}