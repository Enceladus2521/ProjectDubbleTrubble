using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnekBody : MonoBehaviour
{
    [SerializeField] int bodySegments = 5;
    [SerializeField] GameObject bodyPartPrefab;

    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;

    
    public List<GameObject> bodyParts;


    [EButton]
    private void populatebodyParts()
    {
        DestroyAllChildren();
        //spawn body parts
        for (int i = 0; i < bodySegments; i++)
        {
            //find spawn position between player1 and player2 depending on i
            Vector3 spawnPos = Vector3.Lerp(player1.transform.position, player2.transform.position, (float)i / (float)bodySegments);

            
            //rotate towards player1
            Quaternion spawnRot = Quaternion.LookRotation(player1.transform.position - spawnPos);
            GameObject bodyPart = Instantiate(bodyPartPrefab, spawnPos, spawnRot);
            bodyPart.transform.SetParent(transform);
            bodyParts.Add(bodyPart);
        }
    }

    private void DestroyAllChildren()
    {
        if(Application.isEditor)
        {
            bodyParts.Clear();
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }

            if (transform.childCount == 0)
            {
                Debug.Log("All children destroyed");
            }
            else
            {
                DestroyAllChildren();
            }
        }
        else
        {
            bodyParts.Clear();
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
