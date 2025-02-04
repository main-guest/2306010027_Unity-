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

    private Vector3 enemyPosition;  // 他のスクリプトで参照したい変数

    private float speed = 3f;
    private float originalSpeed;  // 元の速度を保持するための変数

    private float distance;

    //現在HP
    public static float enemyCurrentHp;

    private float enemyDamage;

    //---プレイヤーステータス------
    GameObject Player;
    //プレイヤー攻撃力
    float playerAttack;
    //-----------------------------

    //ダメージ表記
    public Text damageText;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("HideDamage", 0f);

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        originalSpeed = agent.speed;  // 初期速度を保存
        agent.speed = speed;

        //現在HP
        enemyCurrentHp = enemyStatusSO.enemyStatusList[0].HP;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤー攻撃力
        playerAttack = PlayerController_0.Attack;

        //被ダメージ
        enemyDamage = (playerAttack - enemyStatusSO.enemyStatusList[0].DEFENCE);
        if (enemyDamage < 0)
        {
            enemyDamage = 0;
        }

        // 敵の現在座標を更新
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
            //ダメージ表示
            ShowDamage(enemyDamage, enemyPosition);

            // 一時停止処理を呼び出し
            StartCoroutine(StopForSeconds(0.6f));  // 0.6秒間停止

            if (enemyCurrentHp <= 0)
            {
                animator.SetBool("Die", true);
                Destroy(this.gameObject, 0.7f);
            }
        }
    }

    // 指定した秒数だけNavMeshAgentを停止させるCoroutine
    private IEnumerator StopForSeconds(float seconds)
    {
        // 速度をゼロに設定して即座に停止
        agent.velocity = Vector3.zero;
        agent.isStopped = true;  // Agentを停止

        // 'Found'をfalseにして、他のアニメーションが再生されないようにする
        animator.SetBool("Found", false);
        if (enemyCurrentHp > 0)
        {
            // 'Hit'アニメーションを再生
            animator.SetBool("Hit", true);
        }

        // 少し待機して、停止が完全に行われるのを確実にする
        yield return new WaitForSeconds(0.1f);  // 必要に応じて微調整

        yield return new WaitForSeconds(seconds);  // 指定秒数待つ

        // アニメーションが終了した後に、'Hit'をfalseに戻し、'Found'をtrueに設定
        animator.SetBool("Hit", false);

        agent.isStopped = false;  // 再開
    }

    //ダメージ表示
    public void ShowDamage(float damage, Vector3 position)
    {
        // テキストの設定
        damageText.transform.position = enemyPosition + new Vector3(0, 2, 0);
        damageText.text = damage.ToString();

        // テキストをアクティブにして、数秒後に非表示にする
        damageText.gameObject.SetActive(true);
        Debug.Log("ON" + enemyPosition);

        Invoke("HideDamage", 0.8f);
    }

    void HideDamage()
    {
        damageText.gameObject.SetActive(false);
    }
}