using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int amountAliensGreen;
    public int amountAliensGrey;
    public GameObject[] alienPrefabs;

    public Transform[] alienGroupings;

    public Transform[] farms;
    public Transform[] towns;

    // List of pooled Aliens
    private List<GameObject> aliensGreenList;
    private List<GameObject> aliensGreyList;

    enum AlienType {Green, Grey};

    void Start()
    {
        CreateAliens();
        InvokeRepeating("SpawnAliens", 1.0f, 5.0f);
    }

    void CreateAliens()
    {
        aliensGreenList = SetupAliens(AlienType.Green, amountAliensGreen, alienPrefabs[(int)AlienType.Green], alienGroupings[(int)AlienType.Green]);
        aliensGreyList = SetupAliens(AlienType.Grey, amountAliensGrey, alienPrefabs[(int)AlienType.Grey], alienGroupings[(int)AlienType.Grey]);
    }

    // Helper function to setup alien object pooling for different types
    List<GameObject> SetupAliens(AlienType _type, int _amount, GameObject _prefab, Transform _parent)
    {
        List<GameObject> alienList = ObjectPooler.CreateObjectPool(_amount, _prefab);
        ObjectPooler.AssignParentGroup(alienList, _parent);

        return alienList;
    }

    void SpawnAliens()
    {

    }
}
