using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController_0 : MonoBehaviour
{

    [SerializeField] PlayerStatusSO playerStatusSO;
    [SerializeField] EnemyStatusSO enemyStatusSO;

    private Rigidbody rigidbody_0;
    private Animator animator;

    private float speed = 5f;
    private float jumpspeed = 200f;

    //接地しているか
    private bool isGround;

    //方向
    private float inputHorizontal;
    private float inputVertical;

    //カメラ
    private Vector3 cameraForward;
    private Vector3 moveForward;

    //---HPゲージ---------
    GameObject HpBar;
    //Hpゲージ減少量
    float debugDamage;
    //-----------------  

    //初期HP
    private float InitiaHp;
    //現在HP
    private float CurrentHp;
    //攻撃力
    public static float Attack;
    //防御力
    private float Defence;
    //被ダメージ
    private float playerDamage;

    //---敵ステータス------------
    GameObject Enemy;
    //敵HP
    float enemyHp;
    //敵攻撃力
    float enemyAttack;
    //--------------------

    //---ステータス画面----------
    [SerializeField] GameObject statusPanel;
    [SerializeField] Text HpText;
    [SerializeField] Text AttackText;
    [SerializeField] Text DefenceText;
    //---------------------------

    public Collider WeaponCollider;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody_0 = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        //HPゲージ
        HpBar = GameObject.Find("HpGauge");

        //初期HP
        InitiaHp = playerStatusSO.HP;

        //現在HP
        CurrentHp = playerStatusSO.HP;
        //攻撃力
        Attack = playerStatusSO.ATTACK;
        //防御力
        Defence = playerStatusSO.DEFENCE;

        //敵攻撃力
        enemyAttack = enemyStatusSO.enemyStatusList[0].ATTACK;

        //武器当たり判定OFF
        WeaponCollider.enabled = false;

        //ステータス画面OFF
        statusPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //敵HP
        enemyHp = EnemyController_0.enemyCurrentHp;
        //Debug.Log(enemyHp);

        //被ダメージ
        playerDamage = (enemyAttack - Defence);
        if (playerDamage < 0)
        {
            playerDamage = 0;
        }

        //HPゲージ
        debugDamage = playerDamage / InitiaHp;

        //---ステータス画面-------------------
        HpText.GetComponent<Text>().text = CurrentHp.ToString();
        AttackText.GetComponent<Text>().text = Attack.ToString();
        DefenceText.GetComponent<Text>().text = Defence.ToString();
        //------------------------------------

        //移動処理
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        //移動アニメーション
        if (inputHorizontal != 0 || inputVertical != 0)
        {
            animator.SetBool("Run", true);
            animator.SetBool("Hit", false);
        }
        if ((inputHorizontal == 0 && inputVertical == 0) || animator.GetBool("JumpStart") || animator.GetBool("Attack"))
        {
            animator.SetBool("Run", false);
        }

        //攻撃
        //左クリック
        if (Input.GetMouseButtonDown(0) && animator.GetBool("JumpStart") == false)
        {
            animator.SetBool("Attack", true);
        }

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            animator.SetBool("Hit", false);
            animator.SetBool("Attack", false);
            rigidbody_0.AddForce(transform.up * jumpspeed, ForceMode.Force);
            animator.SetBool("JumpStart", true);
        }

        //ステータス画面
        if (Input.GetMouseButton(1))
        {
            //ステータス画面
            statusPanel.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            //ステータス画面
            statusPanel.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;
        rigidbody_0.velocity = moveForward * speed + new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);

        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
    }

    //コライダーに入った時
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            animator.SetBool("JumpStart", false);
            animator.SetBool("JumpEnd", true);

            //Debug.Log("Current State: " + animator.GetCurrentAnimatorStateInfo(0).IsName("JumpEnd"));
            //Debug.Log("GROUND TRUE");
        }

        //---ダメージ処理----------------
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (enemyHp > 0)
            {
                HpGauge hpBar = HpBar.GetComponent<HpGauge>();
                hpBar.TakeDamage(debugDamage);
                CurrentHp -= playerDamage;

                if (CurrentHp < 0)
                {
                    CurrentHp = 0;
                }

                animator.SetBool("Hit", true);
            }
        }
        //------------------------------------
    }

    //コライダーが離れた時
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = false;
            //Debug.Log("FALSE");
        }
    }

    //---武器当たり判定-------------------------------
    void WeaponON()  //当たり判定ON
    {
        WeaponCollider.enabled = true;
    }
    void WeaponOFF()  //当たり判定OFF
    {
        WeaponCollider.enabled = false;
        animator.SetBool("Attack", false);
    }
    //--------------------------------------------------

    //---- アニメーションイベントから呼び出すメソッド-------------------------
    //JumpEndアニメーション
    public void OnJumpAnimationEnd()
    {
        animator.SetBool("JumpEnd", false);
    }

    //Attackアニメーション
    public void OnAttackAnimationEnd()
    {
        animator.SetBool("Attack", false);
    }

    //Hitアニメーション
    public void OnHitAnimationEnd()
    {
        animator.SetBool("Hit", false);
    }
    //---------------------------------------------------------------------
}
