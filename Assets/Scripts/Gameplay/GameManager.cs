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

    public static int _playerHealth;
    public static int _playerDamage = 10;

    // Start is called before the first frame update
    void Start()
    {
        _currentGameState = State.Playing;
        _playerHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerHealth <= 0)
        {
            //Player is dead. Game over.
            _currentGameState = State.Over;
        }
    }
}
