using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Spine.Unity
{
    public class PlayerUnit : MonoBehaviour
    {
        [SerializeField] SkeletonAnimation playerAnim;
        [SerializeField] Transform mainTarget;
        [SerializeField] float viewDistance = 10f;
        Transform unit;
        float eps = 2.0f;
        public int HP = 50;
        public int attackEnemyFirst = 3;
        public int attackEnemySecond = 10;
        private float damageTimer = 0.5f;
        private float damageInterval = 1.0f;

        NavMeshAgent agent;
        private Player player;

        public void DamageHP(int Damage)
        {
            HP -= Damage;
            DamageUI.Instance.AddText(Damage, transform.position);

        }

        public void SetPlayer(Player player)
        {
            this.player = player;
            UpdateSkin();
        }

        private void Start()
        {
            unit = GetComponent<Transform>();
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            UpdateSkin();
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
                        DamageUI.Instance.AddText(damage, enemyTarget.transform.position);
                        if (enemyTarget.HP <= 0)
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

        public void UpdateSkin()
        {
            if (playerAnim != null)
            {
                string skinName = "p" + player.level.ToString();
                playerAnim.initialSkinName = skinName;
                playerAnim.skeleton.SetSkin(skinName);
                playerAnim.skeleton.SetSlotsToSetupPose();
                playerAnim.AnimationState.Apply(playerAnim.skeleton); // Apply the new skin
            }
        }

    }
}
