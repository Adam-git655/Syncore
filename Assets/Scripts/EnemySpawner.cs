using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Hallucination1;

    [SerializeField]
    private GameObject Hallucination2;

    private float Hallucination1SpawnInterval = 1.8f;

    public bool HasStartedPlaying = false;
    private bool CanStartSpawningEnemies = true;

    private float TimePassed = 0f;
    public static float nextScaleTime = 20f;
    public static float ScaleInterval = 20f;

    public static int MaxHealth = 30;

    private void Start()
    {
        SetEnemyBaseStats();
    }

    private void Update()
    {
        if (HasStartedPlaying && CanStartSpawningEnemies)
        {
            StartCoroutine(SpawnEnemy(Hallucination1SpawnInterval));
            CanStartSpawningEnemies = false;
        }
        if (HasStartedPlaying)
        {
            TimePassed += Time.deltaTime;
            if (TimePassed >= nextScaleTime)
            {
                IncreaseEnemyStats();
                nextScaleTime += ScaleInterval;
            }
        }
    }

    private void SetEnemyBaseStats()
    {
        Hallucination1.GetComponent<Enemy>().Health = 10;
        Hallucination1.GetComponent<Enemy>().Damage = 3;
        Hallucination1.GetComponent<Enemy>().Movespeed = 2;
        Hallucination1.GetComponent<Enemy>().AttackCooldown = 2f;

        Hallucination2.GetComponent<Enemy>().Health = 10;
        Hallucination2.GetComponent<Enemy>().Damage = 5;
        Hallucination2.GetComponent<Enemy>().Movespeed = 4;
        Hallucination2.GetComponent<Enemy>().AttackCooldown = 1.8f;
    }

    private void IncreaseEnemyStats()
    {
        Hallucination1.GetComponent<Enemy>().Health += 5;
        Hallucination1.GetComponent<Enemy>().Movespeed += 1f;
        GameManager.NoOfContinousHitsForExtraEnemySpawns += 2;
        Hallucination1SpawnInterval -= 0.3f;

        Hallucination2.GetComponent<Enemy>().Health += 5;
        Hallucination2.GetComponent<Enemy>().Movespeed += 0.5f;

        if (Hallucination1.GetComponent<Enemy>().Health > MaxHealth)
        {
            Hallucination1.GetComponent<Enemy>().Health = MaxHealth;
        }
        if (Hallucination2.GetComponent<Enemy>().Health > MaxHealth)
        {
            Hallucination2.GetComponent<Enemy>().Health = MaxHealth;
        }
    }


    IEnumerator SpawnEnemy(float interval)
    {
        yield return new WaitForSeconds(interval);
        SpawnHallucination1();
        StartCoroutine(SpawnEnemy(interval));
    }


    public void SpawnHallucination1()
    {
        GameObject instance = Instantiate(Hallucination1, new Vector3(Random.Range(-8f, 8f), Random.Range(5f, 1f), 0f), transform.rotation);
        instance.transform.SetParent(transform);
    }


    //Only spawn hallucination2 on missing a beat
    public IEnumerator SpawnHallucination2()
    {
        GameObject instance = Instantiate(Hallucination2, new Vector3(Random.Range(-8f, 8f), Random.Range(5f, 2.5f), 0f), transform.rotation);
        instance.transform.SetParent(transform);
        yield return new WaitForSeconds(0.09f);
    }
}

