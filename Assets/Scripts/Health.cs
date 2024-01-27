using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
   
    [SerializeField] float maxHealth;
    [SerializeField] float health;
    [SerializeField] GameObject damagingParticle;
    [SerializeField] Transform damagingParticlePlace;
    [SerializeField] GameObject gameOver;
    public static int deadCount ;
    [SerializeField] AudioClip hitSound;


    CarMovement carMovement;
    Animator animator;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage(float damage)
    { 
      
       
        health = Mathf.Max(health - damage, 0);
        if (damagingParticle != null)
        {
            Instantiate(damagingParticle, damagingParticlePlace.position, Quaternion.identity);
        }
        if (health <= 0)
        {
           
            Death();
        }

    }



    public void Death()
    {

        DeathAnimation();
        isDead = true;
        
        Destroy(gameObject,3);
    }

    public void DeathAnimation()
    {
        if (CompareTag("Zombie"))
        {
            deadCount++;
            animator.SetBool("isDead", true);
        }
        else if (CompareTag("Player"))
        {
            gameOver.SetActive(true);
            carMovement.GetComponent<CarMovement>().enabled = false;
        }


    }

    public float GetHealthPercentage()
    {
        return health/maxHealth * 100;
    }
}
