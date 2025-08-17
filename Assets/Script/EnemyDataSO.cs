using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Create EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public List<EnemyData> enemyDatasList = new List<EnemyData>();

    [Serializable]
    public class EnemyData
    {
        public string enemyName;      // エネミーの名前
        public int enemyNo;           // エネミーの通し番号
        public int hp;                // エネミーのHp
        public int attackPower;       // エネミーの攻撃力
        public int moveSpeed;         // エネミーの移動速度
        public EnemyType enemyType;   // エネミーのタイプ

        [Header("アイテムドロップ率")]
        public int itemDropRate;

        // エネミーの移動アニメーション
        public AnimatorOverrideController enemyOverrideController;

        // TODO 他にもあれば追加

    }
}