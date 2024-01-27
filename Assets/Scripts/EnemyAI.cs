
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    GameObject target;
    NavMeshAgent agent;
    Animator _animation;
    Rigidbody rb;
    Health health;
    CapsuleCollider collider;
    bool isAttacking = false;


    [SerializeField] float AttackPower;
    [SerializeField] float chaseDistance;
    [SerializeField] float speed;
    private void Awake()
    {
        _animation = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        collider = GetComponent<CapsuleCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (target == null)
        {
            target = GameObject.FindWithTag("Player");
        }
        if (IsAggrevated() && !isAttacking && !health.isDead)
        {
            MoveTo();
        }
        else if (health.isDead)
        {
            agent.enabled = false;
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            collider.enabled = false;
        }
        else
        {
            agent.isStopped = true;
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

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawWireSphere(this.transform.position, chaseDistance);
    //}
}
