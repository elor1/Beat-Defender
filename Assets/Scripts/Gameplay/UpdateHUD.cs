using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateHUD : MonoBehaviour
{
    private static TMP_Text _displayText;

    private const string _waveText = "Wave number: ";

    // Start is called before the first frame update
    void Start()
    {
        _displayText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UpdateWaveNumber()
    {
        _displayText.text = _waveText + GameManager._waveNumber.ToString();
    }
}
