using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PlacementCharaSelectPopUp : MonoBehaviour
{
    [SerializeField]
    private Button btnClosePopUp;

    [SerializeField]
    private Button btnChooseChara;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private CharaGenerator charaGenerator;

    [SerializeField]
    private Image imgPickupChara;

    [SerializeField]
    private TMP_Text txtPickupCharaName;

    [SerializeField]
    private TMP_Text txtPickupCharaAttackPower;

    [SerializeField]
    private TMP_Text txtPickupCharaAttackRangeType;

    [SerializeField]
    private TMP_Text txtPickupCharaCost;

    [SerializeField]
    private TMP_Text txtPickupCharaMAxAttackCount;

    // キャラのボタン用のプレファブをアサインする
    [SerializeField]
    private SelectCharaDetail selectCharaDetailPrefab;

    // キャラのボタンを生成する位置をアサインする
    [SerializeField]
    private Transform selectCharaDetailTran;

    // 生成したキャラのボタンを管理する
    [SerializeField]
    private List<SelectCharaDetail> selectCharaDetailsList = new List<SelectCharaDetail>();

    // 現在選択しているキャラの情報を管理する
    private CharaData chooseCharaData;

    /// <summary>
    /// ポップアップの設定
    /// </summary>
    /// <param name="charaGenerator"></param>
    /// <param name="haveCharaDataList"></param>
    public void SetUpPlacementCharaSelectPopUp(CharaGenerator charaGenerator, List<CharaData> haveCharaDataList)
    {

        this.charaGenerator = charaGenerator;

        // TODO 他に設定項目があったら追加する


        // ポップアップを一度見えない状態にする
        canvasGroup.alpha = 0;

        // 各ボタンの操作を押せない状態にする
        SwithcActivateButtons(false);

        // スクリプタブル・オブジェクトに登録されているキャラ分(引数で受け取った情報)を利用して
        for (int i = 0; i < haveCharaDataList.Count; i++)
        {

            // ボタンのゲームオブジェクトを生成
            SelectCharaDetail selectCharaDetail = Instantiate(selectCharaDetailPrefab, selectCharaDetailTran, false);

            // ボタンのゲームオブジェクトの設定(CharaData を設定する)
            selectCharaDetail.SetUpSelectCharaDetail(this, haveCharaDataList[i]);

            // List に追加
            selectCharaDetailsList.Add(selectCharaDetail);

            // 最初に生成したボタンの場合
            if (i == 0)
            {

                // 選択しているキャラとして初期値に設定
                SetSelectCharaDetail(haveCharaDataList[i]);
            }
        }

        // 各ボタンにメソッドを登録
        btnChooseChara.onClick.AddListener(OnClickSubmitChooseChara);

        btnClosePopUp.onClick.AddListener(OnClickClosePopUp);

        // 各ボタンを押せる状態にする
        SwithcActivateButtons(true);
    }

    /// <summary>
    /// 各ボタンのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwithcActivateButtons(bool isSwitch)
    {
        btnChooseChara.interactable = isSwitch;
        btnClosePopUp.interactable = isSwitch;
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public void ShowPopUp()
    {

        // 各キャラのボタンの制御
        CheckAllCharaButtons();

        // ポップアップの表示
        canvasGroup.DOFade(1.0f, 0.5f);
    }

    /// <summary>
    /// 選択しているキャラを配置するボタンを押した際の処理
    /// </summary>
    private void OnClickSubmitChooseChara()
    {

        // コストの支払いが可能か最終確認
        if (chooseCharaData.cost > GameData.instance.currency)
        {
            return;
        }

        // 選択しているキャラの生成
        charaGenerator.CreateChooseChara(chooseCharaData);


        // ポップアップの非表示
        HidePopUp();
    }

    /// <summary>
    /// 配置を止めるボタンを押した際の処理
    /// </summary>
    private void OnClickClosePopUp()
    {

        // ポップアップの非表示
        HidePopUp();
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    private void HidePopUp()
    {

        // 各キャラのボタンの制御
        CheckAllCharaButtons();

        // ポップアップの非表示
        canvasGroup.DOFade(0, 0.5f).OnComplete(() => charaGenerator.InactivatePlacementCharaSelectPopUp());  // 次の手順でメソッドを追加するので、それまでコメントアウトしておいてください。
    }

    /// <summary>
    /// 選択された SelectCharaDetail の情報をポップアップ内のピックアップに表示する
    /// </summary>
    /// <param name="charaData"></param>
    public void SetSelectCharaDetail(CharaData charaData)
    {

        chooseCharaData = charaData;

        // 各値の設定
        imgPickupChara.sprite = charaData.charaSprite;

        txtPickupCharaName.text = charaData.charaName;

        txtPickupCharaAttackPower.text = charaData.attackPower.ToString();

        txtPickupCharaAttackRangeType.text = charaData.attackRange.ToString();

        txtPickupCharaCost.text = charaData.cost.ToString();

        txtPickupCharaMAxAttackCount.text = charaData.maxAttackCount.ToString();

    }

    /// <summary>
    /// コストが支払えるかどうかを 各 SelectCharaDetail で確認してボタン押下機能を切り替え
    /// </summary>
    private void CheckAllCharaButtons()
    {

        // 配置できるキャラがいる場合のみ処理を行う
        if (selectCharaDetailsList.Count > 0)
        {

            // 各キャラのコストとカレンシーを確認して、配置できるかどうかを判定してボタンの押下有無を設定
            for (int i = 0; i < selectCharaDetailsList.Count; i++)
            {
                selectCharaDetailsList[i].ChangeActivateButton(selectCharaDetailsList[i].JudgePermissionCost(GameData.instance.currency));
            }
        }
    }

}