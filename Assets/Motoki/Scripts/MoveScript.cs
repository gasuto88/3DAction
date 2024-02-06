/*-------------------------------------------------
* MoveScript.cs
* 
* 作成日　2024/02/05
* 更新日　2024/02/05
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

/// <summary>
/// 移動クラス
/// </summary>
public class MoveScript : MonoBehaviour 
{
	#region 定数

	// 地面
	private const string GROUND = "Ground";

    #endregion

    #region フィールド変数

    [SerializeField,Header("移動速度"),Range(0,100)]
	private float _moveSpeed = default;
	
	// 自分のTransform
	private Transform _myTransform = default;

	// 自分の座標
	private Vector3 _myPosition = default;

	// 入力クラス
	private InputScript _inputScript = default;

    #endregion

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start () 
	{
		// 自分のTransformを設定
		_myTransform = transform;

		// 自分の座標を設定
		_myPosition = _myTransform.position;

		// InputScriptを取得
		_inputScript = GetComponent<InputScript>();
	}
	
	/// <summary>
    /// 更新処理
    /// </summary>
	private void Update () 
	{
		Move();
	}

	/// <summary>
	/// 移動処理
	/// </summary>
	private void Move()
    {
		// 前
        if (_inputScript.IsUp())
        {
			_myTransform.position += _myTransform.forward * _moveSpeed * Time.deltaTime;
        }
		// 後
		else if (_inputScript.IsDown())
		{
			_myTransform.position -= _myTransform.forward * _moveSpeed * Time.deltaTime;
		}
		// 右
		if (_inputScript.IsRight())
		{
			_myTransform.position += _myTransform.right * _moveSpeed * Time.deltaTime;
		}
		// 左
		else if (_inputScript.IsLeft())
		{
			_myTransform.position -= _myTransform.right * _moveSpeed * Time.deltaTime;
		}
    }
}