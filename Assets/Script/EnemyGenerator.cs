using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening.Core.Easing;

public class EnemyGenerator : MonoBehaviour
{
    // クローンする際に利用するプレファブのゲームオブジェクト
    [SerializeField]
    private EnemyController enemyControllerPrefab;

    [SerializeField]
    private PathData[] pathDatas;

    [SerializeField]
    private DrawPathLine pathLinePrefab;

    private GameManager gameManager;

    private StageData stageData;


    /// <summary>
    /// 敵の生成準備
    /// </summary>
    /// <returns></returns>
    public IEnumerator PreparateEnemyGenerate(GameManager gameManager, StageData stageData)
    {

        this.stageData = stageData;

        this.gameManager = gameManager;

        // 生成用のタイマー用意
        int timer = 0;

        // isEnemyGenetate が true の間はループする
        while (gameManager.isEnemyGenerate)
        {

            if (this.gameManager.currentGameState == GameManager.GameState.Play)
            {

                // タイマーを加算
                timer++;

                // タイマーの値が敵の生成待機時間を超えたら
                if (timer > gameManager.generateIntervalTime)
                {

                    // 次の生成のためにタイマーをリセット
                    timer = 0;

                    //GenerateEnemy();

                    // 敵の生成し、敵の生成数のカウントアップと List への追加
                    gameManager.AddEnemyList(GenerateEnemy());

                    // 最大生成数を超えたら生成停止
                    gameManager.JudgeGenerateEnemysEnd();
                }
            }

            // 1フレーム中断
            yield return null;
        }

        // TODO 生成終了後の処理を記述する

    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="generateNo"></param>
    /// <returns></returns>
    public EnemyController GenerateEnemy(int generateNo = 0)
    {

        //// ランダムな値を配列の最大要素数内で取得
        //int randomValue = Random.Range(0, pathDatas.Length);

        //// プレファブからエネミーのクローンを指定した位置に敵を生成
        //EnemyController enemyController = Instantiate(enemyControllerPrefab, pathDatas[randomValue].generateTran.position, Quaternion.identity);

        //// 移動する地点を取得(<=　いままでEnemyController スクリプト内で行っていた処理をこちらに移動します)
        //Vector3[] paths = pathDatas[randomValue].pathTranArray.Select(x => x.position).ToArray();

        //// 敵の種類をランダムに決定
        //int enemyNo = Random.Range(0, DataBaseManager.instance.enemyDataSO.enemyDatasList.Count);


        // 生成位置(基本的には Element の番号と同じ。-1 の場合はランダム)
        int posNo = generateNo;

        // 生成位置がランダムか確認
        if (stageData.mapInfo.appearEnemyInfos[generateNo].isRandomPos)
        {
            posNo = Random.Range(0, stageData.mapInfo.appearEnemyInfos.Length);
        }

        // 敵の生成
        EnemyController enemyController = Instantiate(enemyControllerPrefab, stageData.mapInfo.appearEnemyInfos[posNo].enemyPathData.generateTran.position, Quaternion.identity);

        // 敵の種類
        int enemyNo = stageData.mapInfo.appearEnemyInfos[generateNo].enemyNo;

        // 敵がランダムか確認
        if (stageData.mapInfo.appearEnemyInfos[generateNo].enemyNo == -1)
        {
            enemyNo = Random.Range(0, DataBaseManager.instance.enemyDataSO.enemyDatasList.Count);
        }

        // 経路の作成
        Vector3[] paths = stageData.mapInfo.appearEnemyInfos[posNo].enemyPathData.pathTranArray.Select(x => x.position).ToArray();

        // 敵の情報の設定
        enemyController.SetUpEnemyController(paths, gameManager, DataBaseManager.instance.enemyDataSO.enemyDatasList[enemyNo]);

        // 敵の移動経路のライン表示を生成の準備
        StartCoroutine(PreparateCreatePathLine(paths));

        enemyController.ResumeMove();

        return enemyController;

    }

    /// <summary>
    /// ライン生成の準備
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    private IEnumerator PreparateCreatePathLine(Vector3[] paths)
    {

        // ラインの生成と削除。この処理が終了するまでは、この処理より下の処理は実行されない
        yield return StartCoroutine(CreatePathLine(paths));

        // 現在のゲームのステートが Play になるまで処理を一時中断する = Play になるまで、敵の移動を停止したままにする
        yield return new WaitUntil(() => gameManager.currentGameState == GameManager.GameState.Play);

        // 敵の移動を再開
        //enemyController.ResumeMove();
    }

    /// <summary>
    /// 移動経路用のラインの生成と破棄
    /// </summary>
    private IEnumerator CreatePathLine(Vector3[] paths)
    {

        // List の宣言と初期化
        List<DrawPathLine> drawPathLinesList = new List<DrawPathLine>();

        // １つの Path ごとに１つずつ順番にラインを生成
        for (int i = 0; i < paths.Length - 1; i++)
        {
            DrawPathLine drawPathLine = Instantiate(pathLinePrefab, transform.position, Quaternion.identity);

            Vector3[] drawPaths = new Vector3[2] { paths[i], paths[i + 1] };

            drawPathLine.CreatePathLine(drawPaths);

            drawPathLinesList.Add(drawPathLine);

            yield return new WaitForSeconds(0.1f);
        }

        // すべてのラインを生成して待機
        yield return new WaitForSeconds(0.5f);

        // １つのラインずつ順番に削除する
        for (int i = 0; i < drawPathLinesList.Count; i++)
        {
            Destroy(drawPathLinesList[i].gameObject);

            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// ステージに応じた PathDatas をセット
    /// </summary>
    public void SetUpPathDatas(PathData[] pathDatas)
    {

        // 初期化して代入
        this.pathDatas = new PathData[pathDatas.Length];
        this.pathDatas = pathDatas;
    }

}
