using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    private const int PLAY_BEAT_ID = 2; //Beat ID to trigger 'Play game'

    private Animator _animator; //Camera's animator component
    private float _zoomPauseTime; //Number of seconds to wait before zooming camera
    private float _scenePauseTime; //Number of seconds to wait before switching scenes

    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("IsZooming", false);

        _zoomPauseTime = 2.0f;
        _scenePauseTime = 1.5f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Game.CurrentBeat != null)
        {
            //When player selects 'Play game', zoom camera and start game
            if (Game.CurrentBeat.ID == PLAY_BEAT_ID)
            {
                ZoomCamera();
            }
        }
    }

    /// <summary>
    /// Triggers camera zoom animation
    /// </summary>
    private void ZoomCamera()
    {
        if (_animator.GetBool("IsZooming"))
        {
            if (_scenePauseTime <= 0.0f)
            {
                //Switch to game scene
                GameManager.CurrentGameState = GameManager.State.Playing;
                Debug.Log(GameManager.CurrentGameState);
                SceneManager.SwitchScene("GameScene");
            }
            else
            {
                _scenePauseTime -= Time.deltaTime;
            }
        }
        else
        {
            if (_zoomPauseTime <= 0.0f)
            {
                _animator.SetBool("IsZooming", true);
            }
            else
            {
                _zoomPauseTime -= Time.deltaTime;
            }
        }
        
    }
}
