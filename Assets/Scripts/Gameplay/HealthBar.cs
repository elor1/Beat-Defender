using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static Slider _healthSlider; //Health bar's slider component

    // Start is called before the first frame update
    void Start()
    {
        _healthSlider = GetComponent<Slider>();
        _healthSlider.maxValue = GameManager.PlayerStartingHealth;
        _healthSlider.value = GameManager.PlayerStartingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthSlider)
        {
            //Update health bar's value
            _healthSlider.value = GameManager.PlayerHealth;
        }
    }
}
