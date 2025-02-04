using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController_0 : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] PlayerStatusSO playerStatusSO;
    [SerializeField] EnemyStatusSO enemyStatusSO;

    private NavMeshAgent agent;
    private Animator animator;

    private Vector3 enemyPosition;  // ���̃X�N���v�g�ŎQ�Ƃ������ϐ�

    private float speed = 3f;
    private float originalSpeed;  // ���̑��x��ێ����邽�߂̕ϐ�

    private float distance;

    //����HP
    public static float enemyCurrentHp;

    private float enemyDamage;

    //---�v���C���[�X�e�[�^�X------
    GameObject Player;
    //�v���C���[�U����
    float playerAttack;
    //-----------------------------

    //�_���[�W�\�L
    public Text damageText;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("HideDamage", 0f);

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        originalSpeed = agent.speed;  // �������x��ۑ�
        agent.speed = speed;

        //����HP
        enemyCurrentHp = enemyStatusSO.enemyStatusList[0].HP;
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[�U����
        playerAttack = PlayerController_0.Attack;

        //��_���[�W
        enemyDamage = (playerAttack - enemyStatusSO.enemyStatusList[0].DEFENCE);
        if (enemyDamage < 0)
        {
            enemyDamage = 0;
        }

        // �G�̌��ݍ��W���X�V
        enemyPosition = transform.position;

        distance = Vector3.Distance(target.position, this.transform.position);

        if (distance < 5)
        {
            if (animator.GetBool("Hit") == false && animator.GetBool("Die") == false)
            {
                agent.destination = target.position;
                animator.SetBool("Found", true);
            }
            else if (animator.GetBool("Hit") == true || animator.GetBool("Die") == true)
            {
                animator.SetBool("Found", false);
            }
        }
        else
        {
            animator.SetBool("Found", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            enemyCurrentHp -= enemyDamage;
            //�_���[�W�\��
            ShowDamage(enemyDamage, enemyPosition);

            // �ꎞ��~�������Ăяo��
            StartCoroutine(StopForSeconds(0.6f));  // 0.6�b�Ԓ�~

            if (enemyCurrentHp <= 0)
            {
                animator.SetBool("Die", true);
                Destroy(this.gameObject, 0.7f);
            }
        }
    }

    // �w�肵���b������NavMeshAgent���~������Coroutine
    private IEnumerator StopForSeconds(float seconds)
    {
        // ���x���[���ɐݒ肵�đ����ɒ�~
        agent.velocity = Vector3.zero;
        agent.isStopped = true;  // Agent���~

        // 'Found'��false�ɂ��āA���̃A�j���[�V�������Đ�����Ȃ��悤�ɂ���
        animator.SetBool("Found", false);
        if (enemyCurrentHp > 0)
        {
            // 'Hit'�A�j���[�V�������Đ�
            animator.SetBool("Hit", true);
        }

        // �����ҋ@���āA��~�����S�ɍs����̂��m���ɂ���
        yield return new WaitForSeconds(0.1f);  // �K�v�ɉ����Ĕ�����

        yield return new WaitForSeconds(seconds);  // �w��b���҂�

        // �A�j���[�V�������I��������ɁA'Hit'��false�ɖ߂��A'Found'��true�ɐݒ�
        animator.SetBool("Hit", false);

        agent.isStopped = false;  // �ĊJ
    }

    //�_���[�W�\��
    public void ShowDamage(float damage, Vector3 position)
    {
        // �e�L�X�g�̐ݒ�
        damageText.transform.position = enemyPosition + new Vector3(0, 2, 0);
        damageText.text = damage.ToString();

        // �e�L�X�g���A�N�e�B�u�ɂ��āA���b��ɔ�\���ɂ���
        damageText.gameObject.SetActive(true);
        Debug.Log("ON" + enemyPosition);

        Invoke("HideDamage", 0.8f);
    }

    void HideDamage()
    {
        damageText.gameObject.SetActive(false);
    }
}