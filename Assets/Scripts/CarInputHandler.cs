using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    

    Vector3 inputVector = Vector3.zero;
    float breakInput;
    //Components
    CarMovement carMovement;

    //Awake is called when the script instance is being loaded.
    void Awake()
    {
         carMovement = GetComponent<CarMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame and is frame dependent
    void Update()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        
       
        //Send the input to the car controller.
        carMovement.UpdateInput(inputVector,breakInput);
    }

    public void SetInput(Vector3 newInput)
    {
        inputVector = newInput;
    }
}
