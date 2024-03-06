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

    #endregion

    #region プロパティ

    public PlanetScript[] PlanetScript {
        get => _planetScripts; set => _planetScripts = value; }

    #endregion

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        // 惑星を取得
        _planetScripts = GameObject.FindObjectsOfType<PlanetScript>();
    }

    /// <summary>
    /// 今いる惑星を設定する処理
    /// </summary>
    /// <param name="position">現在座標</param>
    /// <param name="nowScript">今いる惑星</param>
    /// <param name="isChangePlanet">惑星変更判定</param>
    /// <returns>今いる惑星</returns>
    public PlanetScript SetNowPlanet(
        Vector3 position,PlanetScript nowScript,ref bool isChangePlanet)
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

                // 惑星を設定
                scriptTemp = _planetScripts[gravityIndex];

                if(nowScript != scriptTemp)
                {
                    // 惑星変更判定
                    isChangePlanet = true;
                }
            }
        }

        return scriptTemp;
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