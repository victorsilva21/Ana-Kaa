using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ia_inimigo1 : MonoBehaviour
{
    public GameObject tiroObj;
    float tempo = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
       

    }
    private void Update()
    {
        if(tempo > 0)
        {
            tempo -= Time.deltaTime;
        }
        else
        {
            Instantiate(tiroObj, transform.position, Quaternion.identity);
            tempo = 1.5f;
        }
        
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {

       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ataque"))
        {
            
            Destroy(this.gameObject);

        }
    }
}
