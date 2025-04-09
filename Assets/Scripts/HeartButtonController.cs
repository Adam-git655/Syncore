using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class HeartButtonController : MonoBehaviour
{

    public AudioSource MainMusic;
    public AudioSource HeartBeatFlatlineMusic;

    public GameObject enemySpawner;

    public Rigidbody2D rb;

    public int HeartHealth = 200;

    public HealthBar HealthBar;

    private bool isGameOver = false;

    private bool musicFadeOutEnabled = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HealthBar.SetMaxHealth(HeartHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.localScale += new Vector3(0.2f, 0.2f, 0f);

            }
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                transform.localScale = new Vector3(0.5f, 0.5f, 0f);
            }


            if (HeartHealth <= 0)
            {
                SongManager.instance.IsGameOver = true;
                musicFadeOutEnabled = true;
            }

            if (musicFadeOutEnabled)
            {
                if (MainMusic.volume <= 0.1f)
                {
                    MainMusic.Stop();
                    StartCoroutine(OnGameOver());
                    musicFadeOutEnabled = false;
                }
                else
                {
                    float newVol = MainMusic.volume - (0.3f * Time.deltaTime);
                    if (newVol < 0f)
                    {
                        newVol = 0f;
                    }
                    MainMusic.volume = newVol;
                }
            }
        }
    }

    IEnumerator OnGameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            HeartBeatFlatlineMusic.Play();
            yield return new WaitForSeconds(1f);
            transform.localScale += new Vector3(0.3f, 0.3f, 0f);
            yield return new WaitForSeconds(0.1f);
            transform.localScale = new Vector3(0.5f, 0.5f, 0f);
            yield return new WaitForSeconds(3.8f);

            transform.localScale += new Vector3(0.3f, 0.3f, 0f);
            yield return new WaitForSeconds(0.1f);
            transform.localScale = new Vector3(0.5f, 0.5f, 0f);

            yield return new WaitForSeconds(3f);
            rb.bodyType = RigidbodyType2D.Dynamic;

            for (int i = 0; i < enemySpawner.transform.childCount; i++)
            {
                enemySpawner.transform.GetChild(i).GetComponent<Enemy>().isGameOver = true;
            }

            Vector2 force = new Vector2(Random.Range(-0.5f, 0.5f), 2.5f);
            rb.AddForce(force, ForceMode2D.Impulse);

            rb.AddTorque(Random.Range(-1f, 1f), ForceMode2D.Impulse);

            yield return new WaitForSeconds(4.5f);
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
