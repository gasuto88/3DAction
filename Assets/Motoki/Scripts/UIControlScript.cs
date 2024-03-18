/*-------------------------------------------------
* UIControlScript.cs
* 
* 作成日　2024/03/15
* 更新日　2024/03/15
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI制御クラス
/// </summary>
public class UIControlScript : MonoBehaviour 
{

	#region フィールド変数

	[SerializeField,Header("Hpカウント")]
	private Text _hpText = default;

	[SerializeField,Header("HPゲージ")]
	private Image _hpGageImage = default;

	[Header("HP画像")]
	[SerializeField]
	private Sprite _zeroHpSprite = default;
	[SerializeField]
	private Sprite _oneHpSprite = default;
	[SerializeField]
	private Sprite _twoHpSprite = default;
	[SerializeField]
	private Sprite _threeHpSprite = default;

	private Sprite[] _hpSprites = new Sprite[4];

	private GameManagerScript _gameManagerScript = default;

    #endregion

	/// <summary>
	/// 更新前処理
	/// </summary>
    private void Start()
    {
		_hpSprites = new Sprite[] { _zeroHpSprite, _oneHpSprite, _twoHpSprite, _threeHpSprite };

		_gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }

	/// <summary>
	/// HPのUIを表示する処理
	/// </summary>
	/// <param name="hp">プレイヤーのHP</param>
    public void DisplayHpUI(int hp)
    {
		_hpText.text = "" + hp;

		_hpGageImage.sprite = _hpSprites[hp];
    }

	/// <summary>
	/// アニメーションの終了判定
	/// </summary>
	private void EndBlackOutAnimation()
    {
		_gameManagerScript.EndBlackOut();
    }

	private void EndWhiteOutAnimation()
    {
		_gameManagerScript.EndWhiteOut();
    }
}