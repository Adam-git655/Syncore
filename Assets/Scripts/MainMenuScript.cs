using Melanchall.DryWetMidi.Core;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    public GameObject TracksMenuBackground;
    public List<Toggle> toggles = new();

    public static string TrackChoice = "DeadLoop";

    private void Start()
    {
        foreach(var toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate
            {
                OnToggleValueChanged(toggle);

            });
        }
        TracksMenuBackground.SetActive(false);
    }

    void OnToggleValueChanged(Toggle toggle)
    {
        if (toggle.name == "DeadLoopToggle")
        {
            if (toggle.isOn)
            {
                TrackChoice = "DeadLoop";            
            }
        }
        else if (toggle.name == "HellPhonkToggle")
        {
            if (toggle.isOn)
            {
                TrackChoice = "HellPhonk";
            }
        }
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ChooseSongButton()
    {
        TracksMenuBackground.SetActive(true);
        
    }

    public void BackButton()
    {
        TracksMenuBackground.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
