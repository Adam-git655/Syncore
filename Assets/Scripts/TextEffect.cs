using UnityEngine;

public class TextEffect : MonoBehaviour
{

    public float Lifespan = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, Lifespan);
    }
}
