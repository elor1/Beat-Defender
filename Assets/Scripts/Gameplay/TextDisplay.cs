﻿using System.Collections;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    public enum State { Initialising, Idle, Busy }

    private TMP_Text _displayText;
    private string _displayString;
    private WaitForSeconds _shortWait;
    private WaitForSeconds _longWait;
    private State _state = State.Initialising;

    public bool IsIdle { get { return _state == State.Idle; } }
    public bool IsBusy { get { return _state != State.Idle; } }

    private void Awake()
    {
        _displayText = GetComponent<TMP_Text>();
        _shortWait = new WaitForSeconds(0.03f);
        _longWait = new WaitForSeconds(0.8f);

        _displayString = string.Empty;
        _displayText.text = _displayString;
        _state = State.Idle;
    }

    private void Update()
    {
        if (GameManager.CurrentGameState == GameManager.State.WaveEnd)
        {
            _shortWait = new WaitForSeconds(0.01f);
        }
        else
        {
            _shortWait = new WaitForSeconds(0.03f);
        }

        if (Game.CurrentBeat != null)
        {
            if (Game.CurrentBeat.ID <= 2)
            {
                _displayText.fontSize = 100;
            }
            else if (Game.CurrentBeat.ID == 3)
            {
                _displayText.fontSize = 60;
            }
            else
            {
                _displayText.fontSize = 86;
            }
        }
        
    }

    private IEnumerator DoShowText(string text)
    {
        int currentLetter = 0;
        char[] charArray = text.ToCharArray();

        while (currentLetter < charArray.Length)
        {
            _displayText.text += charArray[currentLetter++];
            yield return _shortWait;
        }

        _displayText.text += "\n";
        _displayString = _displayText.text;
        _state = State.Idle;
    }

    private IEnumerator DoAwaitingInput()
    {
        bool on = true;

        while (enabled)
        {
            _displayText.text = string.Format( "{0}> {1}", _displayString, ( on ? "|" : " " ));
            on = !on;
            yield return _longWait;
        }
    }

    private IEnumerator DoClearText()
    {
        int currentLetter = 0;
        char[] charArray = _displayText.text.ToCharArray();

        while (currentLetter < charArray.Length)
        {
            if (currentLetter > 0 && charArray[currentLetter - 1] != '\n')
            {
                charArray[currentLetter - 1] = ' ';
            }

            if (charArray[currentLetter] != '\n')
            {
                charArray[currentLetter] = '_';
            }

            _displayText.text = charArray.ArrayToString();
            ++currentLetter;
            yield return null;
        }

        _displayString = string.Empty;
        _displayText.text = _displayString;
        _state = State.Idle;
    }

    public void Display(string text)
    {
        if (_state == State.Idle)
        {
            StopAllCoroutines();
            _state = State.Busy;
            StartCoroutine(DoShowText(text));
        }
    }

    public void ShowWaitingForInput()
    {
        if (_state == State.Idle)
        {
            StopAllCoroutines();
            StartCoroutine(DoAwaitingInput());
        }
    }

    public void Clear()
    {
        if (_state == State.Idle)
        {
            if (GameManager.CurrentGameState == GameManager.State.Start)
            {
                StopAllCoroutines();
                _state = State.Busy;
                StartCoroutine(DoClearText());
            }
            else
            {
                _displayString = string.Empty;
                _displayText.text = _displayString;
                _state = State.Idle;
            }
        }
    }

}
