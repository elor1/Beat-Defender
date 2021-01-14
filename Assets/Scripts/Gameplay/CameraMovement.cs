using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    private Animator _anim; //Camera's animator component
    [SerializeField] private float _zoomPauseTime = 2.0f; //Number of seconds to wait before zooming camera
    [SerializeField] private float _scenePauseTime = 1.5f; //Number of seconds to wait before switching scenes

    // Start is called before the first frame update
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.SetBool("IsZooming", false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Game._currentBeat != null)
        {
            //When player selects a difficulty, zoom camera and start game
            if (Game._currentBeat.ID == 2)
            {
                ZoomCamera();
            }
        }
    }

    private void ZoomCamera()
    {
        if (_anim.GetBool("IsZooming"))
        {
            if (_scenePauseTime <= 0.0f)
            {
                GameManager._currentGameState = GameManager.State.Playing;
                Debug.Log(GameManager._currentGameState);
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
                _anim.SetBool("IsZooming", true);
            }
            else
            {
                _zoomPauseTime -= Time.deltaTime;
            }
        }
        
    }
}
