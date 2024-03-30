
using Pathfinding;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    Animator _animation;
    Rigidbody rb;
    Health health;
    Collider baseCollider;
    bool isAttacking = false;
    bool isRagDollNotActive = true;
    Collider[] ragdollCollider;

    [SerializeField] float repathRate = 0.5f;
    [SerializeField] bool reachedEndOfPath;
    [SerializeField] float AttackPower;
    [SerializeField] float chaseDistance;
    [SerializeField] float speed;
    private void Awake()
    {
        _animation = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        baseCollider = GetComponent<CapsuleCollider>();
        ragdollCollider = GetComponentsInChildren<Collider>();

        DisabaleRagdoll();
    }

    private void DisabaleRagdoll()
    {
        if (baseCollider.enabled != true)
        {
            baseCollider.enabled = true;
        }

        foreach (var collider in ragdollCollider)
        {
            if (collider.gameObject != this.gameObject)
            {
                collider.isTrigger = true;

            }
        }
    }

    private void EnableRagdoll()
    {

        //rb.useGravity = false;
        rb.velocity = Vector3.zero;
        _animation.enabled = false;
        //agent.enabled = false;
        _animation.avatar = null;
        foreach (var collider in ragdollCollider)
        {
            if (collider.gameObject != this.gameObject)
            {
                collider.isTrigger = false;


                collider.attachedRigidbody.velocity = Vector3.zero;
                //collider.attachedRigidbody.AddForce(-transform.forward * 150);
            }

        }
        isRagDollNotActive = false;
    }

    void Update()
    {



        if (health.isDead && isRagDollNotActive)
        {
            
            EnableRagdoll();
            baseCollider.enabled = false;
            agent.isStopped = true;
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;

            //this.enabled = false;

        }

        if (target == null)
        {
            target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
        if (IsAggrevated() && !isAttacking && !health.isDead)
        {
            MoveTo();
        }

        else if (!health.isDead)
        {

            rb.angularVelocity = Vector3.zero;
            //transform.rotation = Quaternion.LookRotation(transform.forward);
        }

        UpdateAnimator();
    }



    public void Hit()
    {
        Health targetHealth = target.transform.GetComponent<Health>();
        targetHealth.GetDamage(AttackPower);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isAttacking = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isAttacking = false;
        //_animation.SetBool("isAttacking", isAttacking);
    }


    private void UpdateAnimator()
    {
        Vector3 Velocity = agent.velocity;
        Vector3 LocalVelocity = transform.InverseTransformDirection(Velocity);
        float speed = LocalVelocity.z;

        _animation.SetFloat("Speed", speed);
        _animation.SetBool("isAttacking", isAttacking);

    }

    private void MoveTo()
    {
        transform.LookAt(target.transform.position);
        agent.isStopped = false;
        agent.destination = target.transform.position;
        agent.speed = speed;
    }

    private bool IsAggrevated()
    {
        float DistanceWithPlayer = Vector3.Distance(this.transform.position, target.transform.position);

        return DistanceWithPlayer < chaseDistance;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(this.transform.position, chaseDistance);
    }
}
