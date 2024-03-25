/*-------------------------------------------------
* BlackHoleScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/05
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// ブラックホールクラス
/// </summary>
public class BlackHoleScript : MonoBehaviour 
{

    #region フィールド変数

    [SerializeField,Header("ブラックホールの半径"),Range(0,1000)]
    private float _planetRadius = 0f;

    [SerializeField,Header("重力"),Range(0,100)]
    private float _gravity = 0f;

    private Transform _planetTransform = default;

    private PlanetManagerScript _planetManagerScript = default;

    #endregion

    #region プロパティ

    public Transform PlanetTransform { get => _planetTransform; set => _planetTransform = value; }

    public float PlanetRadius { get => _planetRadius; set => _planetRadius = value; }

    public float Gravity { get => _gravity; set => _gravity = value; }

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {
        // 惑星座標を設定
        _planetTransform = transform;

        // Scriptを取得
        _planetManagerScript 
            = GameObject.FindGameObjectWithTag("Planet").GetComponent<PlanetManagerScript>();
    }

    /// <summary>
    /// ブラックホールの衝突判定
    /// </summary>
    /// <param name="position">座標</param>
    /// <returns>衝突判定</returns>
    public bool IsCollisionBlackHole(Vector3 position)
    {
        // 距離を計算
        float distance
            = _planetManagerScript.DistanceToPlanet(
                _planetTransform.position, position);

        if (distance < _planetRadius)
        {
            return true;
        }
        return false;
    }
}