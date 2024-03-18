/*-------------------------------------------------
* GameManagerScript.cs
* 
* 作成日　2024/02/13
* 更新日　2024/03/18
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム管理クラス
/// </summary>
public class GameManagerScript : MonoBehaviour 
{

    #region 定数

    // シーンの名前
    private const string GAME_SCENE_NAME = "GameScene";
    private const string TITLE_SCENE = "TitleScene";

    // アニメーションの名前
    private const string BLACK_OUT_FLAG_NAME = "BlackOut";
    private const string WHITE_OUT_FLAG_NAME = "WhiteOut";

    #endregion

    #region フィールド変数

    private Animator _canvasAnimator = default;

    private CharacterControlScript[] _characterControlScripts = default;

    private StarScript _starScript = default;

    private GameState _gameState = GameState.EXECUTE;

    private enum GameState
    {
        EXECUTE,
        GAME_CLEAR,
        GAME_OVER

    }

	#endregion

	/// <summary>
    /// 更新前処理
    /// </summary>
	private void Start () 
	{
        _characterControlScripts = GameObject.FindObjectsOfType<CharacterControlScript>();

        _starScript = GameObject.FindGameObjectWithTag("Star").GetComponent<StarScript>();

        _canvasAnimator = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<Animator>();
    }
	
	/// <summary>
    /// 更新処理
    /// </summary>
	private void Update () 
	{
        GameControl();        
	}

    /// <summary>
    /// ゲーム状態を制御する処理
    /// </summary>
    private void GameControl()
    {
        switch (_gameState)
        {
            // 実行状態
            case GameState.EXECUTE:

                foreach (CharacterControlScript script in _characterControlScripts)
                {
                    script.CharacterControl();
                }

                _starScript.Rotate();

                break;
            // ゲームクリア状態
            case GameState.GAME_CLEAR:

                _canvasAnimator.SetBool(WHITE_OUT_FLAG_NAME, true);

                break;
            // ゲームオーバー状態
            case GameState.GAME_OVER:

                _canvasAnimator.SetBool(BLACK_OUT_FLAG_NAME, true);

                break;
        }
    }

    public void GameClear()
    {
        _gameState = GameState.GAME_CLEAR;
    }

    public void GameOver()
    {
        _gameState = GameState.GAME_OVER;
    }

    public void EndBlackOut()
    {
        SceneManager.LoadScene(GAME_SCENE_NAME);
    }

    public void EndWhiteOut()
    {
        SceneManager.LoadScene(TITLE_SCENE);
    }

}