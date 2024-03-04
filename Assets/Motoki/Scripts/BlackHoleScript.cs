/*-------------------------------------------------
* BlackHoleScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/05
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

public class BlackHoleScript : MonoBehaviour 
{

    #region フィールド変数

    [SerializeField,Header("ブラックホールの半径"),Range(0,1000)]
    private float _planetRadius = 0f;

    private Transform _planetTransform = default;

    #endregion

    #region プロパティ

    public Transform PlanetTransform { get => _planetTransform; set => _planetTransform = value; }

    public float PlanetRadius { get => _planetRadius; set => _planetRadius = value; }

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {
        // 惑星座標を設定
        _planetTransform = transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,PlanetRadius);
    }
}