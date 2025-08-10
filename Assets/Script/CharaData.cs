using UnityEngine;

/// <summary>
/// キャラの詳細データ
/// </summary>
[System.Serializable]
public class CharaData
{
    public string charaName;
    public int charaNo;
    public int cost;
    public Sprite charaSprite;

    public int attackPower;
    public AttackRangeType attackRange;
    public float intervalAttackTime;
    public int maxAttackCount;

    [Multiline]
    public string discription;

    public AnimationClip charaAnim;

    // TODO 他にもあれば追加

}