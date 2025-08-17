using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtCost;

    [SerializeField]
    private Transform canvasTran;

    [SerializeField]
    private ReturnSelectCharaPopUp returnCharaPopUpPrefab;

    /// <summary>
    /// カレンシーの表示更新
    /// </summary>
    public void UpdateDisplayCurrency()
    {

        txtCost.text = GameData.instance.currency.ToString();
    }

    /// <summary>
    /// キャラの配置を解除する選択用のポップアップの生成
    /// </summary>
    public void CreateReturnCharaPopUp(CharaController charaController, GameManager gameManager)
    {
        ReturnSelectCharaPopUp returnSelectCharaPopUp = Instantiate(returnCharaPopUpPrefab, canvasTran, false);
        returnSelectCharaPopUp.SetUpReturnSelectCharaPopUp(charaController, gameManager);
    }

}