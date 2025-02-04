using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyStatusSO : ScriptableObject
{
    public List<EnemyStatus> enemyStatusList = new List<EnemyStatus>();

    [System.Serializable]
    public class EnemyStatus
    {
        [SerializeField] float hp;
        [SerializeField] float attack;
        [SerializeField] float defence;
        [SerializeField] float speed;

        public float HP { get => hp; set => hp = value; }
        public float ATTACK { get => attack; set => attack = value; }
        public float DEFENCE { get => defence; set => defence = value; }
        public float SPEED { get => speed; set => speed = value; }
    }
}
