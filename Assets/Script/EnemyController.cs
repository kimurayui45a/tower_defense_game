using UnityEngine;
using DG.Tweening;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("�ړ��o�H�̏��")]
    private PathData pathData;

    [SerializeField, Header("�ړ����x")]
    private float moveSpeed;

    [SerializeField, Header("�ő�HP")]
    private int maxHp;

    [SerializeField]
    private int hp;

    private Tween tween;

    // �ړ�����e�n�_�������邽�߂̔z��
    private Vector3[] paths;

    // Animator �R���|�[�l���g�̎擾�p
    private Animator anim;

    void Start()
    {
        hp = maxHp;

        // Animator �R���|�[�l���g���擾���� anim �ϐ��ɑ��
        TryGetComponent(out anim);

        // �ړ�����n�_���擾
        paths = pathData.pathTranArray.Select(x => x.position).ToArray();

        // �e�n�_�Ɍ����Ĉړ��A���ケ�̏����𐧌䂷�邽�߁ATween �^�̕ϐ��� DOPath ���\�b�h�̏����������Ă���
        transform.DOPath(paths, 1000 / moveSpeed).SetEase(Ease.Linear).OnWaypointChange(ChangeAnimeDirection);
    }

    /// <summary>
    /// �G�̐i�s�������擾���āA�ړ��A�j���Ɠ���
    /// </summary>
    private void ChangeAnimeDirection(int index)
    {

        //Debug.Log(index);

        // ���̈ړ���̒n�_���Ȃ��ꍇ�ɂ́A�����ŏ������I������
        if (index >= paths.Length)
        {
            return;
        }

        //[�菇13�ɂȂ����炱�������g������]
        // �ڕW�̈ʒu�ƌ��݂̈ʒu�Ƃ̋����ƕ������擾���A���K���������s���A�P�ʃx�N�g���Ƃ���(�����̏��͎����A�����ɂ�鑬�x�����Ȃ����Ĉ��l�ɂ���)
        //Vector3 direction = (transform.position - paths[index]).normalized;

        // �L�����̈ړ��̕����ƈړ��A�j���̌��������΂ɂȂ�ꍇ�̑Ώ��@
        Vector3 direction = (paths[index] - transform.position).normalized;


        Debug.Log(direction);

        // �A�j���[�V������ Palameter �̒l���X�V���A�ړ��A�j���� BlendTree �𐧌䂵�Ĉړ��̕����ƈړ��A�j���𓯊�
        anim.SetFloat("X", direction.x);
        anim.SetFloat("Y", direction.y);

    }

    /// <summary>
    /// �_���[�W�v�Z
    /// </summary>
    /// <param name="amount"></param>
    public void CulcDamage(int amount)
    {

        // Hp �̒l�����Z�������ʒl���A�Œ�l�ƍő�l�͈͓̔��Ɏ��܂�悤�ɂ��čX�V
        hp = Mathf.Clamp(hp -= amount, 0, maxHp);

        Debug.Log("�c��HP : " + hp);

        // Hp �� 0 �ȉ��ɂȂ����ꍇ
        if (hp <= 0)
        {

            // �j�󏈗������s���郁�\�b�h���Ăяo��
            DestroyEnemy();
        }

        // TODO ���o�p�̃G�t�F�N�g����


        // TODO �q�b�g�X�g�b�v���o

    }

    /// <summary>
    /// �G�j�󏈗�
    /// </summary>
    public void DestroyEnemy()
    {

        // Kill ���\�b�h�����s���Atween �ϐ��ɑ������Ă��鏈��(DOPath �̏���)���I������
        tween.Kill();

        // TODO SE�̏���


        // TODO �j�󎞂̃G�t�F�N�g�̐�����֘A���鏈��


        // �G�L�����̔j��
        Destroy(gameObject);
    }

}