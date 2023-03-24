using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorcoEspinho : MonoBehaviour
{
    //  //  // DECLARAÇÕES //  //  //
    #region
    // INSPECTOR
    [Header("Basico")]
    [SerializeField, Tooltip("Vida total do mob")] int vida = 1;
    [SerializeField, Tooltip("Velocidade do mob")] float velocidade = 1.8f;
    [SerializeField, Tooltip("0 - Parado; 1 - Andando; 2 - Correndo"), Range(0, 2)] int estado = 0;
    [SerializeField, Tooltip("Possuído ou não?")] bool possuido = false;
    [SerializeField, Tooltip("Alcance da detecção")] float deteccao = 5f;
    [SerializeField, Tooltip("Layer do chão.")] LayerMask mask;
    [SerializeField, Tooltip("Layer dos espinhos.")] LayerMask mask2;


    // OBJETOS EXTERNOS
    AudioSource audioSource;
    Animator anim;
    Transform groundCheck;
    Rigidbody2D rb;
    Transform player;

    // VARIÁVEIS AUXILIARES
    int life;
    bool death = false;
    [SerializeField] bool detected = false;
    int facing = -1;
    #endregion



    //  //  // FUNÇÕES PRINCIPAIS //  //  //
    #region
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.GetChild(0);
        life = vida;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        // Código do possuído
        if (possuido)
        {

            anim.SetLayerWeight(1, 1);
            if(player.position.x > transform.position.x && facing < 0 || player.position.x < transform.position.x && facing > 0)
            {
                //Debug.Log(player.position.x - transform.position.x);
                if (detected)
                {

                    if (Mathf.Abs(player.transform.position.x - transform.position.x) < .5f)
                    {
                        estado = 0;
                    }
                    else
                    {
                        estado = 2;
                    }

                    if (Vector2.Distance(player.transform.position, transform.position) > 2 * deteccao)
                    {
                        detected = false;
                        estado = 1;
                    }
                }
                else
                {
                    if(Vector2.Distance(player.transform.position, transform.position) < deteccao)
                    {
                        detected = true;
                    }
                }
            }
            else if(detected)
            {
                Flip();
            }
        }
        else
        {
            anim.SetLayerWeight(1, 0);
        }

        // Check de vida
        if(life <= 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            rb.isKinematic = true;
            gameObject.tag = "Untagged";
            gameObject.layer = 8;
            death = true;
            audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Mobs/Tatu/Porco espinho morrendo");
            audioSource.loop = false;
            audioSource.Play();
            anim.SetBool("Death", true);
        }
        else
        {
            anim.SetBool("Death", false);
            death = false;
            gameObject.layer = 4;
            gameObject.tag = "dano";
            rb.isKinematic = false;
        }


        // Flip
        if(!Physics2D.OverlapCircle(groundCheck.position, .2f, mask) || Physics2D.OverlapCircle(new Vector2(groundCheck.position.x, transform.position.y), .2f, mask))
        {
            if(!detected)
            {
                Flip();
            }
            else
            {
                estado = 0;
            }
        }
        else if (detected)
        {
            if((player.position.x - transform.position.x < -.5f) || (player.position.x - transform.position.x > .5f))
            estado = 2;
        }

        if(Physics2D.OverlapCircle(new Vector2(groundCheck.position.x, transform.position.y), .2f, mask2))
        {
            if (!detected)
            {
                Flip();
            }
            else
            {
                estado = 0;
            }
        }


        // Movimentação
        switch (estado)
        {
            case 0:
                anim.SetInteger("State", 0);
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;
            case 1:
                anim.SetInteger("State", 1);
                if(!death)
                    rb.velocity = new Vector2(velocidade, rb.velocity.y);
                break;
            case 2:
                anim.SetInteger("State", 2);
                audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Mobs/Tatu/Corrida tatu corrompido");
                audioSource.loop = true;
                audioSource.Play();
                if (!death)
                    rb.velocity = new Vector2(velocidade * 2, rb.velocity.y);
                break;
        }
    }
    #endregion



    //  //  // FUNÇÕES AUXILIARES //  //  //
    #region
    void Flip()
    {
        if (!death)
        {
            //  Muda o lado do objeto usando escala.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            facing *= -1;
            transform.localScale = theScale;

            //  Ajusta a velocidade
            velocidade = -velocidade;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ataque")
        {
            life--;
        }
    }


    private void OnBecameInvisible()
    {
        if (death && possuido)
        {
            Destroy(gameObject);
        }
    }
    #endregion
}