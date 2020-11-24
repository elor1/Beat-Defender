using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData[] enemyTypes; //Add enemy scriptable objects in inspector

    public float _spawnDelay = 5.0f; //Time between each enemy is spawned
    private float _spawnTimer = 0.0f;
    public int _maxEnemies = 20; //Maximum number of enemies to be alive at one time
    public int _aliveEnemies = 0; //Current number of enemies alive
    [SerializeField] private float _playerRadius = 35.0f; //Radius around player where enemies can't spawn

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnDelay)
        {
            _spawnTimer = 0.0f;

            SpawnEnemy();
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
            while (MapGenerator._mapGrid[spawnPoint.tileX, spawnPoint.tileY] != MapGenerator.TileType.Room || isNearPlayer)
            {
                isNearPlayer = false;

                spawnPoint.tileX = randomNumber.Next(0 + MapGenerator._borderSize, MapGenerator._width - MapGenerator._borderSize - 1);
                spawnPoint.tileY = randomNumber.Next(0 + MapGenerator._borderSize, MapGenerator._height - MapGenerator._borderSize - 1);

                //If chosen position is within playerRadius, find a new position
                Collider[] nearbyObjects = Physics.OverlapSphere(new Vector3(spawnPoint.tileX * MapGenerator._tileSize, 0.0f, spawnPoint.tileY * MapGenerator._tileSize), _playerRadius);
                foreach (Collider obj in nearbyObjects)
                {
                    if (obj.tag == "Player")
                    {
                        isNearPlayer = true;
                    }
                }
            }
            //Debug.Log(MapGenerator._mapGrid[spawnPoint.tileX, spawnPoint.tileY]);
            Instantiate(enemyTypes[randomNumber.Next(0, enemyTypes.Length - 1)]._prefab, new Vector3(spawnPoint.tileX * MapGenerator._tileSize, 0.0f, spawnPoint.tileY * MapGenerator._tileSize), Quaternion.identity);
            _aliveEnemies++;
        }
    }
}
