using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    private enum Upgrade
    {
        ProjectileSpeed,
        MovementSpeed,
        ProjectileDamage,
        RateOfFire,
        PlayerHealth,
    }

    [SerializeField] private StoryData _data;
    private TextDisplay _output;
    private static BeatData _currentBeat;
    private WaitForSeconds _wait;
    private string[] _upgrades = { "Increase projectile speed", "Increase player speed", "Increase projectile damage", "Increase rate of fire", "Increase player health" };

    public static Game _singleton;
    public static BeatData CurrentBeat { get { return _currentBeat; } }

    private void Awake()
    {
        _output = GetComponentInChildren<TextDisplay>();
        _currentBeat = null;
        _wait = new WaitForSeconds(0.5f);

        _singleton = this;        
    }

    private void Update()
    {
        if(_output.IsIdle)
        {
            if (_currentBeat == null)
            {
                DisplayBeat(1);
            }
            //else if (_currentBeat.ID > 10)
            //{
            //    DisplayBeat(8);
            //}
            else
            {
                UpdateInput();
            }
        }
    }

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.CurrentGameState == GameManager.State.Start)
        {
            if(_currentBeat != null)
            {
                if (_currentBeat.ID == 1)
                {
                    Application.Quit();
                }
                else
                {
                    DisplayBeat(1);
                }
            }
        }
        else
        {
            KeyCode alpha = KeyCode.Alpha1;
            KeyCode keypad = KeyCode.Keypad1;

            if (GameManager.CurrentGameState == GameManager.State.Start)
            {
                for (int count = 0; count < _currentBeat.Decision.Count; ++count)
                {
                    if (alpha <= KeyCode.Alpha9 && keypad <= KeyCode.Keypad9)
                    {
                        if (Input.GetKeyDown(alpha) || Input.GetKeyDown(keypad))
                        {
                            ChoiceData choice = _currentBeat.Decision[count];
                            DisplayBeat(choice.NextID);
                            break;
                        }
                    }

                    ++alpha;
                    ++keypad;
                }
            }
            else
            {
                for (int count = 0; count < _upgrades.Length; ++count)
                {
                    if (alpha <= KeyCode.Alpha9 && keypad <= KeyCode.Keypad9)
                    {
                        if (Input.GetKeyDown(alpha) || Input.GetKeyDown(keypad))
                        {
                            if (count == (int)Upgrade.ProjectileSpeed)
                            {
                                GameManager.ProjectileSpeed *= 1.3f;
                                GameManager.ChoosingUpgrade = false;
                                Debug.Log("ProjSpeed");
                            }
                            else if (count == (int)Upgrade.MovementSpeed)
                            {
                                GameManager.PlayerSpeed *= 1.2f;
                                GameManager.ChoosingUpgrade = false;
                                Debug.Log("PlayerSpeed");
                            }
                            else if (count == (int)Upgrade.ProjectileDamage)
                            {
                                GameManager.PlayerDamage *= 2;
                                GameManager.ChoosingUpgrade = false;
                                Debug.Log("ProjDamage");
                            }
                            else if (count == (int)Upgrade.RateOfFire)
                            {
                                GameManager.PlayerFireRate -= 0.04f;
                                GameManager.ChoosingUpgrade = false;
                                Debug.Log("FireRate");
                            }
                            else if (count == (int)Upgrade.PlayerHealth)
                            {
                                GameManager.PlayerStartingHealth += (int)(GameManager.PlayerStartingHealth * 0.5f);
                                GameManager.ChoosingUpgrade = false;
                                HealthBar._healthSlider.maxValue = GameManager.PlayerStartingHealth;
                                Debug.Log("PlayerHealth");
                            }
                        }
                    }
                    ++alpha;
                    ++keypad;
                }
            }
        }
    }

    public void DisplayBeat(int id)
    {
        BeatData data = _data.GetBeatById(id);
        StartCoroutine(DoDisplay(data));
        _currentBeat = data;
    }

    private IEnumerator DoDisplay(BeatData data)
    {
        _output.Clear();

        //if (GameManager._currentGameState == GameManager.State.Start)
        //{
            
        //}
        while (_output.IsBusy)
        {
            yield return null;
        }

        _output.Display(data.DisplayText);

        if (GameManager.CurrentGameState == GameManager.State.Start)
        {
            while (_output.IsBusy)
            {
                yield return null;
            }
        }
        if (GameManager.CurrentGameState == GameManager.State.Start)
        {
            for (int count = 0; count < data.Decision.Count; ++count)
            {

                ChoiceData choice = data.Decision[count];
                _output.Display(string.Format("{0}: {1}", (count + 1), choice.DisplayText));

                while (_output.IsBusy)
                {
                    yield return null;
                }
            }
        }
        else
        {
            for (int count = 0; count < _upgrades.Length + 1; ++count)
            {
                if (count == 0)
                {
                    _output.Display(" ");
                }
                else
                {
                    _output.Display(string.Format("{0}: {1}", count, _upgrades[count - 1]));
                }
                
                while (_output.IsBusy)
                {
                    yield return null;
                }
            }
        }
        

        if(data.Decision.Count > 0)
        {
            _output.ShowWaitingForInput();
        }
    }
}
