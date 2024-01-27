using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;

    [SerializeField] float spawnRate = 0.5f;

    public bool allowSpawn = true;
     private void Start()
    {
        StartCoroutine(Spawn());
    }



    IEnumerator Spawn()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);
        
        while (allowSpawn)
        {
            yield return wait;

            Instantiate(enemyPrefab,transform.position, Quaternion.identity);
        }
    }
}
