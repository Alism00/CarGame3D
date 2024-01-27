using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowHP : MonoBehaviour
{
    Health health;


    void Awake()
    {
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = health.GetHealthPercentage().ToString() + "%" ;
    }
}
