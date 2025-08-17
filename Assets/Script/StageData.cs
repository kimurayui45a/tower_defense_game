using UnityEngine;

[System.Serializable]
public class StageData
{
    public string stageName;              // ステージの名称
    public int stageNo;                   // ステージの通し番号
    public int generateIntervalTime;      // ステージのエネミーの出現速度

    public int clearPoint;                // ステージクリア時のボーナスポイント
    public int defenseBaseDurability;     // ステージの防衛拠点の耐久力

    public Sprite stageSprite;            // ステージの俯瞰画像

    public MapInfo mapInfo;               // TileMap で作成したステージの MainMap ゲームオブジェクトのプレファブ(MapInfo クラスがアタッチされているゲームオブジェクト)をアサインする

    public int maxCharaPlacementCount;    // マップ内に配置できるキャラの上限値

    // TODO 他にもあれば追加する

}