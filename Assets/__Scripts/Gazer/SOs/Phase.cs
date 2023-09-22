using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Phase", menuName = "Phase")]
public class Phase : ScriptableObject
{
    [Header("Time Settings")]
    [SerializeField] public float _timeBefore = 1f;
    [SerializeField] public float _timeAfter = 1f;
    [SerializeField] public float _duration = 1f;

    [Header("Gazer Settings")]
    [SerializeField] public float _speed = 1f;
    [SerializeField] public float _size = 1f;


    [Header("Attack Pool")]
    [SerializeField] public List<IAttack> _attackPool = new List<IAttack>();
}
