using UnityEngine;
using DG.Tweening;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("移動経路の情報")]
    private PathData pathData;

    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    // 移動する各地点を代入するための配列
    private Vector3[] paths;

    void Start()
    {
        // 移動する地点を取得するための配列の初期化
        paths = new Vector3[pathData.pathTranArray.Length];

        // 移動する地点を取得
        paths = pathData.pathTranArray.Select(x => x.position).ToArray();

        // 各地点に向けて移動
        transform.DOPath(paths, 1000 / moveSpeed).SetEase(Ease.Linear);
    }
}