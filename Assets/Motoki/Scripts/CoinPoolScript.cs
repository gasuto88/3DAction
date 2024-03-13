/*-------------------------------------------------
* CoinPoolScript.cs
* 
* 作成日　2024/03/13
* 更新日　2024/03/13
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;
using System.Collections.Generic;

public class CoinPoolScript : MonoBehaviour 
{

    #region フィールド変数

    [SerializeField, Header("生成する数"), Range(0, 100)]
    private int _maxCount = 30;

    [SerializeField,Header("コインプレハブ")]
    private CoinScript _coinScript = default;

    private Queue<CoinScript> _ballQueue = default;

    private Vector3 _setPosition = default;

    #endregion

    #region プロパティ
    
    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {
        // Queueを初期化       
        _ballQueue = new Queue<CoinScript>();

        // 指定した数Queueに格納する
        for (int i = 0; i < _maxCount; i++)
        {
            // 弾を生成
            CoinScript tempObj
                = Instantiate(_coinScript, _setPosition, Quaternion.identity, transform).GetComponent<CoinScript>();

            // 不可視化する
            tempObj.gameObject.SetActive(false);

            // 親子関係解除
            tempObj.transform.parent = null;

            // Queueに格納
            _ballQueue.Enqueue(tempObj);
        }
    }

    /// <summary>
    /// 弾を取り出す処理
    /// </summary>
    /// <param name="position">生成する座標</param>
    public void BallOutput(Vector3 position, Quaternion rotation)
    {
        // Queueの中身が空だったら追加で作る
        if (_ballQueue.Count <= 0)
        {
            // 弾を生成
            CoinScript tempScript
                = Instantiate(_coinScript, _setPosition,
                Quaternion.identity, transform).GetComponent<CoinScript>();

            // 不可視化する
            tempScript.gameObject.SetActive(false);

            // 親子関係解除
            tempScript.transform.parent = null;

            // Queueに格納
            _ballQueue.Enqueue(tempScript);
        }

        //  オブジェクトを一つ取り出す
        _coinScript = _ballQueue.Dequeue();

        // 生成する座標を設定
        _coinScript.transform.position = position;

        // 生成する座標を設定
        _coinScript.transform.rotation = rotation;

        // オブジェクトを表示
        _coinScript.gameObject.SetActive(true);
    }

    /// <summary>
    /// 弾をしまう処理
    /// </summary>
    /// <param name="gameObject">しまう物</param>
    public void BallInput()
    {
        // オブジェクトを非表示
        _coinScript.gameObject.SetActive(false);

        // Queueに格納
        _ballQueue.Enqueue(_coinScript);
    }
}