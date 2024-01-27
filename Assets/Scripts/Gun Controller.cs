using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float fireSpeed = 0.3f;
    [SerializeField] float damage = 5;
    [SerializeField] Rigidbody rb;
    [SerializeField] ParticleSystem muzzelFlashParticle;
    [SerializeField] AudioSource shotSound;
    float lastTimeShot = 0;

    private void Awake()
    {
       
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused) return;

        Ray cameraRay = GetMouseRay();
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = GetEnemyPoint(ref cameraRay, rayLength);
            //Debug.DrawLine(cameraRay.origin, pointToLook, Color.yellow);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
        if (Input.GetMouseButton(0) && lastTimeShot + fireSpeed < Time.time)
        {
            lastTimeShot = Time.time;
            Shoot();
        }
    }

    private void Shoot()
    {
        

        Bullet bullet = Instantiate(bulletPrefab, spawnPoint.position, transform.rotation).GetComponent<Bullet>();
        ParticleSystem muzzleFlash = Instantiate(muzzelFlashParticle, spawnPoint.position, spawnPoint.rotation);
        //Rigidbody rb = bullet.GetComponent<Rigidbody>();
        //rb.AddForce(spawnPoint.forward * bullet.GetComponent<Bullet>().GetbulletSpeed(), ForceMode.Impulse);
        float duration = muzzleFlash.main.duration + muzzleFlash.main.startLifetimeMultiplier;
        Destroy(muzzleFlash, duration);
        shotSound.Play();
        bullet.SetDamage(damage);

    }

    public static Vector3 GetEnemyPoint(ref Ray cameraRay, float rayLength)
    {
        return cameraRay.GetPoint(rayLength);
    }

    private Ray GetMouseRay()
    {

        return Camera.main.ScreenPointToRay(Input.mousePosition);

    }

}
