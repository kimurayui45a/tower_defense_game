using UnityEngine;

public class BattleEffectManager : MonoBehaviour
{
    public static BattleEffectManager instance;

    // キャラ 用
    public GameObject destroyCharaEffectPrefab;

    // エネミー用
    public GameObject destroyEnemyEffectPrefab;
    public GameObject hitEnemyEffectPrefab;

    // 拠点用
    public GameObject hitDefenseBaseEffectPrefab;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// EffectType で指定したエフェクトのプレファブを取得
    /// </summary>
    /// <param name="effectType"></param>
    /// <returns></returns>
    public GameObject GetEffect(EffectType effectType)
    {

        // 引数の値で分岐し、該当した EffectType のエフェクトの情報を戻す
        return effectType switch
        {

            // 【チェックする effectType 変数の値】 => 【該当するエフェクトの情報】という書式
            EffectType.Destroy_Chara => destroyCharaEffectPrefab,
            EffectType.Destroy_Enemy => destroyEnemyEffectPrefab,
            EffectType.Hit_Enemy => hitEnemyEffectPrefab,
            EffectType.Hit_DefenseBase => hitDefenseBaseEffectPrefab,
            _ => destroyCharaEffectPrefab,
        };
    }
}