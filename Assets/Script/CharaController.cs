using System.Collections;
using UnityEngine;

public class CharaController : MonoBehaviour
{

    [SerializeField, Header("攻撃力")]
    private int attackPower = 1;

    [SerializeField, Header("攻撃するまでの待機時間")]
    private float intervalAttackTime = 60.0f;

    [SerializeField]
    private bool isAttack;

    [SerializeField]
    private EnemyController enemy;

    private void OnTriggerStay2D(Collider2D collision)
    {

        // 攻撃中ではない場合で、かつ、敵の情報を未取得である場合
        if (!isAttack && !enemy)
        {

            Debug.Log("敵発見");

            // 敵の情報(EnemyController)を取得する
            // EnemyController がアタッチされているゲームオブジェクトを判別しているため、ここで、今までの Tag による判定と同じ動作で判定が行える
            if (collision.gameObject.TryGetComponent(out enemy))
            {

                // 情報を取得できたら、攻撃状態にする
                isAttack = true;

                // 攻撃の準備に入る
                StartCoroutine(PrepareteAttack());
            }

        }
    }

    /// <summary>
    /// 攻撃準備
    /// </summary>
    /// <returns></returns>
    public IEnumerator PrepareteAttack()
    {

        Debug.Log("攻撃準備開始");

        int timer = 0;

        // 攻撃中の間だけループ処理を繰り返す
        while (isAttack)
        {

            // TODO ゲームプレイ中のみ攻撃する

            timer++;

            // 攻撃のための待機時間が経過したら    
            if (timer > intervalAttackTime)
            {

                // 次の攻撃に備えて、待機時間のタイマーをリセット
                timer = 0;

                // 攻撃
                Attack();

                // TODO 攻撃回数関連の処理をここに記述する


            }

            // １フレーム処理を中断する
            // ([注意]この処理を書き忘れると無限ループになり、Unity エディターが動かなくなって再起動することになる)
            yield return null;
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {

        Debug.Log("攻撃");

        // TODO キャラの上に攻撃エフェクトを生成

        // 敵キャラ側に用意したダメージ計算用のメソッドを呼び出して、敵にダメージを与える
        enemy.CulcDamage(attackPower);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.TryGetComponent(out enemy))
        {

            Debug.Log("敵なし");

            isAttack = false;
            enemy = null;
        }
    }

}