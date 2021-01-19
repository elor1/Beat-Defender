using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    private const float PROJECTILE_SPEED_MULTIPLIER = 1.3f; //Multiplier for projectile speed upgrade
    private const float PLAYER_SPEED_MULTIPLIER = 1.2f; //Multiplier for player speed upgrae
    private const int PLAYER_DAMAGE_MULTIPLIER = 2; //Multiplier for player damage upgrade
    private const float RATE_OF_FIRE_MULTIPLIER = 0.04f; //Multiplier for rate of fire upgrade
    private const float PLAYER_HEALTH_MULTIPLIER = 0.4f; //Multiplier for player health upgrade

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
                            //Upgrades
                            UpgradeStats(count);
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

    private void UpgradeStats(int choice)
    {
        if (choice == (int)Upgrade.ProjectileSpeed)
        {
            GameManager.ProjectileSpeed *= PROJECTILE_SPEED_MULTIPLIER;
            GameManager.ChoosingUpgrade = false;
        }
        else if (choice == (int)Upgrade.MovementSpeed)
        {
            GameManager.PlayerSpeed *= PLAYER_SPEED_MULTIPLIER;
            GameManager.ChoosingUpgrade = false;
        }
        else if (choice == (int)Upgrade.ProjectileDamage)
        {
            GameManager.PlayerDamage *= PLAYER_DAMAGE_MULTIPLIER;
            GameManager.ChoosingUpgrade = false;
        }
        else if (choice == (int)Upgrade.RateOfFire)
        {
            GameManager.PlayerFireRate -= RATE_OF_FIRE_MULTIPLIER;
            GameManager.ChoosingUpgrade = false;
        }
        else if (choice == (int)Upgrade.PlayerHealth)
        {
            GameManager.PlayerStartingHealth += (int)(GameManager.PlayerStartingHealth * PLAYER_HEALTH_MULTIPLIER);
            GameManager.ChoosingUpgrade = false;
            HealthBar._healthSlider.maxValue = GameManager.PlayerStartingHealth;
        }
    }
}
