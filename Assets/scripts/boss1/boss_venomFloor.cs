using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_venomFloor : MonoBehaviour
{
    SpriteRenderer imagem;
    BoxCollider2D colisor;

    // Start is called before the first frame update
    void Start()
    {
        colisor = gameObject.GetComponent<BoxCollider2D>();
        imagem = gameObject.GetComponent<SpriteRenderer>();
        imagem.color=  new Color(1,1,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.tag = "dano";
    }
    private void FixedUpdate()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            StartCoroutine(fade());
        }
    }
    IEnumerator fade()
    {
        colisor.enabled = false;
        do
        {
            imagem.color = new Color(imagem.color.r,imagem.color.g,imagem.color.b,imagem.color.a + 0.05f);
            yield return new WaitForSeconds(0.01f);
        } while (imagem.color.a < 1);
        gameObject.tag = "dano";
        colisor.enabled = true;
        print(gameObject.tag);
        yield return new WaitForSeconds(5);
        do
        {
            imagem.color = new Color(imagem.color.r, imagem.color.g, imagem.color.b, imagem.color.a - 0.05f);
            yield return new WaitForSeconds(0.01f);
        } while (imagem.color.a > 0);
        gameObject.tag = "Untagged";
    }

}
