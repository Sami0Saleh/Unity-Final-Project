using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemySO : ScriptableObject
{
    public enum Type {Shooter, Giant, Flyer, None}
    
    [SerializeField] public int startingHealth;

    [SerializeField] public float baseSpeed;

    [SerializeField] public Type type;





}
