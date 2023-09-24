using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunschManager : MonoBehaviour
{
    //Instance
    public static MunschManager Instance;

    private void Awake()
    {
        Instance = this;
    }


    [SerializeField] GameObject HimbbärePrefab;
    [SerializeField] GameObject HealendeErdbeerePrefab;
    [SerializeField] GameObject BombenBlaubärPrefab;
    [SerializeField] GameObject CoconutPrefab;

    public bool doRandomSpawn = true;

    [SerializeField] Vector2 RandomSpawnInterval;

    private void Start()
    {
        StartCoroutine(RandomSpawn());
    }
    IEnumerator RandomSpawn()
    {
        while (doRandomSpawn)
        {
            yield return new WaitForSeconds(Random.Range(RandomSpawnInterval.x, RandomSpawnInterval.y));
            PlaceSingleRandom();
        }
    }
    
    [EButton]
    public void PlaceSingleRandom()
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                PlaceSingleSpuckBareBäre();
                break;
            case 1:
                PlaceSingleHealendeErdbeere();
                break;
            case 2:
                PlaceSingleBombenBlaubär();
                break;
            default:
                break;
        }
    }

    [EButton]

    public void PlaceSingleBombenBlaubär()
    {
        //spawn HimbbärePrefab on every child of this child 2
        Transform child = transform.GetChild(2).GetChild(Random.Range(0, transform.GetChild(2).childCount));
        if (child.childCount > 0) return;
        GameObject s = Instantiate(BombenBlaubärPrefab, child.position, Quaternion.identity);
        s.transform.SetParent(child);
    }

    [EButton]
    public void PlaceSingleHealendeErdbeere()
    {
        //spawn HimbbärePrefab on every child of this child 1
        Transform child = transform.GetChild(1).GetChild(Random.Range(0, transform.GetChild(1).childCount));
        if (child.childCount > 0) return;
        GameObject s = Instantiate(HealendeErdbeerePrefab, child.position, Quaternion.identity);
        s.transform.SetParent(child);
    }


    [EButton]
    public void PlaceSingleSpuckBareBäre()
    {
        //spawn HimbbärePrefab on every child of this child 0
        Transform child = transform.GetChild(0).GetChild(Random.Range(0, transform.GetChild(0).childCount));
        if (child.childCount > 0) return;
        GameObject s = Instantiate(HimbbärePrefab, child.position, Quaternion.identity);
        s.transform.SetParent(child);
    }


    [EButton]
    public void ReplaceAllSpucks()
    {
        PlaceBombenBlaubär();
        PlaceCoconut();
        PlaceHealendeErdbeere();
        PlaceSpuckBareBäre();
    }



    [EButton]
    public void PlaceSpuckBareBäre()
    {
        //spawn HimbbärePrefab on every child of this child 0
        foreach (Transform child in transform.GetChild(0))
        {
            if (child.childCount > 0) continue;
           
            GameObject s = Instantiate(HimbbärePrefab, child.position, Quaternion.identity);
            s.transform.SetParent(child);
        }
    }

    [EButton]
    public void PlaceHealendeErdbeere()
    {
        //spawn HimbbärePrefab on every child of this child 1
        foreach (Transform child in transform.GetChild(1))
        {
            if (child.childCount > 0) continue;
            GameObject s = Instantiate(HealendeErdbeerePrefab, child.position, Quaternion.identity);
            s.transform.SetParent(child);
        }
    }
    [EButton]
    public void PlaceBombenBlaubär()
    {
        //spawn HimbbärePrefab on every child of this child 2
        foreach (Transform child in transform.GetChild(2))
        {
            if (child.childCount > 0) continue;
            GameObject s = Instantiate(BombenBlaubärPrefab, child.position, Quaternion.identity);
            s.transform.SetParent(child);
        }
    }
    [EButton]
    public void PlaceCoconut()
    {
        //spawn HimbbärePrefab on every child of this child 3
        foreach (Transform child in transform.GetChild(3))
        {
            if (child.childCount > 0) continue;
            GameObject s = Instantiate(CoconutPrefab, child.position, Quaternion.identity);
            s.transform.SetParent(child);
        }
    }
}
