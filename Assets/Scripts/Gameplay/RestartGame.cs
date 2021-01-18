using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    /// <summary>
    /// Goes back to menu screen and resets the game
    /// </summary>
    public void Restart()
    {
        GameManager.CurrentGameState = GameManager.State.Start;
        SceneManager.SwitchScene("IntroScene");
    }
}
