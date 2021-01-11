using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static Slider _healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        _healthSlider = GetComponent<Slider>();
        _healthSlider.maxValue = GameManager._playerStartingHealth;
        _healthSlider.value = GameManager._playerStartingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthSlider)
        {
            _healthSlider.value = GameManager._playerHealth;
        }
    }
}
