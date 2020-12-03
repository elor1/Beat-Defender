using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject _player;

    private Rigidbody _rb;
    public EnemyData _enemyData;
    public int _health;

    private Pathfinding _pathfinding;
    private List<Tile> _pathToPlayer;

    [SerializeField] float _playerRadius = 10.0f;

    private SpawnProjectiles _projectileSpawner;

    private float _step;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();

        _pathfinding = GetComponent<Pathfinding>();
        _pathToPlayer = new List<Tile>();

        _projectileSpawner = GetComponent<SpawnProjectiles>();

        _health = _enemyData._health;
    }

    private void Update()
    {
        if (_health <= 0)
        {
            //Enemy is dead
            Destroy(gameObject);
            EnemySpawner._aliveEnemies--;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) > _playerRadius)
        {
            if  (_pathToPlayer.Count == 0)
            {
                _pathToPlayer.Clear();
                _pathToPlayer = _pathfinding.FindPath(gameObject, _player);
            }
            else
            {
                //if (transform.position.x == _pathToPlayer[0].coord.tileX && transform.position.z == _pathToPlayer[0].coord.tileY)
                //{
                //    _pathToPlayer.Remove(_pathToPlayer[0]);
                //}

                //Vector3 direction = (new Vector3(_pathToPlayer[0].coord.tileX, 0.0f, _pathToPlayer[0].coord.tileY) - transform.position).normalized;
                //_rb.MovePosition(transform.position + direction * _enemyData._speed * Time.deltaTime);
                //transform.position = new Vector3(_pathToPlayer[0].coord.tileX, 0.0f, _pathToPlayer[0].coord.tileY);

                _step = _enemyData._speed * Time.deltaTime;
                Vector3 target = new Vector3(_pathToPlayer[0].coord.tileX, 0.0f, _pathToPlayer[0].coord.tileY);
                MoveEnemy(target);
            }

            transform.LookAt(_player.transform);
        }
    }

    private void LateUpdate()
    {
        if (HasLineOfSight(gameObject, _player))
        {
            _projectileSpawner.SpawnParticle();

        }
    }

    private bool HasLineOfSight(GameObject obj, GameObject target)
    {
        RaycastHit hit;
        Vector3 direction = target.transform.position - obj.transform.position;
        if (Physics.Raycast(obj.transform.position, direction, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    private void MoveEnemy(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, _step / 1000);
        if (Vector3.Distance(transform.position, target) < 0.001f)
        {
            _pathToPlayer.Remove(_pathToPlayer[0]);
        }
    }
}
