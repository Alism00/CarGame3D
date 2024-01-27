using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    float Damage;

    Rigidbody rb;
    Rigidbody carRb;

    //[SerializeField] AudioSource hitSound;
     public float bulletSpeed = 10f;
    [SerializeField] float livingTime = 2f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        carRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
    }
    void Start()
    {
        Destroy(gameObject, livingTime);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward *( bulletSpeed + carRb.velocity.magnitude) ;
        
    }
    public float GetbulletSpeed()
    {
        return bulletSpeed;
    }
    public void SetDamage(float damage)
    {

        this.Damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {

        
        
        if (other.transform.CompareTag("Zombie"))
        {
            other.transform.GetComponent<Health>().GetDamage(Damage);
            Destroy(gameObject);
        }

        
    }
}
