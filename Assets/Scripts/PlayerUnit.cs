using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Mirror;
using static UnityEngine.GraphicsBuffer;


namespace Spine.Unity
{
    public class PlayerUnit : NetworkBehaviour
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
        LayerMask targetLayer = 1 << 3;
        [SyncVar] private Player player;
        [SyncVar] private EnemyUnit currentTarget;

        [ClientRpc]
        public void DamageHP(int Damage)
        {
            HP -= Damage;
            if (HP <= 0)
            {
                // Ensure destruction logic is handled on the server
                if (isServer)
                {
                    NetworkServer.Destroy(this.gameObject);
                    player.playerUnits.Remove(this);
                }
            }
        }
        [Server]
        public void SetPlayer(Player player)
        {
            this.player = player;
            UpdateSkin();
        }
        [Server]
        private void Start()
        {
            unit = GetComponent<Transform>();
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            UpdateSkin();
        }

        [Server]
        private void Update()
        {
            if (currentTarget == null || (currentTarget != null && currentTarget.HP <= 0))
            {                
                currentTarget = FindFirstObjectByType<EnemyUnit>();
            }

            if (currentTarget != null)
            {
                Transform visibleTarget = currentTarget.transform;
                agent.SetDestination(visibleTarget.position);
                float deltaX = Mathf.Abs(unit.position.x - visibleTarget.position.x);
                float deltaY = Mathf.Abs(unit.position.y - visibleTarget.position.y);
                if (deltaX < eps && deltaY < eps)
                {
                    RpcPlayAnimation("Attack");
                    // Apply damage over time
                    damageTimer -= Time.deltaTime;
                    if (damageTimer <= 0)
                    {
                        int damage = Random.Range(attackEnemyFirst, attackEnemySecond);
                        if (currentTarget.HP - damage <= 0) { player.AddMoney(20); }
                        currentTarget.DamageHP(damage);
                        damageTimer = damageInterval;
                        
                    }
                }
                else
                {
                    RpcPlayAnimation("Move");
                }
            }
            else
            {
                agent.SetDestination(mainTarget.position);
                RpcPlayAnimation("Move");
            }
        }



        [ClientRpc]
        void RpcPlayAnimation(string animationName)
        {
            playerAnim.AnimationName = animationName;
        }
        [Server]
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