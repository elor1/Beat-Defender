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

    /*private AudioSource _audioSource;
    private int _numSamples = 1024;
    private int _numFrequencyBands = 16;
    [SerializeField] private float _sampleMultipler = 10.0f;

    private float[] _samples;
    private float[] _spectrum;
    private float[] _frequencyBands;
    private float[] _frequencyBandBuffer;
    private float[] _bufferDecrease;

    [SerializeField] private float _buffer = 0.005f;
    [SerializeField] private float _bufferMultiplier = 1.2f;

    private float[] _maxFrequency;
    private float[] _audioBand;
    private float[] _audioBandBuffer;

    //[SerializeField] private int _lowBandIndex = 1;
    //[SerializeField] private int _highBandIndex = 9;
    //private int _lowIndexCount = 0;
    //private int _highIndexCount = 0;
    //private int _tempLowIndexCount = 0;
    //private int _tempHighIndexCount = 0;
    //private int[] _tempCount = new int[16];
    //private int[] _count = new int[16];

    //private float _intensityMultiplier = 100.0f;
    //public static float _intensity = 0;
    //private float _intensityLerpDuration = 5.0f;

    private float _averageAmplitude;
    private float _currentAmplitude;

    [SerializeField] private float _beatThreshold = 0.6f;

    private float _beatCountFrequency = 0.2f; //Time between each beat count is done
    private float _timer;

    private void Start()
    {
        _audioSource = GameManager._audioSource;
        _samples = new float[_numSamples];
        _spectrum = new float[_numSamples];
        _frequencyBands = new float[_numFrequencyBands];
        _frequencyBandBuffer = new float[_numFrequencyBands];
        _bufferDecrease = new float[_numFrequencyBands];

        _maxFrequency = new float[_numFrequencyBands];
        _audioBand = new float[_numFrequencyBands];
        _audioBandBuffer = new float[_numFrequencyBands];

        _timer = _beatCountFrequency;

        //StartCoroutine(LerpIntensity());
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        GetSpecrumData();
        CreateFrequencyBands();
        SetBandBuffer();
        CreateAudioBands();
        GetAmplitude();
        //CalculateBeatsPerSecond(_lowBandIndex, ref _lowIndexCount);
        //CalculateBeatsPerSecond(_highBandIndex, ref _highIndexCount);
        //if (IsBeat(_lowBandIndex))
        //{
        //    _tempLowIndexCount++;
        //}

        //if (IsBeat(_highBandIndex))
        //{
        //    _tempHighIndexCount++;
        //}

    //    for (int i = 0; i < _numFrequencyBands; i++)
    //    {
    //        if (IsBeat(i))
    //        {
    //            _tempCount[i]++;
    //        }
    //    }

    //    if (_timer <= 0.0f)
    //    {
    //        //_lowIndexCount = _tempLowIndexCount;
    //        //_highIndexCount = _tempHighIndexCount;
    //        //_tempLowIndexCount = 0;
    //        //_tempHighIndexCount = 0;

    //        //float endValue = (_lowIndexCount + _highIndexCount) * _intensityMultiplier;

    //        float endValue = 0;
    //        for (int i = 0; i < _numFrequencyBands; i++)
    //        {
    //            _count[i] = _tempCount[i];
    //            _tempCount[i] = 0;

    //            endValue += _count[i];
    //        }

    //        if ( _intensity > endValue)
    //        {
    //            _intensity = Mathf.Lerp(_intensity, endValue, 0.25f);
    //        }
    //        else
    //        {
    //            _intensity = 1 / Mathf.Lerp(_intensity, endValue, 0.75f);
    //        }

    //        _intensity *= _intensityMultiplier;

    //        _timer = _beatCountFrequency;
    //        Debug.Log(_intensity);
    //    }


    }

    private void GetSpecrumData()
    {
        _audioSource.GetOutputData(_samples, 0);
        _audioSource.GetSpectrumData(_spectrum, 0, FFTWindow.Blackman);
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
                average += _spectrum[band] * (band + 1);
                band++;
            }

            average /= sampleCount;
            _frequencyBands[i] = average;
        }
    }

    /// <summary>
    /// Divides all frequency values by max frequency so that frequency bands have a value between 0 and 1
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

    private bool IsBeat(int index)
    {
        if (_audioBandBuffer[index] >= _beatThreshold)
        {
            return true;
        }

        return false;
    }

    private void GetAmplitude()
    {
        float amplitude = 0.0f;
        for (int i = 0; i < _numSamples; i++)
        {
            amplitude = _samples[i] * _samples[i];
        }
        _averageAmplitude = Mathf.Sqrt(amplitude / _numSamples);
        _currentAmplitude = 20 * Mathf.Log10(_currentAmplitude / 0.1f);
    }

    //private IEnumerator LerpIntensity()
    //{
    //    float timePassed = 0.0f;

    //    while (timePassed < _intensityLerpDuration)
    //    {
    //        float endValue = (_lowIndexCount + _highIndexCount) * _intensityMultiplier;
    //        _intensity = Mathf.Lerp(_intensity, endValue, 0.5f);

    //        timePassed += Time.deltaTime;

    //        yield return null;
    //    }

    //   // _intensity = (_lowIndexCount + _highIndexCount) * _intensityMultiplier;
    //}*/

    private AudioSource _audioSource;

    private const int NUM_SAMPLES = 1024;
    [SerializeField] private const float BEAT_THRESHOLD = 0.15f;
    private const float AMPLITUDE_MULTIPLIER = 0.1f;
    private const int SAMPLE_RATE = 44100;

    private float[] _samples;
    private float[] _spectrum;

    public static float _averageAmplitude = 0.0f;
    public static float _currentAmplitude = 0;
    private static int _amplitudeCount = 0;

    public static float _loudestFrequency;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _samples = new float[NUM_SAMPLES];
        _spectrum = new float[NUM_SAMPLES];
    }

    private void Update()
    {
        GetAudioData();
        CalculateAmplitude();
  
        if (_currentAmplitude > _averageAmplitude && (_currentAmplitude - _averageAmplitude) > BEAT_THRESHOLD)
        {
            //BEAT
            ScaleCubes.RaiseWalls();
        }

        _loudestFrequency = FindLoudestFrequency();
    }

    private void GetAudioData()
    {
        _audioSource.GetOutputData(_samples, 0);
        _audioSource.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
    }

    private void CalculateAmplitude()
    {
        _amplitudeCount++;
        float total = 0.0f;
        foreach (float sample in _spectrum)
        {
            total += sample;
        }

        _averageAmplitude += (total - _averageAmplitude) / _amplitudeCount;
        _currentAmplitude = total;
        Debug.Log(_currentAmplitude);
    }

    public static void ResetAmplitude()
    {
        _averageAmplitude = 0.0f;
        _currentAmplitude = 0.0f;
        _amplitudeCount = 0;
    }

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
