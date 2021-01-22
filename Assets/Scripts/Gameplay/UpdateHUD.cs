using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateHUD : MonoBehaviour
{
    private const string WAVE_TEXT = "Wave number: "; //HUD wave label
    private const string END_STATS_TOP = "Waves survived: "; //Top stat on end screen
    private const string END_STATS_BOTTOM = "Enemies killed: "; //Bottom stat on end screen

    private static TMP_Text _displayText; //HUD text component

    // Start is called before the first frame update
    void Start()
    {
        _displayText = GetComponent<TMP_Text>();

        if (GameManager.CurrentGameState == GameManager.State.Over)
        {
            _displayText.text = END_STATS_TOP + (GameManager.WaveNumber + 1).ToString() + "\n"
                                + END_STATS_BOTTOM + GameManager.EnemiesKilled.ToString();
        }
    }

    /// <summary>
    /// Update HUD's display to show current wave number
    /// </summary>
    public static void UpdateWaveNumber()
    {
        _displayText.text = WAVE_TEXT + (GameManager.WaveNumber + 1).ToString();
    }
}
