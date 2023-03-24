using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projetil : MonoBehaviour
{
    GameObject cano;
    // Start is called before the first frame update
    void Start()
    {
        cano = GameObject.Find("Personagem");
        if(cano.transform.localScale.x > 0)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0,0,-180));
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(24 * Time.deltaTime, 0));
        if (Vector2.Distance(transform.position, cano.transform.position) > 15)
        {
            Destroy(this.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.layer == 11)
        {
            Destroy(this.gameObject);
        }
    }

}
