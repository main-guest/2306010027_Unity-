using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private PlayerStatusSO playerStatusSO;
    [SerializeField] private EnemyStatusSO enemyStatusSO;
    [SerializeField] private Text damageText;

    private float textUpSpeed = 0.4f;
    private float damageTextDuration = 0.8f;

    private NavMeshAgent agent;
    private Animator animator;

    private float distance;
    private float enemyDamage;
    private float enemyCurrentHp;

    private bool isDamageTextActive;
    private float damageTextTime;

    //Player‚ÌHP
    private float PlayerHp;

    public float EnemyCurrentHp => enemyCurrentHp;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = enemyStatusSO.enemyStatusList[0].SPEED;
        enemyCurrentHp = enemyStatusSO.enemyStatusList[0].HP;
    }

    private void Update()
    {
        UpdatePlayerHp();
        HandleDamageText();
        HandleMovementAndAnimation();
    }

    private void UpdatePlayerHp()
    {
        PlayerHp = PlayerController.currentHp;
        if(PlayerHp <= 0)
        {
            // ‚·‚×‚Ä‚Ì Collider ‚ð–³Œø‰»
            foreach (Collider col in GetComponents<Collider>())
            {
                col.enabled = false;
            }
        }
    }

    private void HandleDamageText()
    {
        if (!isDamageTextActive) return;

        damageText.transform.position += Vector3.up * textUpSpeed * Time.deltaTime;
        damageTextTime += Time.deltaTime;

        if (damageTextTime > damageTextDuration)
        {
            HideDamageText();
        }
    }

    private void HandleMovementAndAnimation()
    {
        if (animator.GetBool("Hit") || animator.GetBool("Die"))
        {
            agent.isStopped = true;
            return;
        }

        distance = Vector3.Distance(target.position, transform.position);

        if (distance < 5)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            animator.SetBool("Found", true);
        }
        else
        {
            animator.SetBool("Found", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            ApplyDamage();
            if (enemyCurrentHp <= 0)
            {
                HandleDeath();
            }
        }
    }

    private void ApplyDamage()
    {
        enemyDamage = Mathf.Max(0, PlayerController.attack - enemyStatusSO.enemyStatusList[0].DEFENCE);
        enemyCurrentHp -= enemyDamage;

        ShowDamageText(enemyDamage);
        StartCoroutine(TemporarilyStopAgent(0.6f));
    }

    private void HandleDeath()
    {
        animator.SetBool("Die", true);

        // ‚·‚×‚Ä‚Ì Collider ‚ð–³Œø‰»
        foreach (Collider col in GetComponents<Collider>())
        {
            col.enabled = false;
        }

        StartCoroutine(HideDamageTextAfterDelay(0.6f));
        Destroy(gameObject, 0.7f);
    }

    private IEnumerator TemporarilyStopAgent(float duration)
    {
        agent.velocity = Vector3.zero;
        agent.isStopped = true;

        if (enemyCurrentHp > 0)
        {
            animator.SetBool("Hit", true);
        }

        animator.SetBool("Found", false);
        yield return new WaitForSeconds(duration);

        animator.SetBool("Hit", false);
        agent.isStopped = false;
    }

    private void ShowDamageText(float damage)
    {
        if (!isDamageTextActive)
        {
            damageText.transform.position = transform.position + Vector3.up * 2;
            damageText.text = damage.ToString();
            damageText.gameObject.SetActive(true);

            isDamageTextActive = true;
            damageTextTime = 0f;
        }
    }

    private void HideDamageText()
    {
        damageText.gameObject.SetActive(false);
        isDamageTextActive = false;
    }

    private IEnumerator HideDamageTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideDamageText();
    }
}
