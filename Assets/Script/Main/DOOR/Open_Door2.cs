using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation2 : MonoBehaviour
{
    //[SerializeField] EnemyStatusSO enemyStatusSO;

    private Animator animator;

    //---“GHP------------
    GameObject Enemy;
    //“GHP
    private float enemyHp;
    //--------------------

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //“GHP
        UpdateEnemyHp();
    }

    // “G‚ÌHP‚ğXV‚·‚é
    private void UpdateEnemyHp()
    {
        // ƒV[ƒ““à‚Ì‘S‚Ä‚ÌEnemyController‚ğæ“¾
        //ENEMY1
        EnemyController_2[] enemies = FindObjectsOfType<EnemyController_2>();

        foreach (EnemyController_2 enemy in enemies)
        {
            enemyHp = enemy.EnemyCurrentHp;
        }

        if (enemyHp <= 0)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        transform.Rotate(new Vector3(0, 90, 0));
        animator.SetBool("Open", true);
    }
}