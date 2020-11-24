using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyScriptableObject", order = 1)]
public class EnemyData : ScriptableObject
{
    public string _name;
    public GameObject _prefab;
    public int _health;
    public float _speed;
    public int _damage;
}
