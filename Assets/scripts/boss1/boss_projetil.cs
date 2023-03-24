using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_projetil : MonoBehaviour
{
    [SerializeField] float numero,numero2;
    float posicao,seno,angulo;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicao = GameObject.Find("Personagem").transform.position.x;
        float distancia = Vector2.Distance(GameObject.Find("Personagem").transform.position, gameObject.transform.position);
        seno = (distancia / (1.58f * 1.58f)) / (9.81f * 2);
        angulo = Mathf.Asin(seno) * Mathf.Rad2Deg;
        print(angulo);
        transform.rotation = Quaternion.Euler(-180, 0, angulo);
        Vector3 movimento = new Vector3(-16.5f, 0.0f, 0);
        movimento = transform.TransformDirection(movimento);
        rb.velocity = (movimento);
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(transform.rotation.z > -90)
        {
            transform.Rotate(0, 0, -30 * Time.deltaTime);
        }

       //rotacao z = -90
        //numero += Time.deltaTime;
        // transform.position = new Vector2(numero, (-numero * numero) + posicao * numero );

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "chao")
        {
            Destroy(this.gameObject);
        }
    }
   
}
