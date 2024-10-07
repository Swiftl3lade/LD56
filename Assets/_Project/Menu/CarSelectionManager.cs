using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectionManager : SingletonMonobehaviour<CarSelectionManager>
{
    public GameObject selectedCar;

    public void SelectCar(CarDetails car)
    {
        selectedCar = car.playerCar;
    }

    public GameObject GetCar()
    {
        return selectedCar;
    }
}
