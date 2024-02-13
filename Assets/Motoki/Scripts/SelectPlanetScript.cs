/*-------------------------------------------------
* SelectPlanetScript.cs
* 
* 作成日　2024/02/13
* 更新日　2024/02/13
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

public class SelectPlanetScript : MonoBehaviour 
{

	#region フィールド変数

	

	// 惑星
	private Transform[] _planets = default;

	private MoveScript _moveScript = default;

	private GravityScript _gravityScript = default;

	#endregion

	/// <summary>
    /// 更新前処理
    /// </summary>
	private void Start () 
	{
		Transform planets = GameObject.FindGameObjectWithTag("Planet").transform;

		_planets = new Transform[planets.childCount];

        for (int i = 0; i < planets.childCount; i++)
        {
			_planets[i] = planets.GetChild(i);
        }

		GameObject player = GameObject.FindGameObjectWithTag("Player");

		_moveScript = player.GetComponent<MoveScript>();

		_gravityScript = player.GetComponent<GravityScript>();
	}
}