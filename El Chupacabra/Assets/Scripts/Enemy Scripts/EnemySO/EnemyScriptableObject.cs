using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemySO : ScriptableObject
{
    [SerializeField] public enum Type {Shooter, Giant, None}
    
    [SerializeField] public int startingHealth;

    [SerializeField] public float baseSpeed;

    [SerializeField] public Type type;
}
