using Melanchall.DryWetMidi.Interaction;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BeatLane : MonoBehaviour
{

    public Melanchall.DryWetMidi.MusicTheory.NoteName BeatRestriction;
    public KeyCode input;
    public GameObject BeatPrefab;
    List<Beat> beats = new List<Beat>();
    public List<double> timeStamps = new List<double>();

    int SpawnIndex = 0;
    int InputIndex = 0;

    public bool HasStartedPlaying = false;  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var beat in array)
        {
            if (beat.NoteName == BeatRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan> (beat.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HasStartedPlaying && !SongManager.instance.IsGameOver)
        {
            if (SpawnIndex < timeStamps.Count)
            {
                if (SongManager.GetAudioSourceTime() >= timeStamps[SpawnIndex] - SongManager.instance.BeatTime)
                {
                    var beat = Instantiate(BeatPrefab, transform.parent);
                    beat.transform.SetParent(transform);
                    beat.transform.position = new Vector3(10.5f, -3.7f, 0f);
                    beats.Add(beat.GetComponent<Beat>());
                    beat.GetComponent<Beat>().AssignedTime = (float)timeStamps[SpawnIndex];
                    SpawnIndex++;
                }
            }

            if (InputIndex < timeStamps.Count)
            {
                double timeStamp = timeStamps[InputIndex];
                double MarginOfError = SongManager.instance.MarginOfError;
                double AudioTime = SongManager.GetAudioSourceTime() - (SongManager.instance.InputDelayInMilliseconds / 1000.0);

                if (Input.GetKeyDown(input))
                {
                    if (Math.Abs(AudioTime - timeStamp) < MarginOfError)
                    {
                        GameManager.instance.HitBeat(); 
                        Destroy(beats[InputIndex].gameObject);
                        InputIndex++;
                    }
                    else
                    {
                        GameManager.instance.MissedBeat();
                    }
                }
                if (timeStamp + MarginOfError <= AudioTime)
                {
                    GameManager.instance.MissedBeat();
                    InputIndex++;
                }
            }
        }
    }
}
