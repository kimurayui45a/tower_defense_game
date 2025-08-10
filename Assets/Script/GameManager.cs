using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private EnemyGenerator enemyGenerator;

    [SerializeField]
    private CharaGenerator charaGenerator;

    // Enemyの生成間隔を指示する変数
    public bool isEnemyGenerate;

    public int generateIntervalTime;

    public int generateEnemyCount;

    public int maxEnemyCount;


    void Start()
    {
        // キャラ配置用ポップアップの生成と設定
        StartCoroutine(charaGenerator.SetUpCharaGenerator(this));

        isEnemyGenerate = true;

        // 敵の生成準備開始
        StartCoroutine(enemyGenerator.PreparateEnemyGenerate(this));
    }

    /// <summary>
    /// 敵の情報を List に追加
    /// </summary>
    public void AddEnemyList()
    { 
        //　TODO　敵の情報を List に追加する際に、引数を追加

        //　TODO　敵の情報を List に追加

        // 敵の生成数をカウントアップ
        generateEnemyCount++;
    }

    /// <summary>
    /// 敵の生成を停止するか判定
    /// </summary>
    public void JudgeGenerateEnemysEnd()
    {
        if (generateEnemyCount >= maxEnemyCount)
        {
            isEnemyGenerate = false;
        }
    }
}