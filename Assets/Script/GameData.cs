using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    [Header("コスト用の通貨")]
    public int currency;

    [Header("カレンシーの最大値")]
    public int maxCurrency;

    [Header("加算までの待機時間")]
    public int currencyIntervalTime;

    [Header("加算値")]
    public int addCurrencyPoint;

    public int maxCharaPlacementCount;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}