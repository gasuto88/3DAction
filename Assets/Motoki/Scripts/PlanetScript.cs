/*-------------------------------------------------
* PlanetScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/03/04
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

public class PlanetScript : MonoBehaviour 
{
    #region 定数

    // 半分
    private const int HALF = 2;

    #endregion

    #region フィールド変数

    private Transform _planetTransform = default;

    private float _planetRadius = 0f;

    #endregion

    #region プロパティ

    public Transform PlanetTransform { get => _planetTransform; set => _planetTransform = value; }

    public float PlanetRadius { get => _planetRadius; set => _planetRadius = value; }

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start () 
	{
        // 惑星座標を設定
		_planetTransform = transform;

        // 惑星の半径を設定
        _planetRadius = _planetTransform.localScale.x / HALF;
	}
}