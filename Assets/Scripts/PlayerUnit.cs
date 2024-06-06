using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


namespace Spine.Unity
{
    public class PlayerUnit : MonoBehaviour
    {
        [SerializeField] SkeletonAnimation playerAnim;
        [SerializeField] Transform target;
        Transform unit;
        float eps = 2.0f;
        public int HP = 100;
        public int attackEnemyFirst = 3;
        public int attackEnemySecond = 10;
        private float damageTimer = 0.5f;
        private float damageInterval = 1.0f;
        
        UnityEngine.AI.NavMeshAgent agent;

        private void Start()
        {
            unit = GetComponent<Transform>();
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        private void Update()
        {
            agent.SetDestination(target.position);

            float deltaX = Mathf.Abs(unit.position.x - target.position.x);
            float deltaY = Mathf.Abs(unit.position.y - target.position.y);

            if (deltaX < eps && deltaY < eps)
            {
                playerAnim.AnimationName = "Attack";
                // Apply damage over time
                damageTimer -= Time.deltaTime;
                if (damageTimer <= 0)
                {
                    int damage = Random.Range(attackEnemyFirst, attackEnemySecond); // Generate random damage between 5 and 10
                    HP -= damage;
                    damageTimer = damageInterval;
                }

                // Check if HP is zero
                if (HP <= 0)
                {
                    Destroy(gameObject); // Destroy the unit
                }
            }
            else
            {
                playerAnim.AnimationName = "Move";
            }
        }

    }
}