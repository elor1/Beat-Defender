using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyScriptableObject", order = 1)]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string _name; //Enemy type name
    [SerializeField] private GameObject _prefab; //Prefab to spawn
    [SerializeField] private int _health; //Base health
    [SerializeField] private float _speed; //Base speed
    [SerializeField] private int _damage; //Base damage
    [SerializeField] private float _fireRate; //Based fire rate

    public GameObject Prefab{ get { return _prefab; } }
    public int Health { get { return _health; } }
    public float Speed { get { return _speed; } }
    public int Damage { get { return _damage; } }
    public float FireRate { get { return _fireRate; } }
}
