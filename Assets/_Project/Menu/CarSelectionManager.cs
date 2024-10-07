using _Project._Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectionManager : SingletonMonobehaviour<CarSelectionManager>
{
    public GameObject selectedCar;

    public void SelectCar(CarDetails car)
    {
        selectedCar = car.playerCar;
        selectedCar.GetComponent<CarStats>().renderer.gameObject.GetComponent<MeshFilter>().mesh = car.color;
    }

    public GameObject GetCar()
    {
        return selectedCar;
    }

    public GameObject CreateCar(Vector3 position)
    {
        var _playerCar = Instantiate(GetCar(), position, Quaternion.identity);
        return _playerCar;
    }
}
