using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateHUD : MonoBehaviour
{
    private const string WAVE_TEXT = "Wave number: "; //HUD wave label

    private static TMP_Text _displayText; //HUD text component

    // Start is called before the first frame update
    void Start()
    {
        _displayText = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Update HUD's display to show current wave number
    /// </summary>
    public static void UpdateWaveNumber()
    {
        _displayText.text = WAVE_TEXT + GameManager.WaveNumber.ToString();
    }
}
