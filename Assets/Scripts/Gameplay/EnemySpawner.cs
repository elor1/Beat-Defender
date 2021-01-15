using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData[] enemyTypes; //Add enemy scriptable objects in inspector
    private enum Enemy
    {
        Basic,
        Strong
    }

    private const float SPAWN_DELAY_MULTIPLIER = 0.5f;
    private const float MIN_SPAWN_DELAY = 1.0f;
    private const float MAX_SPAWN_DELAY = 7.0f;
    private const int INITIAL_MAX_ENEMIES = 10;
    private const int MAX_ENEMIES_MULTIPLIER = 5;
    private const float WAVE_SPAWN_DELAY_MULTIPLIER = 0.25f;
    private const int STRONG_ENEMY_CHANCE = 20;
    private const float PLAYER_RADIUS = 35.0f; //Radius around player where enemies can't spawn

    private float _spawnDelay; //Time between each enemy is spawned
    private float _spawnTimer;
    public int _maxEnemies = 10; //Maximum number of enemies to be alive at one time
    public static int _aliveEnemies = 0; //Current number of enemies alive

    // Start is called before the first frame update
    private void Start()
    {
        _spawnDelay = 3.0f;
        _spawnTimer = _spawnDelay - 1.0f; //First enemy spawns 1 second into game
    }

    // Update is called once per frame
    private void Update()
    {
        //The higher the average amplitude, the faster enemies spawn
        _spawnDelay = Mathf.Clamp(SPAWN_DELAY_MULTIPLIER / AudioAnalyser._averageAmplitude, MIN_SPAWN_DELAY, MAX_SPAWN_DELAY);

        //The higher the wave number, enemies spawn faster and max number of enemies increases
        if (GameManager._waveNumber > 1)
        {
            _maxEnemies = INITIAL_MAX_ENEMIES + (GameManager._waveNumber * MAX_ENEMIES_MULTIPLIER);
            _spawnDelay = Mathf.Clamp(_spawnDelay - (GameManager._waveNumber * WAVE_SPAWN_DELAY_MULTIPLIER), MIN_SPAWN_DELAY, MAX_SPAWN_DELAY);
        }

        //Debug.Log(_spawnDelay);
        if (GameManager._currentGameState == GameManager.State.Playing)
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer >= _spawnDelay)
            {
                _spawnTimer = 0.0f;

                SpawnEnemy();
            }
        }
        
    }

    public void SpawnEnemy()
    {
        if (_aliveEnemies < _maxEnemies)
        {
            MapGenerator.Coordinate spawnPoint = new MapGenerator.Coordinate();
            System.Random randomNumber = new System.Random(System.DateTime.Now.GetHashCode());

            bool isNearPlayer = true;

            //Generate random spawn points until one is found that isn't too close to the player or in a wall
            while (MapGenerator._mapGrid[spawnPoint.tileX, spawnPoint.tileY].type != MapGenerator.TileType.Room || isNearPlayer || !MapGenerator.IsInBounds(spawnPoint.tileX, spawnPoint.tileY))
            {
                isNearPlayer = false;

                spawnPoint.tileX = randomNumber.Next(0 + MapGenerator._borderSize, MapGenerator._width - MapGenerator._borderSize - 1);
                spawnPoint.tileY = randomNumber.Next(0 + MapGenerator._borderSize, MapGenerator._height - MapGenerator._borderSize - 1);

                //If chosen position is within playerRadius, find a new position
                Collider[] nearbyObjects = Physics.OverlapSphere(new Vector3(spawnPoint.tileX * MapGenerator._tileSize, 0.0f, spawnPoint.tileY * MapGenerator._tileSize), PLAYER_RADIUS);
                foreach (Collider obj in nearbyObjects)
                {
                    if (obj.tag == "Player")
                    {
                        isNearPlayer = true;
                    }
                }
            }
            //Debug.Log(MapGenerator._mapGrid[spawnPoint.tileX, spawnPoint.tileY]);
            int index;
            if (Random.Range(0, 100) < STRONG_ENEMY_CHANCE)
            {
                index = (int)Enemy.Strong;
            }
            else
            {
                index = (int)Enemy.Basic;
            }
            Instantiate(enemyTypes[index]._prefab, new Vector3(spawnPoint.tileX * MapGenerator._tileSize, 0.0f, spawnPoint.tileY * MapGenerator._tileSize), Quaternion.identity);
            _aliveEnemies++;
        }
    }
}
