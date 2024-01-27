using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindActiveCar : MonoBehaviour
{

    CinemachineVirtualCamera virtualCamera;
    Transform Player;


    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

    }
    private void Update()
    {
        if (virtualCamera.m_Follow == null)
        {

            Player = GameObject.FindWithTag("Player").transform;
            virtualCamera.m_Follow = Player; 
            

        }
    }
}

