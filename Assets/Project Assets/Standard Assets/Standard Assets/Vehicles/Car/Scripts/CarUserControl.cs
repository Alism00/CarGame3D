using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        public GameObject handle;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }
     private void Update()
        {

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                handle.transform.Rotate(0, 0, +110 * Time.deltaTime);
            }
            if (Input.GetKeyUp(KeyCode.RightArrow)) { handle.transform.rotation = new Quaternion(0, 0, 0, 0); }
            if (Input.GetKeyUp(KeyCode.LeftArrow)) { handle.transform.rotation= new Quaternion(0,0,0,0); }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                handle.transform.Rotate(0, 0, -110 * Time.deltaTime);
            }    
        }

        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
