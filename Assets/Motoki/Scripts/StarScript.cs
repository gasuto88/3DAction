/*-------------------------------------------------
* StarScript.cs
* 
* 作成日　2024/03/15
* 更新日　2024/03/15
*
* 作成者　本木大地
-------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour 
{

	#region フィールド変数

	[SerializeField,Header("回転速度"),Range(0,2000)]
	private float _rotationSpeed = 0f;

	private Transform _myTransform = default;

	#endregion

	/// <summary>
    /// 更新前処理
    /// </summary>
	private void Start () 
	{
		_myTransform = transform;
	}
	
	public void Rotate()
    {
		_myTransform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
    }
}