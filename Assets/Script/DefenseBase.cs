using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBase : MonoBehaviour
{
    [SerializeField, Header("耐久値")]
    private int maxDefenseBaseDurability;

    // 耐久力の現在値
    private int defenseBaseDurability;

    private GameManager gameManager;

    private UIManager uiManager;

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpDefenseBase(GameManager gameManager, int defenseBaseDurability, UIManager uiManager)
    {

        this.gameManager = gameManager;
        this.uiManager = uiManager;

        // 耐久力の最大値を決定する
        // デバッグモードを適用している場合
        if (GameData.instance.isDebug)
        {

            // GameData に設定している defenseBaseDurability を利用する
            maxDefenseBaseDurability = GameData.instance.defenseBaseDurability;
        }

        // 耐久力の初期値の設定
        this.defenseBaseDurability = maxDefenseBaseDurability;
    }


    // TODO 設定用のメソッドの作成。作成後は Start メソッドを削除


    private void OnTriggerEnter2D(Collider2D collision)
    {

        // 侵入してきたゲームオブジェクトの確認と敵キャラの情報の取得
        if (collision.gameObject.TryGetComponent(out EnemyController enemyController))
        {

            // 敵キャラの攻撃力分だけ耐久力を減算し、耐久力の値の下限と上限内に収まるように制御した上で更新
            defenseBaseDurability = Mathf.Clamp(defenseBaseDurability - enemyController.attackPower, 0, maxDefenseBaseDurability);

            // TODO ダメージ演出生成
            CreateDamageEffect();

            // TODO ゲーム画面に耐久力の表示がある場合、その表示を更新


            // 耐久力の残りを確認
            if (defenseBaseDurability <= 0 && gameManager.currentGameState == GameManager.GameState.Play)
            {

                Debug.Log("Game Over");

                // TODO ゲームオーバー処理

            }

            // 敵の破壊
            enemyController.DestroyEnemy();
        }
    }

    /// <summary>
    /// ダメージ演出生成
    /// </summary>
    private void CreateDamageEffect()
    {

        GameObject effect = Instantiate(BattleEffectManager.instance.GetEffect(EffectType.Hit_DefenseBase), transform.position, Quaternion.identity);

        Destroy(effect, 1.5f);
    }

}