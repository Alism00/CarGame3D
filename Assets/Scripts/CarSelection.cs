using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelection : MonoBehaviour
{

    // به خاطر کمبود وقت نتونستیم این قسمت رو به بازی اضافه کنیم 
    // be khater kambod vagt natonestam in bakhsh ro ezafe konam

    private int MaxCar;
    private int currentCar;
    
    private void Awake()
    {
        MaxCar = transform.childCount;
    }

    private void SelectCar(int _index)
    {



        for (int i = 0; i < MaxCar; i++)
        {

            transform.GetChild(i).gameObject.SetActive(_index == i);

        }

    }

    public void ChangeCar(int _change)
    {
        //if (i > MaxCar)
        //{
        //    i = 0;
        //    return;
        //}
        //if (i < 0)
        //{
        //    i = MaxCar;
        //    return;
        //}

        currentCar += _change;
        if (currentCar == MaxCar )
        {
            currentCar = 0;
            //SelectCar(currentCar);
            //return;
        }
        if (currentCar == -1)
        {
            currentCar = MaxCar - 1;
            //SelectCar(currentCar);
            //return;
        }


        print(currentCar);


        SelectCar(currentCar);
    }
}
