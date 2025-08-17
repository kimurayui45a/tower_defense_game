/// <summary>
/// エフェクトの種類
/// </summary>
public enum EffectType
{
    Destroy_Chara,　　　　　// 配置した味方キャラが破壊される際のエフェクト
    Destroy_Enemy,          // エネミーを破壊した際のエフェクト
    Hit_Enemy,              // 味方キャラの攻撃がエネミーにヒットした際の、エネミー側の被ダメージのエフェクト
    Hit_DefenseBase,        // 防衛拠点がエネミーの攻撃を受けた際のエフェクト

    // TODO 他にもあれば追加

}