/*-------------------------------------------------
* TitleCameraScript.cs
* 
* 作成日　2024/03/15
* 更新日　2024/03/15
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// タイトルカメラクラス
/// </summary>
public class TitleCameraScript : MonoBehaviour 
{

	#region フィールド変数

	[SerializeField,Header("カメラ速度"),Range(0,100)]
	private float _cameraSpeed = 0f;

	private Transform _myTransform = default;

	#endregion

	/// <summary>
    /// 更新前処理
    /// </summary>
	private void Start () 
	{
		_myTransform = transform;
	}
	
	/// <summary>
    /// 更新処理
    /// </summary>
	private void Update () 
	{
		_myTransform.Rotate(-Vector3.one * _cameraSpeed * Time.deltaTime);
	}
}