using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_boss : MonoBehaviour
{
    GameObject boss,hudboss;
    // Start is called before the first frame update
    void Start()
    {
        hudboss = GameObject.FindGameObjectWithTag("bossHud");
        boss = GameObject.Find("Boss 1");
        boss.SetActive(false);
        hudboss.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            boss.SetActive(true);
            hudboss.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
