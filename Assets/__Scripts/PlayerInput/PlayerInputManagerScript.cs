using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerInputManagerScript : MonoBehaviour
{

    [SerializeField] private List<GameObject> players = new List<GameObject>();	


    private void Start()
    {
        
    }


    public void OnPlayerJoined()
    {
        players.Clear();
        GameObject[] pl = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject p in pl)
        {
            players.Add(p);
        }

        GameObject child = transform.GetChild(0).gameObject;

        CinemachineTargetGroup targetGroop = child.GetComponent<CinemachineTargetGroup>();

        targetGroop.m_Targets = new CinemachineTargetGroup.Target[players.Count];

        for(int i = 0; i < players.Count; i++)
        {
            targetGroop.m_Targets[i].target = players[i].transform;
            targetGroop.m_Targets[i].weight = 1;
        }

    }


}
