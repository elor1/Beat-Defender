using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        Start,
        Playing,
        WaveEnd,
        Over,
    }

    public static State _currentGameState = State.Playing;

    public static int _playerHealth = 100;
    public static int _playerDamage = 1;

    private static int _waveNumber;

    public static AudioSource _audioSource;
    [SerializeField] private AudioClip[] songs; //Songs to be added in inspector
    private int _currentSongIndex;
    private float _timePlaying;

    // Start is called before the first frame update
    void Start()
    {
        _currentGameState = State.Playing;
        _playerHealth = 100;
        _waveNumber = 0;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = songs[0];
        _currentSongIndex = 0;
        _timePlaying = _audioSource.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        _timePlaying -= Time.deltaTime;

        if (_playerHealth <= 0)
        {
            //Player is dead. Game over.
            //_currentGameState = State.Over;
            Debug.Log("GAME OVER");
        }
        
        if (_timePlaying <= 0.0f)
        {
            //Wave over
            Debug.Log("Wave over");
            DestroyEnemies();
            _waveNumber++;
            MapGenerator.GenerateMap();
            ChangeSong();
        }
    }

    private void DestroyEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies != null)
        {
            foreach(GameObject enemy in enemies)
            {
                EnemyMovement movementScript = enemy.GetComponent<EnemyMovement>();
                if (movementScript)
                {
                    Destroy(movementScript);
                }
                Destroy(enemy);
            }
        }
    }

    private void ChangeSong()
    {
        _currentSongIndex++;
        if (_currentSongIndex >= songs.Length)
        {
            _currentSongIndex = 0;
        }
        _audioSource.clip = songs[_currentSongIndex];
        _timePlaying = _audioSource.clip.length;
        _audioSource.Play();
    }
}
