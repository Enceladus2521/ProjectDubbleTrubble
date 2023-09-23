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


    [EButton]

    public void ReplaceAllSpucks()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            foreach (Transform child in transform.GetChild(i))
            {
                int breakCounter = 0;
                if (Application.isEditor)
                {
                    DestroyImmediate(child.gameObject);
                    breakCounter++;
                    if (breakCounter > 100)
                    {
                        Debug.LogWarning("BreakCounter triggered something is wrong");
                        break;
                    }
                }
                else
                {
                    Destroy(child.gameObject);
                    breakCounter++;
                }
            }

            string test1 = transform.GetChild(i).name;
            string test2 = transform.GetChild(i).gameObject.name;

            
        }

    }



    [EButton]
    public void PlaceSpuckBareBäre()
    {
        //spawn HimbbärePrefab on every child of this child 0
        foreach (Transform child in transform.GetChild(0))
        {
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
            GameObject s = Instantiate(CoconutPrefab, child.position, Quaternion.identity);
            s.transform.SetParent(child);
        }
    }
}
