using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PhaseSetup", menuName = "PhaseSetup")]
public class PhaseSetup : ScriptableObject
{
    


    [SerializeField] public List<Phase> _phases = new List<Phase>();
}
