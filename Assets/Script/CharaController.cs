using System.Collections;
using TMPro;
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

    [SerializeField]
    private int attackCount = 3;

    //[SerializeField]
    //private UnityEngine.UI.Text txtAttackCount;

    [SerializeField]
    private TextMeshProUGUI txtAttackCount;

    [SerializeField]
    private BoxCollider2D attackRangeArea;

    [SerializeField]
    private CharaData charaData;

    private GameManager gameManager;

    //private SpriteRenderer spriteRenderer;

    private Animator anim;

    private string overrideClipName = "Chara_0";

    private AnimatorOverrideController overrideController;

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

                // 攻撃回数関連の処理をここに記述する            
                attackCount--;

                // 残り攻撃回数の表示更新
                UpdateDisplayAttackCount();

                // 攻撃回数がなくなったら
                if (attackCount <= 0)
                {

                    // キャラ破壊
                    Destroy(gameObject);
                }

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

    /// <summary>
    /// 残り攻撃回数の表示更新
    /// </summary>
    private void UpdateDisplayAttackCount()
    {
        txtAttackCount.text = attackCount.ToString();
    }

    /// <summary>
    /// キャラの設定
    /// </summary>
    /// <param name="charaData"></param>
    /// <param name="gameManager"></param>
    public void SetUpChara(CharaData charaData, GameManager gameManager)
    {

        this.charaData = charaData;
        this.gameManager = gameManager;

        // 各値を CharaData から取得して設定
        attackPower = this.charaData.attackPower;

        intervalAttackTime = this.charaData.intervalAttackTime;

        // DataBaseManager に登録されている AttackRangeSizeSO スクリプタブル・オブジェクトのデータと照合を行い、CharaData の AttackRangeType の情報を元に Size を設定
        attackRangeArea.size = DataBaseManager.instance.GetAttackRangeSize(this.charaData.attackRange);

        attackCount = this.charaData.maxAttackCount;

        // キャラ画像の設定。アニメを利用するようになったら、この処理はやらない
        //if (TryGetComponent(out spriteRenderer)) {//　　<=　☆　アニメを登録するので、この一連の画像の差し替え処理の方は行わないように処理をコメントアウトします。

        // 画像を配置したキャラの画像に差し替える
        //spriteRenderer.sprite = this.charaData.charaSprite;
        //}　　　　　　

        // キャラごとの AnimationClip を設定
        SetUpAnimation();


    }

    /// <summary>
    /// キャラクターのアニメーションを設定
    /// AnimatorController の Motion に登録されている AnimationClip をキャラクターごとに異なる AnimationClip を適用できるようにするため
    /// AnimatorOverrideController を使って AnimationClip を変更
    /// </summary>
    private void SetUpAnimation()
    {

        // Chara プレハブに Animator コンポーネントがアタッチされているかを確認し、取得して anim に代入
        // if の中に入った場合、Animator が存在することが保証されているので、その後の処理を安全に実行できる
        if (TryGetComponent(out anim))
        {

            // 新しい AnimatorOverrideController を作成
            // AnimatorOverrideController は、元のアニメーションコントローラーの中のアニメーションを変更できる特別なコントローラー
            overrideController = new AnimatorOverrideController();

            // 現在の Animator のコントローラーをコピーする
            // こうすることにより、元のアニメーション設定を維持したまま、一部の AnimationClip だけを変更できるようになる
            overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;

            // Animator に新しく作成した overrideController を適用
            // これにより、overrideController の設定が有効になり、アニメーション(AnimationClip)の上書きが可能になる
            anim.runtimeAnimatorController = overrideController;

            // AnimatorStateInfo という型の配列 layerInfo を作り、各レイヤーの 現在のアニメーション状態を保存
            // anim.layerCount は、アニメーションのレイヤーの数を取得(今回は BaseLayer しかないので、1 を取得)
            AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[anim.layerCount];

            for (int i = 0; i < anim.layerCount; i++)
            {

                // anim.GetCurrentAnimatorStateInfo(i) を使うと、指定したレイヤーの現在のアニメーション情報を取得できる
                // この処理をしておくことで、アニメーションを変更した後に元のアニメーション状態を復元できる
                layerInfo[i] = anim.GetCurrentAnimatorStateInfo(i);
            }

            // 変更したいアニメーションを overrideController[overrideClipName] に設定する
            // キャラクターごとのアニメーションを charaData に登録された AnimationClip に差し替える
            overrideController[overrideClipName] = this.charaData.charaAnim;

            // Animator に overrideController を再適用
            anim.runtimeAnimatorController = overrideController;

            // anim.Update(0.0f) を呼ぶことで、Animator の状態を即座に更新し、変更を適用する
            // これがないと、変更したアニメーションがすぐに反映されない可能性がある
            anim.Update(0.0f);

            for (int i = 0; i < anim.layerCount; i++)
            {
                // もともと再生していたアニメーションを再開する
                anim.Play(layerInfo[i].fullPathHash, i, layerInfo[i].normalizedTime);
            }
        }
    }


}