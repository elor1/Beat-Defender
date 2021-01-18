using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);//Stop SceneManager from being destroyed when switching scenes
    }

    /// <summary>
    /// Switches scene to given scene name
    /// </summary>
    /// <param name="sceneName">Scene to switch to</param>
    public static void SwitchScene (string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
