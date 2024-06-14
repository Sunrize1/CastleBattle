using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


namespace Spine.Unity
{
    public class PlayerUnit : MonoBehaviour
    {
        [SerializeField] SkeletonAnimation playerAnim;
        [SerializeField] Transform mainTarget;
        [SerializeField] float viewDistance = 10f;
        Transform unit;
        float eps = 2.0f;
        public int HP = 100;
        public int attackEnemyFirst = 3;
        public int attackEnemySecond = 10;
        private float damageTimer = 0.5f;
        private float damageInterval = 1.0f;

        NavMeshAgent agent;
        LayerMask targetLayer = 1 << 3;
        private Player player;

        public void DamageHP(int Damage)
        {
            HP -= Damage;
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        private void Start()
        {
            unit = GetComponent<Transform>();
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }


        private void Update()
        {
            EnemyUnit enemyTarget = FindFirstObjectByType<EnemyUnit>();

            if (enemyTarget != null && enemyTarget != this)
            {
                Transform visibleTarget = enemyTarget.transform;
                agent.SetDestination(visibleTarget.position);
                float deltaX = Mathf.Abs(unit.position.x - visibleTarget.position.x);
                float deltaY = Mathf.Abs(unit.position.y - visibleTarget.position.y);
                if (deltaX < eps && deltaY < eps)
                {
                    playerAnim.AnimationName = "Attack";
                    // Apply damage over time
                    damageTimer -= Time.deltaTime;
                    if (damageTimer <= 0)
                    {
                        int damage = Random.Range(attackEnemyFirst, attackEnemySecond);
                        enemyTarget.DamageHP(damage);
                        damageTimer = damageInterval;
                        if(enemyTarget.HP < 0)
                        {
                            Destroy(enemyTarget.gameObject);
                            player.AddMoney(10);
                        }
                    }
                }
                else
                {
                    playerAnim.AnimationName = "Move";
                }
            }
            else
            {
                agent.SetDestination(mainTarget.position);
                playerAnim.AnimationName = "Move";
            }
        }

        EnemyUnit FindFirstObjectByType<T>() where T : EnemyUnit
        {
            T[] targets = FindObjectsOfType<T>();
            foreach (T target in targets)
            {
                if (target != this)
                {
                    float distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance <= viewDistance)
                    {
                        return target;
                    }
                }
            }
            return null;
        }

        public void SetMainTarget(Transform target)
        {
            mainTarget = target;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, viewDistance);
        }

    }
}