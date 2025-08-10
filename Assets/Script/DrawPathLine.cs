using UnityEngine;

public class DrawPathLine : MonoBehaviour
{
    [SerializeField]
    private float startLineWidth = 0.5f;

    [SerializeField]
    private float endLineWidth = 0.5f;

    /// <summary>
    /// 経路用のライン生成
    /// </summary>
    /// <param name="drawPaths"></param>
    public void CreatePathLine(Vector3[] drawPaths) {

        TryGetComponent(out LineRenderer lineRenderer);

        // ラインの太さを調整
        lineRenderer.startWidth = startLineWidth;
        lineRenderer.endWidth = endLineWidth;

        // 生成するラインの頂点数を設定(今回は始点と終点を１つずつ)
        lineRenderer.positionCount = drawPaths.Length;

        // ラインを１つ生成
        lineRenderer.SetPositions(drawPaths);
    }
}