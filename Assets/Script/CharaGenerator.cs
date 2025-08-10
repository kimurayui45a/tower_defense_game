using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharaGenerator : MonoBehaviour
{

    [SerializeField]
    private CharaController charaControllerPrefab;

    // タイルマップの座標を取得するための情報。Grid_Base 側の Grid を指定する 
    [SerializeField]
    private Grid grid;

    // Grid_Walk ゲームオブジェクトの子オブジェクトの Tilemap ゲームオブジェクトをアサインする
    [SerializeField]
    private Tilemap tilemaps;

    // PlacementCharaSelectPopUp プレファブゲームオブジェクトをアサイン用
    [SerializeField]
    private PlacementCharaSelectPopUp placementCharaSelectPopUpPrefab;

    // PlacementCharaSelectPopUp ゲームオブジェクトの生成位置の登録用
    [SerializeField]
    private Transform canvasTran;

    [SerializeField, Header("キャラのデータリスト")]
    private List<CharaData> charaDatasList = new List<CharaData>();

    // 生成された PlacementCharaSelectPopUp ゲームオブジェクトを代入するための変数
    private PlacementCharaSelectPopUp placementCharaSelectPopUp;

    private GameManager gameManager;

    // タイルマップのタイルのセル座標の保持用
    private Vector3Int gridPos;


    void Update()
    {

        // 配置できる最大キャラ数に達している場合には配置できない
        if (gameManager.GetPlacementCharaCount() >= GameData.instance.maxCharaPlacementCount)
        {
            return;
        }

        // 画面をタップ(マウスクリック)
        if (Input.GetMouseButtonDown(0) && !placementCharaSelectPopUp.gameObject.activeSelf && gameManager.currentGameState == GameManager.GameState.Play)
        {

            // タップ(マウスクリック)の位置を取得してワールド座標に変換し、それをさらにタイルのセル座標に変換
            gridPos = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            // タップした位置のタイルのコライダーの情報を確認し、それが None であるなら
            if (tilemaps.GetColliderType(gridPos) == Tile.ColliderType.None)
            {
                // キャラ生成処理をメソッド化
                //CreateChara(gridPos);

                // 配置キャラ選択用ポップアップの表示
                ActivatePlacementCharaSelectPopUp();

            }

        }
    }

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="gameManager"></param>
    /// <returns></returns>
    public IEnumerator SetUpCharaGenerator(GameManager gameManager)
    {

        this.gameManager = gameManager;

        // TODO ステージのデータを取得


        // キャラのデータをリスト化
        CreateHaveCharaDatasList();


        // キャラ配置用のポップアップの生成
        yield return StartCoroutine(CreatePlacementCharaSelectPopUp());
    }

    /// <summary>
    /// 配置キャラ選択用ポップアップ生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreatePlacementCharaSelectPopUp()
    {

        // ポップアップを生成
        placementCharaSelectPopUp = Instantiate(placementCharaSelectPopUpPrefab, canvasTran, false);

        // ポップアップの設定、キャラ設定用の情報も渡す
        placementCharaSelectPopUp.SetUpPlacementCharaSelectPopUp(this, charaDatasList);

        // ポップアップを非表示にする
        placementCharaSelectPopUp.gameObject.SetActive(false);

        yield return null;
    }

    /// <summary>
    /// 配置キャラ選択用のポップアップの表示
    /// </summary>
    public void ActivatePlacementCharaSelectPopUp()
    {

        // ゲームの進行状態をゲーム停止に変更
        gameManager.SetGameState(GameManager.GameState.Stop);

        // すべての敵の移動を一時停止
        gameManager.PauseEnemies();


        // 配置キャラ選択用のポップアップの表示
        placementCharaSelectPopUp.gameObject.SetActive(true);
        placementCharaSelectPopUp.ShowPopUp();
    }

    /// <summary>
    /// 配置キャラ選択用のポップアップの非表示
    /// </summary>
    public void InactivatePlacementCharaSelectPopUp()
    {

        // 配置キャラ選択用のポップアップの非表示
        placementCharaSelectPopUp.gameObject.SetActive(false);

        // ゲームオーバーやゲームクリアではない場合
        if (gameManager.currentGameState == GameManager.GameState.Stop)
        {

            // ゲームの進行状態をプレイ中に変更して、ゲーム再開
            gameManager.SetGameState(GameManager.GameState.Play);

            // すべての敵の移動を再開
            gameManager.ResumeEnemies();

            // カレンシーの加算処理を再開
            StartCoroutine(gameManager.TimeToCurrency());
        }

    }

    /// <summary>
    /// キャラのデータをリスト化
    /// </summary>
    private void CreateHaveCharaDatasList()
    {

        // CharaDataSO スクリプタブル・オブジェクト内の CharaData を１つずつリストに追加
        // TODO スクリプタブル・オブジェクトではなく、実際にプレイヤーが所持しているキャラの番号を元にキャラのデータのリストを作成
        for (int i = 0; i < DataBaseManager.instance.charaDataSO.charaDatasList.Count; i++)
        {
            charaDatasList.Add(DataBaseManager.instance.charaDataSO.charaDatasList[i]);
        }
    }

    /// <summary>
    /// 選択したキャラを生成して配置
    /// </summary>
    /// <param name="charaData"></param>
    public void CreateChooseChara(CharaData charaData)
    {

        // コスト支払い
        GameData.instance.currency -= charaData.cost;

        // カレンシーの画面表示を更新
        gameManager.uiManager.UpdateDisplayCurrency();

        // キャラをタップした位置に生成
        CharaController chara = Instantiate(charaControllerPrefab, gridPos, Quaternion.identity);

        // 位置が左下を 0,0 としているので、中央にくるように調整
        chara.transform.position = new Vector2(chara.transform.position.x + 0.5f, chara.transform.position.y + 0.5f);

        // キャラの設定
        chara.SetUpChara(charaData, gameManager);

        // 選択しているキャラのデータがとどいているかを確認するためのログ表示
        Debug.Log(charaData.charaName);

        // キャラを List に追加
        gameManager.AddCharasList(chara);

    }

}
