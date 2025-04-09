using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Text PressAnyButtonToPlayText;

    public GameObject MainCam;
    public AnimationCurve Curve;

    public GameObject SongManager;

    public GameObject EnemySpawner;

    public GameObject TextEffectPrefab;

    private bool HasStartedPlaying = false;

    public static GameManager instance;

    public int DamageToHallucination1 = 10;

    public AudioSource HeartBeatSoundEffect;

    private int ContinousHitCount = 0;
    public static int NoOfContinousHitsForExtraEnemySpawns = 20;
    public static int NoOfExtraEnemySpawnsOnContinousHits = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        PressAnyButtonToPlayText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!HasStartedPlaying)
        {
            if (Input.anyKeyDown)
            {
                HasStartedPlaying = true;
                PressAnyButtonToPlayText.gameObject.SetActive(false);
                SongManager.GetComponent<SongManager>().HasStartedPlaying = true;
                EnemySpawner.GetComponent<EnemySpawner>().HasStartedPlaying = true;
            }
        }   
    }

    public void HitBeat()
    {
        ContinousHitCount++;
        HeartBeatSoundEffect.Play();
        StartCoroutine(Screenshake(0.2f));
        if (EnemySpawner.transform.childCount > 0)
        {
            EnemySpawner.transform.GetChild(0).GetComponent<Enemy>().Health -= DamageToHallucination1;
        }

        if (ContinousHitCount == NoOfContinousHitsForExtraEnemySpawns)
        {
            for (int i = 0; i < NoOfExtraEnemySpawnsOnContinousHits; i++)
            {
                EnemySpawner.GetComponent<EnemySpawner>().SpawnHallucination1();
            }
            ContinousHitCount = 0;
        }
    }

    IEnumerator Screenshake(float duration)
    {
        Vector3 StartPos = MainCam.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = Curve.Evaluate(elapsedTime / duration);
            MainCam.transform.position = StartPos + Random.insideUnitSphere * strength;
            yield return null;
        }

        MainCam.transform.position = StartPos;
    }

    public void MissedBeat()
    {
        ContinousHitCount = 0;
 
        Instantiate(TextEffectPrefab, GameObject.Find("Canvas").transform);

        StartCoroutine(EnemySpawner.GetComponent<EnemySpawner>().SpawnHallucination2());

    }

}
