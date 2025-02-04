using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // プレイヤーや敵のステータスを保持するScriptableObject
    [SerializeField] private PlayerStatusSO playerStatusSO;
    [SerializeField] private EnemyStatusSO enemyStatusSO;

    // HPゲージの数値を操作するためのUI
    [SerializeField] private Text currentHpText, initialHpText;

    // ステータス画面を操作するためのUI関連
    [SerializeField] private Collider weaponCollider;

    // RigidbodyやAnimatorのキャッシュ
    private new Rigidbody rigidbody;
    private Animator animator;

    // HPゲージのUIオブジェクト
    private GameObject hpBar;

    //UiControllerスクリプト
    private UiController uiController;

    // ジャンプ力のパラメータ
    private float jumpSpeed = 200f;
    private float playerDamage;

    private float knockBackDuration = 0.3f; // ノックバック時間
    private float knockBackForce = 15f; // ノックバックの強さ
    private Vector3 knockBackVelocity;

    // プレイヤーのステータス
    public static float currentHp;
    public static float initialHp;
    public static float attack;
    public static float defence;
    private float speed;

    // 敵の攻撃力やHP
    private float enemyAttack;
    private float enemyHp;
    private float enemy2Attack;
    private float enemy2Hp;
    private float enemy3Attack;
    private float enemy3Hp;
    private float enemy4Attack;
    private float enemy4Hp;
    private float enemy5Attack;
    private float enemy5Hp;

    // 接地状態の確認
    private bool isGround;

    //ノックバックフラグ
    private bool isKnockBack = false;

    // 無敵時間を管理するフラグ
    private bool isInvincible;

    //ゲームオーバーを管理するフラグ
    private bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        // RigidbodyとAnimatorのキャッシュ
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // HPゲージの参照を取得
        hpBar = GameObject.Find("HpGauge");

        //UiControllerスクリプト
        uiController = FindObjectOfType<UiController>();

        // プレイヤーのステータスを初期化
        InitializePlayerStatus();

        // 敵の攻撃力を取得
        InitializeEnemyAttack();

        // 武器の当たり判定を初期状態でオフにする
        weaponCollider.enabled = false;

        //// ステータス画面を初期状態で非表示にする
        //statusPanel.SetActive(false);
    }

    // プレイヤーの初期ステータスを設定
    private void InitializePlayerStatus()
    {
        initialHp = playerStatusSO.HP;  // 初期HP
        currentHp = initialHp;          // 現在HP
        attack = playerStatusSO.ATTACK; // 攻撃力
        defence = playerStatusSO.DEFENCE; // 防御力
        speed = playerStatusSO.SPEED; // 素早さ

        initialHpText.text = initialHp.ToString(); // HPゲージの数値

        // 無敵時間を管理するフラグ
        isInvincible = false;

        //ゲームオーバーを管理するフラグ
        isGameOver = false;
    }

    private void InitializeEnemyAttack()
    {
        // 敵の攻撃力を取得
        enemyAttack = enemyStatusSO.enemyStatusList[0].ATTACK;
        enemy2Attack = enemyStatusSO.enemyStatusList[1].ATTACK;
        enemy3Attack = enemyStatusSO.enemyStatusList[2].ATTACK;
        enemy4Attack = enemyStatusSO.enemyStatusList[3].ATTACK;
        enemy5Attack = enemyStatusSO.enemyStatusList[4].ATTACK;
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnockBack)
        {
            // 徐々にノックバックを弱める
            knockBackVelocity = Vector3.Lerp(knockBackVelocity, Vector3.zero, Time.deltaTime * 5);
            transform.position += knockBackVelocity * Time.deltaTime;
        }

        // 敵のHPを更新
        UpdateEnemyHp();

        // プレイヤーの受けるダメージを計算
        //CalculatePlayerDamage();

        // プレイヤーの攻撃処理
        HandleAttackInput();

        // プレイヤーのジャンプ処理
        HandleJumpInput();

        // UIを更新
        UpdateUI();

        //ゲームオーバー処理
        PlayerGameOver();
    }

    // 敵のHPを更新する
    private void UpdateEnemyHp()
    {
        // シーン内の全てのEnemyControllerを取得
        //ENEMY1
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        foreach (EnemyController enemy in enemies)
        {
            enemyHp = enemy.EnemyCurrentHp;
        }

        //ENEMY2
        EnemyController_1[] enemies1 = FindObjectsOfType<EnemyController_1>();

        foreach (EnemyController_1 enemy in enemies1)
        {
            enemy2Hp = enemy.EnemyCurrentHp;
        }

        //ENEMY3
        EnemyController_2[] enemies2 = FindObjectsOfType<EnemyController_2>();

        foreach (EnemyController_2 enemy in enemies2)
        {
            enemy3Hp = enemy.EnemyCurrentHp;
        }

        //ENEMY4
        EnemyController_3[] enemies3 = FindObjectsOfType<EnemyController_3>();

        foreach (EnemyController_3 enemy in enemies3)
        {
            enemy4Hp = enemy.EnemyCurrentHp;
        }

        //ENEMY5
        EnemyController_4[] enemies4 = FindObjectsOfType<EnemyController_4>();

        foreach (EnemyController_4 enemy in enemies4)
        {
            enemy5Hp = enemy.EnemyCurrentHp;
        }
    }

    // プレイヤーの移動処理
    private void HandleMovementInput()
    {
        //ゲームオーバーなら動けない
        if (isGameOver) return;

        float inputHorizontal = Input.GetAxisRaw("Horizontal");  // 水平移動入力
        float inputVertical = Input.GetAxisRaw("Vertical");      // 垂直移動入力

        // カメラの向きに基づいて移動方向を計算
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

        // プレイヤーの移動速度を設定（y軸の速度はそのまま）
        rigidbody.velocity = moveForward * speed + new Vector3(0, rigidbody.velocity.y, 0);

        // 移動方向に応じてプレイヤーの向きを変更
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }

        // 移動アニメーションの更新
        UpdateMovementAnimation(inputHorizontal, inputVertical);
    }

    // 移動アニメーションの更新
    private void UpdateMovementAnimation(float inputHorizontal, float inputVertical)
    {
        // 移動入力があるかどうか
        bool isMoving = inputHorizontal != 0 || inputVertical != 0;

        // ジャンプ中または攻撃中ではない場合のみ「Run」アニメーションをオン
        if (isMoving && !animator.GetBool("JumpStart") && !animator.GetBool("Attack"))
        {
            animator.SetBool("Run", true);
            animator.SetBool("Hit", false);  // ヒットアニメーションをオフ
        }
        else
        {
            // 移動していない、またはジャンプ中/攻撃中の場合は「Run」をオフ
            animator.SetBool("Run", false);
        }
    }

    // 攻撃の入力を処理
    private void HandleAttackInput()
    {
        //ゲームオーバーなら動けない
        if (isGameOver) return;

        if (Input.GetMouseButtonDown(0) && !animator.GetBool("JumpStart") && !animator.GetBool("Hit")) // 左クリックで攻撃開始
        {
            animator.SetBool("Attack", true);  // 攻撃アニメーションをオン
        }
    }

    // ジャンプの入力を処理
    private void HandleJumpInput()
    {
        //ゲームオーバーなら動けない
        if (isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Space) && isGround) // スペースキーでジャンプ
        {
            animator.SetBool("Hit", false);  // ヒットアニメーションをオフ
            animator.SetBool("Attack", false);  // 攻撃アニメーションをオフ
            rigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Force);  // ジャンプ力を加える
            animator.SetBool("JumpStart", true);  // ジャンプ開始アニメーションをオン
        }
    }

    // UI（HP、攻撃力、防御力）を更新
    private void UpdateUI()
    {
        currentHpText.text = currentHp.ToString();  // HPを更新
    }

    // FixedUpdateで物理演算を処理
    void FixedUpdate()
    {
        // プレイヤーの移動処理
        HandleMovementInput();

        if (isKnockBack)
        {
            rigidbody.velocity = Vector3.zero; // ノックバック中は移動を制限
        }
    }

    // 地面との衝突処理（ジャンプのため）
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = true;  // 地面に接していることを確認
            animator.SetBool("JumpStart", false);  // ジャンプ開始アニメーションをオフ
            animator.SetBool("JumpEnd", true);    // ジャンプ終了アニメーションをオン
        }

        // 敵との衝突処理（ダメージ処理）
        if (other.gameObject.CompareTag("Enemy"))
        {
            HandleEnemyCollision(other.transform.position);
        }
        else if (other.gameObject.CompareTag("Enemy2"))
        {
            HandleEnemy2Collision(other.transform.position);
        }
        else if (other.gameObject.CompareTag("Enemy3"))
        {
            HandleEnemy3Collision(other.transform.position);
        }
        else if (other.gameObject.CompareTag("Enemy4"))
        {
            HandleEnemy4Collision(other.transform.position);
        }
        else if (other.gameObject.CompareTag("Enemy5"))
        {
            HandleEnemy5Collision(other.transform.position);
        }
    }

    // プレイヤーが敵に衝突した際の処理（ダメージ）
    private void HandleEnemyCollision(Vector3 enemyPosition)
    {
        if (isInvincible) return; // 無敵状態ならダメージを受けない

        playerDamage = Mathf.Max(enemyAttack - defence, 0);  // 防御力を差し引いてダメージを計算

        if (enemyHp > 0)
        {
            HpGauge hpBarComponent = hpBar.GetComponent<HpGauge>();  // HPゲージを取得
            float debugDamage = playerDamage / initialHp;  // ダメージを割合で計算
            hpBarComponent.TakeDamage(debugDamage);  // HPゲージにダメージを与える
            currentHp -= playerDamage;  // プレイヤーのHPを減らす
            currentHp = Mathf.Max(currentHp, 0);  // HPが0未満にならないようにする

            if (!animator.GetBool("JumpStart") && !animator.GetBool("Attack"))
            {
                animator.SetBool("Hit", true);  // ヒットアニメーションをオン
            }

            ApplyKnockBack(enemyPosition);

            StartCoroutine(InvincibilityTimer(0.6f));  // 無敵時間を0.6秒設定
        }
    }

    private void HandleEnemy2Collision(Vector3 enemyPosition)
    {
        if (isInvincible) return; // 無敵状態ならダメージを受けない

        playerDamage = Mathf.Max(enemy2Attack - defence, 0);  // 防御力を差し引いてダメージを計算

        if (enemy2Hp > 0)
        {
            HpGauge hpBarComponent = hpBar.GetComponent<HpGauge>();  // HPゲージを取得
            float debugDamage = playerDamage / initialHp;  // ダメージを割合で計算
            hpBarComponent.TakeDamage(debugDamage);  // HPゲージにダメージを与える
            currentHp -= playerDamage;  // プレイヤーのHPを減らす
            currentHp = Mathf.Max(currentHp, 0);  // HPが0未満にならないようにする

            if (!animator.GetBool("JumpStart") && !animator.GetBool("Attack"))
            {
                animator.SetBool("Hit", true);  // ヒットアニメーションをオン
            }

            ApplyKnockBack(enemyPosition);

            StartCoroutine(InvincibilityTimer(0.6f));  // 無敵時間を0.6秒設定
        }
    }

    private void HandleEnemy3Collision(Vector3 enemyPosition)
    {
        if (isInvincible) return; // 無敵状態ならダメージを受けない

        playerDamage = Mathf.Max(enemy3Attack - defence, 0);  // 防御力を差し引いてダメージを計算

        if (enemy3Hp > 0)
        {
            HpGauge hpBarComponent = hpBar.GetComponent<HpGauge>();  // HPゲージを取得
            float debugDamage = playerDamage / initialHp;  // ダメージを割合で計算
            hpBarComponent.TakeDamage(debugDamage);  // HPゲージにダメージを与える
            currentHp -= playerDamage;  // プレイヤーのHPを減らす
            currentHp = Mathf.Max(currentHp, 0);  // HPが0未満にならないようにする

            if (!animator.GetBool("JumpStart") && !animator.GetBool("Attack"))
            {
                animator.SetBool("Hit", true);  // ヒットアニメーションをオン
            }

            ApplyKnockBack(enemyPosition);

            StartCoroutine(InvincibilityTimer(0.6f));  // 無敵時間を0.6秒設定
        }
    }

    private void HandleEnemy4Collision(Vector3 enemyPosition)
    {
        if (isInvincible) return; // 無敵状態ならダメージを受けない

        playerDamage = Mathf.Max(enemy4Attack - defence, 0);  // 防御力を差し引いてダメージを計算

        if (enemy4Hp > 0)
        {
            HpGauge hpBarComponent = hpBar.GetComponent<HpGauge>();  // HPゲージを取得
            float debugDamage = playerDamage / initialHp;  // ダメージを割合で計算
            hpBarComponent.TakeDamage(debugDamage);  // HPゲージにダメージを与える
            currentHp -= playerDamage;  // プレイヤーのHPを減らす
            currentHp = Mathf.Max(currentHp, 0);  // HPが0未満にならないようにする
            if (!animator.GetBool("JumpStart") && !animator.GetBool("Attack"))
            {
                animator.SetBool("Hit", true);  // ヒットアニメーションをオン
            }

            ApplyKnockBack(enemyPosition);

            StartCoroutine(InvincibilityTimer(0.6f));  // 無敵時間を0.6秒設定
        }
    }

    private void HandleEnemy5Collision(Vector3 enemyPosition)
    {
        if (isInvincible) return; // 無敵状態ならダメージを受けない

        playerDamage = Mathf.Max(enemy5Attack - defence, 0);  // 防御力を差し引いてダメージを計算

        if (enemy5Hp > 0)
        {
            HpGauge hpBarComponent = hpBar.GetComponent<HpGauge>();  // HPゲージを取得
            float debugDamage = playerDamage / initialHp;  // ダメージを割合で計算
            hpBarComponent.TakeDamage(debugDamage);  // HPゲージにダメージを与える
            currentHp -= playerDamage;  // プレイヤーのHPを減らす
            currentHp = Mathf.Max(currentHp, 0);  // HPが0未満にならないようにする
            if (!animator.GetBool("JumpStart") && !animator.GetBool("Attack"))
            {
                animator.SetBool("Hit", true);  // ヒットアニメーションをオン
            }

            ApplyKnockBack(enemyPosition);

            StartCoroutine(InvincibilityTimer(0.6f));  // 無敵時間を0.6秒設定
        }
    }

    //ノックバック処理
    private void ApplyKnockBack(Vector3 enemyPosition)
    {
        if (!isKnockBack) // すでにノックバック中でない場合のみ適用
        {
            isKnockBack = true;

            // 敵の位置を基準に、プレイヤーが反対方向にノックバックする
            Vector3 knockBackDirection = (transform.position - enemyPosition).normalized;
            knockBackDirection.y = 0;

            knockBackVelocity = knockBackDirection * knockBackForce;
            StartCoroutine(ResetKnockBack());
        }
    }

    //ゲームオーバー処理
    private void PlayerGameOver()
    {
        if(currentHp <= 0)
        {
            isGameOver = true;
            uiController.gameOver();
            
            if(isGameOver && Input.GetKeyDown(KeyCode.R))
            {
                uiController.gameReStart();
            }
        }
    }

    // 地面から離れた際の処理
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = false;  // 地面から離れたことを確認
        }
    }

    // 武器の当たり判定をオンにする
    private void WeaponON()
    {
        weaponCollider.enabled = true;  // 武器の当たり判定を有効にする
    }

    // 武器の当たり判定をオフにする
    private void WeaponOFF()
    {
        weaponCollider.enabled = false;  // 武器の当たり判定を無効にする
        animator.SetBool("Attack", false);  // 攻撃アニメーションをオフ
    }

    // アニメーションイベントから呼び出されるメソッド
    public void OnJumpAnimationEnd()
    {
        animator.SetBool("JumpEnd", false);  // ジャンプ終了アニメーションをオフ
    }

    public void OnAttackAnimationEnd()
    {
        animator.SetBool("Attack", false);  // 攻撃アニメーションをオフ
    }

    public void OnHitAnimationEnd()
    {
        animator.SetBool("Hit", false);  // ヒットアニメーションをオフ
    }

    //ノックバックリセット処理
    private IEnumerator ResetKnockBack()
    {
        yield return new WaitForSeconds(knockBackDuration);
        isKnockBack = false;
    }

    private IEnumerator InvincibilityTimer(float duration)
    {
        isInvincible = true;  // 無敵状態にする
        yield return new WaitForSeconds(duration);  // 指定時間待つ
        isInvincible = false; // 無敵解除
    }
}