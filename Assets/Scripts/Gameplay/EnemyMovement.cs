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
    private Tile _previousTile;

    [SerializeField] float _playerRadius = 10.0f;
    [SerializeField] float _enemyRadius = 10.0f;

    private SpawnProjectiles _projectileSpawner;

    private float _step;

    private Vector3 _spawnPosition;
    private bool _collision = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();

        _pathfinding = GetComponent<Pathfinding>();
        _pathToPlayer = new List<Tile>();

        _projectileSpawner = GetComponent<SpawnProjectiles>();

        _health = _enemyData._health;

        _previousTile = new Tile();
        _previousTile.coord.tileX = (int)transform.position.x;
        _previousTile.coord.tileY = (int)transform.position.z;

        _spawnPosition = transform.position;
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
        transform.LookAt(_player.transform);

        if (_player)
        {
            if (gameObject)
            {
                if (Vector3.Distance(_player.transform.position, transform.position) > _playerRadius)
                {
                    //Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, _enemyRadius);

                    //foreach (Collider c in nearbyObjects)
                    //{
                    //    if (c.gameObject.tag == "Enemy" && c.transform.position != transform.position)
                    //    {
                    //        _pathfinding._nodes[(int)c.transform.position.x, (int)c.transform.position.z].neighbourWalls += 100;
                    //        _pathToPlayer.Clear();
                    //        _pathToPlayer = _pathfinding.FindPath(transform.position, _player.transform.position);
                    //    }
                    //}
                    //if (_collision == true)
                    //{
                    //    transform.position = Vector3.MoveTowards(transform.position, _spawnPosition / 10, _step / 1000);
                    //    if (Vector3.Distance(transform.position, _spawnPosition / 10) < 0.001f)
                    //    {
                    //        _collision = false;
                    //    }
                    //}

                    if (_pathToPlayer.Count == 0)
                    {
                        _pathToPlayer.Clear();
                        _pathToPlayer = _pathfinding.FindPath(gameObject.transform.position, _player.transform.position);
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


                }
            }
        }
        
    }

    private void LateUpdate()
    {
        if (HasLineOfSight(gameObject, _player))
        {
            Color particleColour = new Color(Random.Range(0.890f, 0.921f), Random.Range(0.329f, 0.482f), Random.Range(0.020f, 0.188f), 1.0f);
            _projectileSpawner.SpawnParticle(particleColour);

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
            _previousTile = _pathToPlayer[0];
            _pathToPlayer.Remove(_pathToPlayer[0]);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If an enemy collides with another enemy, move back to its spawn position
        if (collision.gameObject.tag == "Enemy")
        {
            //_collision = true;
            _pathToPlayer.Clear();
            MapGenerator.Coordinate randomCoord = MapGenerator.GetRandomRoomTile();
            _pathToPlayer = _pathfinding.FindPath(gameObject.transform.position, new Vector3(randomCoord.tileX, 0.0f, randomCoord.tileY));
        }
        
    }
}
