using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chao : MonoBehaviour
{
    Collider2D chaofisics;
    GameObject player;
    float posicao;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("personagem teste");
        chaofisics = GetComponent<Collider2D>();
        chaofisics.usedByComposite = true;
    }

    // Update is called once per frame
    /*void Update()
    {
        posicao = player.transform.position.y - transform.position.y;
        if (posicao > 0.6f)
        {
            
        }
        else
        {
            
            
        }
    } //*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //chaofisics.composite.isTrigger = true;
            chaofisics.usedByComposite = false;
            chaofisics.isTrigger = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //chaofisics.composite.isTrigger = true;
            chaofisics.usedByComposite = false;
            chaofisics.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //chaofisics.composite.isTrigger = false;
            chaofisics.usedByComposite = true;
            chaofisics.isTrigger = false;
        }
    }
}
