using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "FaceAnim", menuName = "ScriptableObjects/FaceAnim", order = 1)]
public class FaceAnim : ScriptableObject
{
    [SerializeField] public List<Sprite> faces;
    [SerializeField] public GameObject eyeObject;
}
