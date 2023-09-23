using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SnekBody : MonoBehaviour
{
    [Range(1, 100)]
    [SerializeField] float playerDistance = 5;
    [Range(1, 100)]
    [SerializeField] int bodySegments = 5;
    //[SerializeField] GameObject bodyPartPrefab;
    [SerializeField] GameObject leftpartPrefab;
    [SerializeField] GameObject rightpartPrefab;
    [SerializeField] GameObject middlepartPrefab;

    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;


    public List<GameObject> bodyParts;


    private void Awake()
    {
        populatebodyParts();
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    private void OnValidate()
    {
        // bodyParts even number +1
        if (bodySegments % 2 == 0)
        {
            bodySegments++;
        }
        
    }





    [EButton]
    private void populatebodyParts()
    {

        //move player1 playerdistance/2 to the left and player2 playerdistance/2 to the right
        player1.transform.position = new Vector3(transform.position.x - playerDistance / 2, transform.position.y, transform.position.z);
        player2.transform.position = new Vector3(transform.position.x + playerDistance / 2, transform.position.y, transform.position.z);

        //rotate player1 away from player2
        player1.transform.LookAt(player2.transform);
        player1.transform.Rotate(0, 180, 0);
        //rotate player2 away from player1
        player2.transform.LookAt(player1.transform);
        player2.transform.Rotate(0, 180, 0);

        DestroyAllChildren();
        //spawn body parts
        for (int i = 0; i < bodySegments; i++)
        {
            //find spawn position between player1 and player2 depending on i
            float startOffSet = playerDistance / bodySegments / 2;
            Vector3 spawnPos = Vector3.Lerp(player1.transform.position, player2.transform.position, (float)i / (float)bodySegments);
            //add offset to spawn position
            spawnPos = new Vector3(spawnPos.x + startOffSet, spawnPos.y, spawnPos.z);

            GameObject bodyPart;
            //spawn body part depending if(i) < bodySegments/2 spawn leftpartPrefab else if(i) > bodySegments/2 spawn rightpartPrefab else spawn middlepartPrefab
            if (i < bodySegments / 2)
            {
                bodyPart = Instantiate(leftpartPrefab, spawnPos, Quaternion.identity);
                //rotate body part towards player1
                bodyPart.transform.LookAt(player1.transform);
            }
            else if (i > bodySegments / 2)
            {
                bodyPart = Instantiate(rightpartPrefab, spawnPos, Quaternion.identity);
                //rotate body part towards player2
                bodyPart.transform.LookAt(player2.transform);
            }
            else
            {
                bodyPart = Instantiate(middlepartPrefab, spawnPos, Quaternion.identity);
                //rotate body part towards player1
                bodyPart.transform.LookAt(player1.transform);
            }




            //set layer to 15
            bodyPart.layer = 15;
            //add sphere collider
            SphereCollider sphereCollider = bodyPart.AddComponent<SphereCollider>();
            sphereCollider.radius = 0.2f;
            
            bodyPart.transform.SetParent(transform);
            bodyParts.Add(bodyPart);

            //connect CharacterJoint to previous body part if first body part add player1 as connected body instead if last body part add previus and add additionel new character joint component and player2 as connected body
            if (i == 0)
            {
                CharacterJoint joint = bodyPart.GetComponent<CharacterJoint>();
                joint.connectedBody = player1.GetComponent<Rigidbody>();
            }
            else if (i == bodySegments - 1)
            {
                CharacterJoint joint = bodyPart.GetComponent<CharacterJoint>();
                joint.connectedBody = bodyParts[i - 1].GetComponent<Rigidbody>();
                joint = bodyPart.AddComponent<CharacterJoint>();
                joint.connectedBody = player2.GetComponent<Rigidbody>();
            }
            else
            {
                CharacterJoint joint = bodyPart.GetComponent<CharacterJoint>();
                joint.connectedBody = bodyParts[i - 1].GetComponent<Rigidbody>();
            }



        }
    }

    private void DestroyAllChildren()
    {
        if (Application.isEditor)
        {
            bodyParts.Clear();
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }

            if (transform.childCount == 0)
            {
                
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


    private void OnDrawGizmos()
    {

    }










}


