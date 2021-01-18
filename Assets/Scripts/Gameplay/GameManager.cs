using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private const int UPGRADE_BEAT_ID = 4; //ID of upgrade screen beat

    public enum State
    {
        Start,
        Playing,
        WaveEnd,
        Over,
    }

    [SerializeField] private GameObject _screen; //Screen used to display upgrades
    private static State _currentGameState = State.Start; //Current state of the game
    private static int _playerStartingHealth; //Player's health at the start of the wave
    private static int _playerDamage; //Player damage amount
    private static float _playerFireRate; //Player's fire rate
    private static float _playerSpeed; //Player's move speed
    private static float _projectileSpeed; //Player projectile speed
    private static int _waveNumber; //Current wave number
    private static bool _choosingUpgrade; //Checks whether player is currently choosing an upgrade
    private AudioSource _audioSource; //Game's main audio source
    [SerializeField] private AudioClip[] _songs; //Songs to be added in inspector
    private int _currentSongIndex; //Index of song currently playing
    private float _timePlaying; //Number of seconds left before current song ends

    public static int PlayerHealth; //Player's current health
    public static State CurrentGameState { get { return _currentGameState; } set { _currentGameState = value; } }
    public static int PlayerStartingHealth { get { return _playerStartingHealth; } set { _playerStartingHealth = value; } }
    public static int PlayerDamage { get { return _playerDamage; } set { _playerDamage = value; } }
    public static float PlayerFireRate { get { return _playerFireRate; } set { _playerFireRate = value; } }
    public static float PlayerSpeed { get { return _playerSpeed; } set { _playerSpeed = value; } }
    public static float ProjectileSpeed { get { return _projectileSpeed; } set { _projectileSpeed = value; } }
    public static int WaveNumber { get { return _waveNumber; } set { _waveNumber = value; } }
    public static bool ChoosingUpgrade { set { _choosingUpgrade = value; } }

    // Start is called before the first frame update
    void Awake()
    {

        //_currentGameState = State.Start;
        _playerStartingHealth = 150;
        PlayerHealth = _playerStartingHealth;
        _playerDamage = 10;
        _playerFireRate = 0.2f;
        _playerSpeed = 7000.0f;
        _projectileSpeed = 40.0f;

        _waveNumber = 0;
        _choosingUpgrade = false;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _songs[0];
        _currentSongIndex = 0;
        _timePlaying = _audioSource.clip.length;

        _screen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _timePlaying -= Time.deltaTime;

        //If player runs out of health, game is over
        if (PlayerHealth <= 0)
        {
            //Change game state and scene
            _currentGameState = State.Over;
            _audioSource.Stop();
            SceneManager.SwitchScene("EndScene");
            Debug.Log("GAME OVER");
        }
        
        //When player has finished choosing upgrades, start new wave
        if (_currentGameState == State.WaveEnd && !_choosingUpgrade)
        {
            MapGenerator.GenerateMap();
            ChangeSong();
            
            _screen.SetActive(false);
            _currentGameState = State.Playing;
            PlayerHealth = _playerStartingHealth;
        }

        //When song finishes, wave ends
        if (_timePlaying <= 0.0f)
        {
            _currentGameState = State.WaveEnd;
            ShowUpgradeScreen();
        }
    }

    /// <summary>
    /// Changes song to next one in list
    /// </summary>
    private void ChangeSong()
    {
        _currentSongIndex++;
        if (_currentSongIndex >= _songs.Length)
        {
            _currentSongIndex = 0;
        }
        _audioSource.clip = _songs[_currentSongIndex];
        _timePlaying = _audioSource.clip.length;
        _audioSource.Play();
    }

    /// <summary>
    /// Displays upgrade screen
    /// </summary>
    private void ShowUpgradeScreen()
    {
        _choosingUpgrade = true;
        _screen.SetActive(true);
        Game._singleton.DisplayBeat(UPGRADE_BEAT_ID);
        _timePlaying = 5000.0f;
    }
}
