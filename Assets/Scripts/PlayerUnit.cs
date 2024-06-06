using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Spine.Unity
{
    public class PlayerUnit : MonoBehaviour
    {
        [SerializeField] SkeletonAnimation playerAnim;
        [SerializeField] Transform target;
        Transform unit;
        float eps = 2.0f;

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
                playerAnim.AnimationName = "Idle";
            }
            else
            {
                playerAnim.AnimationName = "Move";
            }
        }
    }
}