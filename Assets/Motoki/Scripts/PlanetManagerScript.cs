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

    [SerializeField, Header("重力を受ける範囲"), Range(0, 1000)]
    private float _gravityScope = 0f;

    private PlanetScript[] _planetScripts = default;

    private BlackHoleScript _blackHoleScript = default;

    #endregion

    #region プロパティ

    public PlanetScript[] PlanetScript { get => _planetScripts; set => _planetScripts = value; }

    public BlackHoleScript BlackHoleScript { 
        get => _blackHoleScript; set => _blackHoleScript = value; }

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {
        // 惑星を取得
        _planetScripts = GameObject.FindObjectsOfType<PlanetScript>();

        _blackHoleScript 
            = GameObject.FindGameObjectWithTag("BlackHole").GetComponent<BlackHoleScript>();
    }

    /// <summary>
    /// 近い惑星を設定する処理
    /// </summary>
    public PlanetScript SetNearPlanet(Vector3 position,PlanetScript nowScript,ref bool isChangePlanet)
    {
        PlanetScript scriptTemp = default;

        float nearPlanetDistance = float.MaxValue;

        int gravityIndex = 0;
        
        for (; gravityIndex < _planetScripts.Length; gravityIndex++)
        {         
            // 距離を計算
            float distance
                = DistanceToPlanet(
                    _planetScripts[gravityIndex].PlanetTransform.position, position);

            if (distance < _gravityScope + _planetScripts[gravityIndex].PlanetRadius
                && distance - _planetScripts[gravityIndex].PlanetRadius < nearPlanetDistance)
            {
                // 惑星までの距離を設定
                nearPlanetDistance = distance - _planetScripts[gravityIndex].PlanetRadius;

                scriptTemp = _planetScripts[gravityIndex];

                if(nowScript != scriptTemp)
                {
                    isChangePlanet = true;
                }
            }
        }

        return scriptTemp;
    }

    /// <summary>
    /// ブラックホールの衝突判定
    /// </summary>
    /// <param name="position">座標</param>
    /// <returns>衝突判定</returns>
    public bool isCollisionBlackHole(Vector3 position)
    {
        // 距離を計算
        float distance
            = DistanceToPlanet(
                _blackHoleScript.PlanetTransform.position, position);

        if(distance < _blackHoleScript.PlanetRadius)
        {
            return true;
        }
        return false;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    for (int i = 0; i < _planetScripts.Length; i++)
    //    {
    //        Gizmos.DrawWireSphere(
    //            _planetScripts[i].transform.position, _planetScripts[i].PlanetRadius + _gravityScope);
    //    }     
    //}

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
}