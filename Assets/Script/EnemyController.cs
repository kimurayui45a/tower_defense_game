using UnityEngine;
using DG.Tweening;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("�ړ��o�H�̏��")]
    private PathData pathData;

    [SerializeField, Header("�ړ����x")]
    private float moveSpeed;

    // �ړ�����e�n�_�������邽�߂̔z��
    private Vector3[] paths;

    void Start()
    {
        // �ړ�����n�_���擾���邽�߂̔z��̏�����
        paths = new Vector3[pathData.pathTranArray.Length];

        // �ړ�����n�_���擾
        paths = pathData.pathTranArray.Select(x => x.position).ToArray();

        // �e�n�_�Ɍ����Ĉړ�
        transform.DOPath(paths, 1000 / moveSpeed).SetEase(Ease.Linear);
    }
}