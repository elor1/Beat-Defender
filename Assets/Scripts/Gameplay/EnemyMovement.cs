using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private const float FREQUENCY_MULTIPLIER = 0.00225f; //Amount loudest current frequency is multiplied by to increase fire rate
    private const int HEALTH_MULTIPLIER = 3; //Amount of health increase to enemies with each wave
    private const float FIRE_RATE_MULTIPLIER = 0.007f; //Amount rate of fire is increased each wave
    private const int DAMAGE_MULTIPLIER = 1; //Amount of damage increase enemies get each wave
    private const float PLAYER_RADIUS = 10.0f; //Radius around player that enemies can't enter

    [SerializeField] private EnemyData _enemyData; //Enemy data scriptable object for enemy type
    private GameObject _player; //Player game object
    private Rigidbody _rigidbody; //Enemy's rigidbody component
    private SpawnProjectiles _projectileSpawner; //Enemy's projectile spawner component
    private Pathfinding _pathfinding; //Enemy's pathfinding component
    private float _fireRate; //Enemy fire rate
    private int _damage; //Enemy damage
    private bool _isAlive; //Used to check if enemy is alive or dead
    private List<Tile> _pathToPlayer; //Current path of tiles to player
    private Tile _previousTile; //Last tile enemy moved to
    private float _step; //Amount to move enemy by
    private float _timePassed; //Time passed since last projectile fired

    public int Health; //Enemy health
    public int Damage { get { return _damage; } }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rigidbody = GetComponent<Rigidbody>();
        _pathfinding = GetComponent<Pathfinding>();
        _projectileSpawner = GetComponent<SpawnProjectiles>();

        ////Set enemy stats
        //if (GameManager.WaveNumber > 1)
        //{

        //}
        //else
        //{
        //    Health = _enemyData.Health;
        //    _fireRate = _enemyData.FireRate;
        //    _damage = _enemyData.Damage;
        //}

        //Increase enemy stats with each wave
        Health = _enemyData.Health + (GameManager.WaveNumber * HEALTH_MULTIPLIER);
        _fireRate = _enemyData.FireRate - (GameManager.WaveNumber * FIRE_RATE_MULTIPLIER);
        _damage = _enemyData.Damage + (GameManager.WaveNumber * DAMAGE_MULTIPLIER);

        _fireRate -= AudioAnalyser.LoudestFrequency * FREQUENCY_MULTIPLIER;

        _previousTile = new Tile();
        _previousTile._coord.tileX = (int)transform.position.x;
        _previousTile._coord.tileY = (int)transform.position.z;

        _isAlive = true;

        _pathToPlayer = new List<Tile>();
    }

    private void Update()
    {
        //If enemy health is less than 0 or the wave ends, enemy is dead
        if (GameManager.CurrentGameState != GameManager.State.Playing || Health <= 0)
        {
            _isAlive = false;
        }

        //Destroy enemy object when dead
        if (!_isAlive)
        {
            _isAlive = false;
            Destroy(gameObject);
            EnemySpawner.AliveEnemies--;
            Destroy(this);
        }

        _timePassed += Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isAlive)
        {
            transform.LookAt(_player.transform);

            if (_player)
            {
                if (gameObject)
                {
                    if (Vector3.Distance(_player.transform.position, transform.position) > PLAYER_RADIUS)
                    {
                        //If enemy doesn't have a path to the player, find a new path
                        if (_pathToPlayer == null || _pathToPlayer.Count == 0)
                        {
                            _pathToPlayer = _pathfinding.FindPath(gameObject.transform.position, _player.transform.position);
                        }
                        else
                        {
                            //Move enemy towards player
                            _step = _enemyData.Speed * Time.deltaTime;
                            Vector3 target = new Vector3(_pathToPlayer[0]._coord.tileX, 0.0f, _pathToPlayer[0]._coord.tileY);
                            MoveEnemy(target);
                        }


                    }
                }
            }
        }
        
        
    }

    private void LateUpdate()
    {
        if (_isAlive)
        {
            //If enemy has line of sight to the player, fire projectiles
            if (HasLineOfSight(gameObject, _player))
            {
                if (_timePassed >= _fireRate)
                {
                    Color particleColour = new Color(Random.Range(0.890f, 0.921f), Random.Range(0.329f, 0.482f), Random.Range(0.020f, 0.188f), 1.0f);
                    _projectileSpawner.SpawnParticle(particleColour);
                    _timePassed = 0.0f;
                }
            }
        }
        
    }

    /// <summary>
    /// Checks to see if enemy has line of sight on the player
    /// </summary>
    /// <param name="obj">Enemy</param>
    /// <param name="target">Player</param>
    /// <returns>True or false</returns>
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

    /// <summary>
    /// Smoothly moves enemy to target location
    /// </summary>
    /// <param name="target">Object to move towards</param>
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
        //If an enemy collides with another enemy, move to a random position
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Wall")
        {
            MapGenerator.Coordinate randomCoord = MapGenerator.GetRandomRoomTile();
            _pathToPlayer = _pathfinding.FindPath(gameObject.transform.position, new Vector3(randomCoord.tileX, 0.0f, randomCoord.tileY));
        }
        
    }
}
