using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class SongManager : MonoBehaviour
{
    public static SongManager instance;
    public AudioSource music;
    public AudioClip DeadLoopClip;
    public AudioClip HellPhonkClip;
    public BeatLane[] beatLanes;
    public float SongDelayInSeconds;
    public double MarginOfError; //In Seconds
    public int InputDelayInMilliseconds;
    

    public string FileLocation;
    public float BeatTime;
    public float BeatSpawnXRight;
    public float BeatSpawnXLeft;
    public float BeatTapX;

    public float BeatDespawnXRight
    {
        get
        {
            return BeatTapX - 1f;
        }
    }

    public float BeatDespawnXLeft
    {
        get
        {
            return BeatTapX + 1f;
        }
    }

    public static MidiFile midiFile;

    public bool HasStartedPlaying = false;
    public bool IsGameOver = false;
    private bool CanStartStuff = true;

    private void Start()
    {
        if (MainMenuScript.TrackChoice == "DeadLoop")
        {
            music.clip = DeadLoopClip;
            FileLocation = "song2_chart.mid";
            EnemySpawner.ScaleInterval = 20f;
            EnemySpawner.nextScaleTime = 20f;
            GameManager.NoOfExtraEnemySpawnsOnContinousHits = 3;
            GameManager.NoOfContinousHitsForExtraEnemySpawns = 20;
            EnemySpawner.MaxHealth = 30;
        }
        else if (MainMenuScript.TrackChoice == "HellPhonk")
        {
            music.clip = HellPhonkClip;
            FileLocation = "song_chart.mid";
            EnemySpawner.ScaleInterval = 25f;
            EnemySpawner.nextScaleTime = 25f;
            GameManager.NoOfExtraEnemySpawnsOnContinousHits = 3;
            GameManager.NoOfContinousHitsForExtraEnemySpawns = 15;
            EnemySpawner.MaxHealth = 20;
        }
    }

    private void Update()
    {
        if (HasStartedPlaying && CanStartStuff)
        {
            instance = this;
            if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
            {
                StartCoroutine(ReadFromWebsite());
            }
            else
            {
                ReadfromFile();
            }
            CanStartStuff = false;
        }
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + FileLocation))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);  
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    private void ReadfromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + FileLocation);
        GetDataFromMidi();
    }

    public void GetDataFromMidi()
    {
        var beats = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[beats.Count];
        beats.CopyTo(array, 0);

        foreach (var beatLane in beatLanes)
        {
            beatLane.HasStartedPlaying = true;
            beatLane.SetTimeStamps(array);
        }

        Invoke(nameof(StartSong), SongDelayInSeconds);
    }

    public void StartSong()
    {
        music.Play();
    }

    private void LateUpdate()
    {
        if (!music.isPlaying && HasStartedPlaying && !IsGameOver)
        {
            SceneManager.LoadScene("GameWonScene");
        }
    }

    public static double GetAudioSourceTime()
    {
        return (double)instance.music.timeSamples / instance.music.clip.frequency;
    }


}
