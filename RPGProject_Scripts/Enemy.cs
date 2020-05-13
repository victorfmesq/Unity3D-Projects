using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float totalHealth = 20f;
    public float currentHealt;
    public float attackDamage = 3f;
    public float movementSpeed = 5f;

    private Animator _anim;
    private CapsuleCollider _cap;

    public float lookRadius = 10f;
    public Transform target;
    private NavMeshAgent _agent;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _cap = GetComponent<CapsuleCollider>();
        _agent = GetComponent<NavMeshAgent>();

        currentHealt = totalHealth;
    }

    private void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius) // movimenta o inimigo em direção ao alvo
        {
            _agent.SetDestination(target.position);
            _anim.SetInteger("Transition", 2);

            if (distance <= _agent.stoppingDistance)
            {
                // atacar
                LookTarget();
                _anim.SetInteger("Transition", 1);
            }
        }
        else
        {
            _anim.SetInteger("Transition", 0);
        }

    }

    void LookTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void GetHit(float damage)
    {
        currentHealt -= damage;
        if (currentHealt > 0)
        {
            _anim.SetInteger("Transition", 3);
            StartCoroutine(RecoveryFromHit());
        }
        else
        {
            Die();
        }
    }

    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(1f);
        _anim.SetInteger("Transition", 0);
    }

    public void Die()
    { 
        _anim.SetInteger("Transition", 4);
        _cap.enabled = false;
    }

}
