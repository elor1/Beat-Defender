using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private const float SPAWN_DELAY_MULTIPLIER = 0.5f; //Multiplier for amplitude of song to increase spawn times
    private const float MIN_SPAWN_DELAY = 1.0f; //Minimum seconds between enemies spawning
    private const float MAX_SPAWN_DELAY = 7.0f; //Maximum seconds between enemies spawning
    private const int INITIAL_MAX_ENEMIES = 6; //Initial maximum number of enemies alive at once
    private const int MAX_ENEMIES_MULTIPLIER = 2; //Amount maximum number of enemies is increased each wave
    private const float WAVE_SPAWN_DELAY_MULTIPLIER = 0.15f; //Amount spawn delay is decreased each round
    private const int STRONG_ENEMY_CHANCE = 20; //Percentage chance of strong enemy being spawned
    private const float PLAYER_RADIUS = 20.0f; //Radius around player where enemies can't spawn

    private enum Enemy
    {
        Basic,
        Strong
    }

    [SerializeField] private EnemyData[] _enemyTypes; //Add enemy scriptable objects in inspector
    private float _spawnDelay; //Time between each enemy is spawned
    private float _spawnTimer; //Time since last enemy was spawned
    private int _maxEnemies; //Maximum number of enemies to be alive at one time
    private static int _aliveEnemies; //Current number of enemies alive

    public static int AliveEnemies { get { return _aliveEnemies; } set { _aliveEnemies = value; } }

    // Start is called before the first frame update
    private void Start()
    {
        _spawnDelay = 2.5f;
        _spawnTimer = _spawnDelay - 1.0f; //First enemy spawns 1 second into game

        _maxEnemies = 10;
        _aliveEnemies = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        //The higher the average amplitude, the faster enemies spawn
        _spawnDelay = Mathf.Clamp(SPAWN_DELAY_MULTIPLIER / AudioAnalyser.AverageAmplitude, MIN_SPAWN_DELAY, MAX_SPAWN_DELAY);
        
        _maxEnemies = INITIAL_MAX_ENEMIES + (GameManager.WaveNumber * MAX_ENEMIES_MULTIPLIER);
        _spawnDelay = Mathf.Clamp(_spawnDelay - (GameManager.WaveNumber * WAVE_SPAWN_DELAY_MULTIPLIER), MIN_SPAWN_DELAY, MAX_SPAWN_DELAY);

        if (GameManager.CurrentGameState == GameManager.State.Playing)
        {
            _spawnTimer += Time.deltaTime;

            //If time since last enemy spawned is greater than spawn delay, spawn enemy
            if (_spawnTimer >= _spawnDelay)
            {
                _spawnTimer = 0.0f;

                SpawnEnemy();
            }
        }
        
    }
    
    /// <summary>
    /// Spawns a new enemy in a random location
    /// </summary>
    public void SpawnEnemy()
    {
        if (_aliveEnemies < _maxEnemies)
        {
            MapGenerator.Coordinate spawnPoint = GetRandomSpawn();
            
            //Choose a random enemy type
            int index;
            if (Random.Range(0, 100) < STRONG_ENEMY_CHANCE)
            {
                index = (int)Enemy.Strong;
            }
            else
            {
                index = (int)Enemy.Basic;
            }

            //Create new enemy object
            Instantiate(_enemyTypes[index].Prefab, new Vector3(spawnPoint.tileX * MapGenerator.TILE_SIZE, 0.0f, spawnPoint.tileY * MapGenerator.TILE_SIZE), Quaternion.identity);
            _aliveEnemies++;
        }
    }

    /// <summary>
    /// Finds a random valid spawn location
    /// </summary>
    /// <returns>Tile coordinate of spawn location</returns>
    private MapGenerator.Coordinate GetRandomSpawn()
    {
        bool isNearPlayer = true;
        MapGenerator.Coordinate spawnPoint = new MapGenerator.Coordinate();
        System.Random randomNumber = new System.Random(System.DateTime.Now.GetHashCode());

        //Generate random spawn points until one is found that isn't too close to the player or in a wall
        while (MapGenerator.MapGrid[spawnPoint.tileX, spawnPoint.tileY].Type != MapGenerator.TileType.Room || isNearPlayer || !MapGenerator.IsInBounds(spawnPoint.tileX, spawnPoint.tileY))
        {
            isNearPlayer = false;

            spawnPoint.tileX = randomNumber.Next(0 + MapGenerator.BORDER_SIZE, MapGenerator.Width - MapGenerator.BORDER_SIZE - 1);
            spawnPoint.tileY = randomNumber.Next(0 + MapGenerator.BORDER_SIZE, MapGenerator.Height - MapGenerator.BORDER_SIZE - 1);

            //If chosen position is within playerRadius, find a new position
            Collider[] nearbyObjects = Physics.OverlapSphere(new Vector3(spawnPoint.tileX * MapGenerator.TILE_SIZE, 0.0f, spawnPoint.tileY * MapGenerator.TILE_SIZE), PLAYER_RADIUS);
            foreach (Collider obj in nearbyObjects)
            {
                if (obj.tag == "Player")
                {
                    isNearPlayer = true;
                }
            }
        }

        return spawnPoint;
    }
}
