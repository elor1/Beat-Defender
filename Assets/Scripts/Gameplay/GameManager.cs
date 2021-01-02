using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        Start,
        Playing,
        WaveEnd,
        Over,
    }

    public enum Upgrade
    {
        ProjectileSpeed,
        MovementSpeed,
        ProjectileDamage,
    }

    private float _upgradeTimer = 2.0f;
    public static State _currentGameState = State.Start;

    [SerializeField] private GameObject _screen;

    public static int _playerHealth = 100;
    public static int _playerDamage = 1;

    private static int _waveNumber;
    public static bool _choosingUpgrade;

    public static AudioSource _audioSource;
    [SerializeField] private AudioClip[] songs; //Songs to be added in inspector
    private int _currentSongIndex;
    private float _timePlaying;

    private int[] _upgradeIDs = { 8 };
    private Dictionary<int, Upgrade> _upgradeChoiceIDs = new Dictionary<int, Upgrade>()
    {
        { 9, Upgrade.ProjectileSpeed },
        { 10, Upgrade.ProjectileDamage }
    };

    public static float _playerSpeed = 7000.0f;
    public static float _projectileSpeed = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        
        //_currentGameState = State.Start;
        _playerHealth = 100;
        _waveNumber = 0;
        _choosingUpgrade = false;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = songs[0];
        _currentSongIndex = 0;
        _timePlaying = _audioSource.clip.length;

        _screen.SetActive(false);
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
        
        if (_currentGameState == State.WaveEnd && !_choosingUpgrade)
        {
            //Upgrades();
            MapGenerator.GenerateMap();
            ChangeSong();

            Debug.Log("Cleared screen");
            _screen.SetActive(false);
            _currentGameState = State.Playing;
        }

        if (_timePlaying <= 0.0f)
        {
            //Wave over
            _currentGameState = State.WaveEnd;

            Debug.Log("Wave over");
            //DestroyEnemies();
            UpdateScreen();
            //_waveNumber++;
            //MapGenerator.GenerateMap();
            // ChangeSong();
            //_timePlaying = 5000.0f;
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
                    //Destroy(movementScript);
                    movementScript._isAlive = false;
                }
                //Destroy(enemy);
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

    private void UpdateScreen()
    {
        _choosingUpgrade = true;
        _screen.SetActive(true);
        //Game._singleton.DisplayBeat(8);
        //Game._currentBeat = Game._singleton._data.GetBeatById(8);
        //Game._singleton.CancelInvoke();
        //System.Random randomNumber = new System.Random(System.DateTime.Now.GetHashCode());
        //Game._singleton.DisplayBeat(_upgradeIDs[randomNumber.Next(0, _upgradeIDs.Length - 1)]);
        Game._singleton.DisplayBeat(8);
        _timePlaying = 5000.0f;
    }

    private void Upgrades()
    {
        _upgradeTimer -= Time.deltaTime;
        if (_upgradeChoiceIDs.ContainsKey(Game._currentBeat.ID))
        {
            if (_upgradeTimer <= 0.0f)
            {
                _upgradeTimer = 2.0f;
                Upgrade upgradeType;
                _upgradeChoiceIDs.TryGetValue(Game._currentBeat.ID, out upgradeType);
                switch (upgradeType)
                {
                    case Upgrade.MovementSpeed:
                        _playerSpeed *= 1.1f;
                        break;
                    case Upgrade.ProjectileDamage:
                        _playerDamage *= 2;
                        break;
                    case Upgrade.ProjectileSpeed:
                        _projectileSpeed *= 1.2f;
                        break;
                }

                MapGenerator.GenerateMap();
                ChangeSong();
                
                Debug.Log("Cleared screen");
                _screen.SetActive(false);
                _currentGameState = State.Playing;
            }
        }
    }
}
