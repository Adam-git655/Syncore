using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject Heart;
    public Rigidbody2D rb;
    private HealthBar HealthBar;
    public float Movespeed;
    public int Health;
    public float AttackCooldown;
    public int Damage;

    public bool isGameOver = false;

    private bool CanMove = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Heart = GameObject.Find("Heart");
        HealthBar = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<HealthBar>();
        Vector3 direction = Heart.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 180);
    }

    // Update is called once per frame
    void Update()
    {

        if (isGameOver)
        {
            GetComponent<Collider2D>().enabled = false;
            transform.position = Vector3.MoveTowards(transform.position, Heart.transform.position, Movespeed * Time.deltaTime);
        }

        if (CanMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, Heart.transform.position, Movespeed * Time.deltaTime);
        }

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Heart"))
        {

            CanMove = false;
            StartCoroutine(Attack());
        }
    }


    IEnumerator Attack()
    {
        yield return new WaitForSeconds(AttackCooldown);
        Heart.GetComponent<HeartButtonController>().HeartHealth -= Damage;
        HealthBar.SetHealth(Heart.GetComponent<HeartButtonController>().HeartHealth);
        StartCoroutine(Attack());
    }

}