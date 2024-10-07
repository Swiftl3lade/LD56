using _Project._Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectionManager : SingletonMonobehaviour<CarSelectionManager>
{
    public CarDetails selectedCar;

    public void SelectCar(CarDetails car)
    {
        selectedCar = car;
    }

    public GameObject GetCar()
    {
        return selectedCar.playerCar;
    }

    public GameObject CreateCar(Vector3 position)
    {
        var _playerCar = Instantiate(GetCar(), position, Quaternion.identity);
        _playerCar.GetComponent<CarStats>().renderer.gameObject.GetComponent<MeshFilter>().mesh = selectedCar.color;
        return _playerCar;
    }
}
