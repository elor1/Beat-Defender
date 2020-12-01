using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject _player;

    private Rigidbody _rb;
    [SerializeField] private EnemyData _enemyData;

    private List<Tile> _pathToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _pathToPlayer = Pathfinding.FindPath(gameObject, _player);

        if (_pathToPlayer.Count > 0)
        {
            //if (transform.position.x == _pathToPlayer[0].coord.tileX && transform.position.z == _pathToPlayer[0].coord.tileY)
            //{
            //    _pathToPlayer.Remove(_pathToPlayer[0]);
            //}

            //Vector3 direction = (new Vector3(_pathToPlayer[0].coord.tileX, 0.0f, _pathToPlayer[0].coord.tileY) - transform.position).normalized;
            //_rb.MovePosition(transform.position + direction * _enemyData._speed * Time.deltaTime);
            //transform.position = new Vector3(_pathToPlayer[0].coord.tileX, 0.0f, _pathToPlayer[0].coord.tileY);

            float step = _enemyData._speed * Time.deltaTime;
            Vector3 target = new Vector3(_pathToPlayer[0].coord.tileX, 0.0f, _pathToPlayer[0].coord.tileY);
            transform.position = Vector3.MoveTowards(transform.position, target, step/1000);
            if (Vector3.Distance(transform.position, target) < 0.001f)
            {
                _pathToPlayer.Remove(_pathToPlayer[0]);
            }
        }
    }
}
