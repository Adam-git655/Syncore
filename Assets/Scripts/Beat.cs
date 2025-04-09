using UnityEngine;

public class Beat : MonoBehaviour
{
    double TimeInstatiated;
    public float AssignedTime;
    public float BeatYPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TimeInstatiated = SongManager.GetAudioSourceTime();  
    }

    // Update is called once per frame
    void Update()
    {
        double TimeSinceInstantiated = SongManager.GetAudioSourceTime() - TimeInstatiated;
        float t = (float)(TimeSinceInstantiated / (SongManager.instance.BeatTime + 0.1f));

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {

            if (transform.parent.name == "HeartBeatLineRight")
            {
                transform.position = Vector3.Lerp(new Vector3(1f * SongManager.instance.BeatSpawnXRight, BeatYPosition, 0f), new Vector3(1f * SongManager.instance.BeatDespawnXRight, BeatYPosition, 0f), t);
            }
            if (transform.parent.name == "HeartBeatLineLeft")
            {
                transform.position = Vector3.Lerp(new Vector3(1f * SongManager.instance.BeatSpawnXLeft, BeatYPosition, 0f), new Vector3(1f * SongManager.instance.BeatDespawnXLeft, BeatYPosition, 0f), t);
            }

        }
    }
}
