using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnalyser : MonoBehaviour
{
    /*AudioSource audioSource;
    public float[] outputData = new float[1024];
    public float[] samples = new float[512];
    public static float[] frequencyBands = new float[8];
    //public static float amplitude = 0.0f;
    //float totalAmplitude = 0.0f;
    //public float runningAmplitude = 0.0f;
    //int amplitudeCounter = 0;
    //public float currentAmplitude = 0.0f;
    //float timeSinceLastBeat = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //timeSinceLastBeat += Time.deltaTime;
        GetSpecrumData();
        SetFrequencyBands();
        //CalculateBPM();

        //totalAmplitude += GetAmplitude();
        //amplitudeCounter++;
        //runningAmplitude = totalAmplitude / amplitudeCounter;
        //if (amplitudeCounter >= (1 / Time.deltaTime) * 10)
        //{
        //    totalAmplitude = 0.0f;
        //    amplitude = 0;
        //}
    }

    void GetSpecrumData()
    {
        audioSource.GetOutputData(outputData, 0);
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void SetFrequencyBands()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            float average = 0.0f;

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;

            frequencyBands[i] = average * 10.0f;
        }
    }

    public static float GetAmplitude()
    {
        float amplitude = 0.0f;
        foreach (float sample in frequencyBands)
        {
            amplitude += sample;
        }
        Debug.Log(amplitude);
        return amplitude;
    }

   
    public float threshold = 1.3f;
    public float averageAmplitude = 0.0f;
    public void CalculateBPM()
    {
        //averageAmplitude = 0.0f;
        //foreach (float sample in sampleBuffer)
        //{
        //    averageAmplitude += sample;
        //}
        //currentAmplitude = GetAmplitude();
        //if (frequencyBands[2] > runningAmplitude * threshold)
        //{
        //    Debug.Log("BEAT");
        //    //player.transform.position += new Vector3(0.0f, 0.05f, 0.0f);
        //}
    }*/

    private AudioSource _audioSource;
    private int _numSamples = 512;
    private int _numFrequencyBands = 16;
    [SerializeField] private float _sampleMultipler = 10.0f;

    private float[] _samples;
    private float[] _frequencyBands;
    private float[] _frequencyBandBuffer;
    private float[] _bufferDecrease;

    [SerializeField] private float _buffer = 0.005f;
    [SerializeField] private float _bufferMultiplier = 1.2f;

    private float[] _maxFrequency;
    private float[] _audioBand;
    private float[] _audioBandBuffer;

    private void Start()
    {
        _audioSource = GameManager._audioSource;
        _samples = new float[_numSamples];
        _frequencyBands = new float[_numFrequencyBands];
        _frequencyBandBuffer = new float[_numFrequencyBands];
        _bufferDecrease = new float[_numFrequencyBands];

        _maxFrequency = new float[_numFrequencyBands];
        _audioBand = new float[_numFrequencyBands];
        _audioBandBuffer = new float[_numFrequencyBands];
    }

    private void Update()
    {
        GetSpecrumData();
        CreateFrequencyBands();
        SetBandBuffer();
        CreateAudioBands();
    }

    private void GetSpecrumData()
    {
        //_audioSource.GetOutputData(outputData, 0);
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    private void CreateFrequencyBands()
    {
        //int count = 0;

        //for (int i = 0; i < _numFrequencyBands; i++)
        //{
        //    float average = 0.0f;
        //    int sampleCount = (int)Mathf.Pow(2, i) * 2;

        //    if (i == _numFrequencyBands - 1)
        //    {
        //        sampleCount += 2;
        //    }

        //    for (int j = 0; j < sampleCount; j++)
        //    {
        //        average += _samples[count] * (count + 1);
        //        count++;
        //    }

        //    average /= count;

        //    _frequencyBands[i] = average * _sampleMultipler;
        //}

        int band = 0;
        for (int i = 0; i < _numFrequencyBands; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Lerp(2.0f, _numSamples - 1, i / ((float)_numFrequencyBands - 1));

            for (int j = band; j < sampleCount; j++)
            {
                average += _samples[band] * (band + 1);
                band++;
            }

            average /= sampleCount;
            _frequencyBands[i] = average;
        }
    }

    /// <summary>
    /// Makes frequency bands have a value between 0 and 1
    /// </summary>
    private void CreateAudioBands()
    {
        for (int i = 0; i < _numFrequencyBands; i++)
        {
            if (_frequencyBands[i] > _maxFrequency[i])
            {
                _maxFrequency[i] = _frequencyBands[i];
            }
            _audioBand[i] = _frequencyBands[i] / _maxFrequency[i];
            _audioBandBuffer[i] = _frequencyBandBuffer[i] / _maxFrequency[i];
        }
    }

    private void SetBandBuffer()
    {
        for (int i = 0; i < _numFrequencyBands; i++)
        {
            if (_frequencyBands[i] > _frequencyBandBuffer[i])
            {
                _frequencyBandBuffer[i] = _frequencyBands[i];
                _bufferDecrease[i] = _buffer;
            }
            else
            {
                _frequencyBandBuffer[i] -= _bufferDecrease[i];
                _bufferDecrease[i] *= _bufferMultiplier;
            }
        }
    }
}
