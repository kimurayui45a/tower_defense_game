using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectCharaDetail : MonoBehaviour
{
    [SerializeField]
    private Button btnSelectCharaDetail;

    [SerializeField]
    private Image imgChara;

    private PlacementCharaSelectPopUp placementCharaSelectPop;

    private CharaData charaData;

    /// <summary>
    /// SelectCharaDetail の設定
    /// </summary>
    /// <param name="placementCharaSelectPop"></param>
    /// <param name="charaData"></param>
    public void SetUpSelectCharaDetail(PlacementCharaSelectPopUp placementCharaSelectPop, CharaData charaData)
    {

        this.placementCharaSelectPop = placementCharaSelectPop;
        this.charaData = charaData;

        // ボタンを押せない状態に切り替える
        ChangeActivateButton(false);

        imgChara.sprite = this.charaData.charaSprite;


        // ボタンにメソッドを登録
        btnSelectCharaDetail.onClick.AddListener(OnClickSelectCharaDetail);


        // コストに応じてボタンを押せるかどうかを切り替える
        ChangeActivateButton(JudgePermissionCost(GameData.instance.currency));

    }

    /// <summary>
    /// SelectCharaDetail を押したの処理
    /// </summary>
    private void OnClickSelectCharaDetail()
    {

        // TODO アニメ演出

        // タップした SelectCharaDetail の情報をポップアップに送る
        // TODO 次の手順で、PlacementCharaSelectPop スクリプト内に SetSelectCharaDetail メソッドを作成するため、それまでコメントアウトしておいてください
        placementCharaSelectPop.SetSelectCharaDetail(charaData);
    }

    /// <summary>
    /// ボタンを押せる状態の切り替え
    /// </summary>
    public void ChangeActivateButton(bool isSwitch)
    {
        btnSelectCharaDetail.interactable = isSwitch;
    }

    /// <summary>
    /// コストが支払えるか確認する
    /// </summary>
    public bool JudgePermissionCost(int value)
    {

        //Debug.Log("コスト確認");

        // コストが支払える場合
        if (charaData.cost <= value)
        {

            // ボタンを押せる状態にする
            ChangeActivateButton(true);
            return true;
        }
        return false;
    }

    /// <summary>
    /// ボタンの状態の取得(今後のために実装)
    /// </summary>
    /// <returns></returns>
    public bool GetActivateButtonState()
    {
        return btnSelectCharaDetail.interactable;
    }

    /// <summary>
    /// CharaData の取得(今後のために実装)
    /// </summary>
    /// <returns></returns>
    public CharaData GetCharaData()
    {
        return charaData;
    }

}