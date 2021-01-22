using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnalyser : MonoBehaviour
{
    private const int NUM_SAMPLES = 1024; //Number of samples taken each time the spectrum data is analysed
    private const float BEAT_THRESHOLD = 0.15f; //Difference in amplitude needed for a beat to be detected

    private AudioSource _audioSource; //Audio source to analyse
    private float[] _spectrum; //Stores spectrum data of the current song
    private static float _averageAmplitude; //Running average of amplitude as song plays
    private static float _currentAmplitude; //Current amplitude of the playing song
    private static int _amplitudeCount; //Number of times amplitude has been calculated for this song
    private static float _loudestFrequency; //The current loudest frequency in the audio spectrum

    public static float AverageAmplitude { get { return _averageAmplitude; } }
    public static float CurrentAmplitude { get { return _currentAmplitude; } }
    public static float LoudestFrequency { get { return _loudestFrequency; } }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _spectrum = new float[NUM_SAMPLES];

        _averageAmplitude = 0.0f;
        _currentAmplitude = 0.0f;
        _amplitudeCount = 0;
    }

    private void Update()
    {
        GetAudioData();
        CalculateAmplitude();
  
        //If the current amplitude is higher than the average by more than the threshold, a beat has occured
        if (_currentAmplitude > _averageAmplitude && (_currentAmplitude - _averageAmplitude) > BEAT_THRESHOLD)
        {
            //Raise walls around player when a beat is detected
            ScaleCubes.RaiseWalls();
        }

        _loudestFrequency = FindLoudestFrequency();
    }

    /// <summary>
    /// Gets spectrum data from the current song
    /// </summary>
    private void GetAudioData()
    {
        _audioSource.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
    }

    /// <summary>
    /// Calculates current and average amplitudes for the current song
    /// </summary>
    private void CalculateAmplitude()
    {
        _amplitudeCount++;

        //Add the amplitude of each sample in the spectrum to get the total current amplitude
        float total = 0.0f;
        foreach (float sample in _spectrum)
        {
            total += sample;
        }

        _averageAmplitude += (total - _averageAmplitude) / _amplitudeCount;
        _currentAmplitude = total;
    }

    /// <summary>
    /// Sets amplitude variables back to 0
    /// </summary>
    public static void ResetAmplitude()
    {
        _averageAmplitude = 0.0f;
        _currentAmplitude = 0.0f;
        _amplitudeCount = 0;
    }

    /// <summary>
    /// Finds the sample in the spectrum array with the highest amplitude
    /// </summary>
    /// <returns>The index of the loudest frequency in the spectrum array</returns>
    private int FindLoudestFrequency()
    {
        int maxIndex = 0;
        for (int i = 0; i < NUM_SAMPLES; i++)
        {
            if (_spectrum[i] > _spectrum[maxIndex])
            {
                maxIndex = i;
            }
        }
        return maxIndex / 128;
    }
}
