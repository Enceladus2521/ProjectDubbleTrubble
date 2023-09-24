using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpinner : MonoBehaviour
{
    [SerializeField] float SpinnSpeed = 1f;

    [SerializeField] float radius = 1f;

    

    
    private void Start()
    {
        
        PositionChildren();
        StartCoroutine(Spin());
    }

    [EButton]
    private void PositionChildren()
    {
        //position the children in a circle around the parent in radius distance in an angleStep angle steps

        float angleStep =  360f / transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            float z = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            transform.GetChild(i).transform.localPosition = new Vector3(x, 0, z);   

            

        }

        //rotate the children so they look at the next child last child looks at first child
        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 lookAtPos;
            if(i == transform.childCount - 1)
            {
                lookAtPos = transform.GetChild(0).transform.position;
            }
            else
            {
                lookAtPos = transform.GetChild(i + 1).transform.position;
            }
            transform.GetChild(i).transform.LookAt(lookAtPos);
        }
    }


    IEnumerator Spin()
    {
        while(true)
        {
            transform.Rotate(Vector3.up, SpinnSpeed * Time.deltaTime);
            yield return null;
        }
    }


}
