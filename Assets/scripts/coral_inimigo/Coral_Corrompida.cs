using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coral_Corrompida : MonoBehaviour
{
    //  //  // DECLARAÇÕES //  //  //
    #region
    // INSPECTOR
    [Header("Basico")]
    [SerializeField, Tooltip("Vida total do mob")] int vida = 1;
    [SerializeField, Tooltip("Velocidade do mob")] float velocidade = 2.3f;
    [SerializeField, Tooltip("Alcance da detecção")] float deteccao = 13f;
    [SerializeField, Tooltip("Tempo entre os movimentos")] float tempoParado = 2f;
    [SerializeField, Tooltip("Tempo pré ataque")] float tempoAtaque = 1.3f;
    [SerializeField, Tooltip("Tempo se movendo")] float tempoMovendo = 5f;
    [SerializeField, Tooltip("Layer do chão.")] LayerMask mask;
    [SerializeField, Tooltip("Layer dos espinhos.")] LayerMask mask2;



    // OBJETOS EXTERNOS
    Animator anim;
    Transform groundCheck;
    Rigidbody2D rb;
    Transform player;
    AudioSource audioSource;

    // VARIÁVEIS AUXILIARES
    int state = 0;
    bool moving = true;
    bool attack = false;
    int life;
    bool death = false;
    bool detected = false;
    int facing = -1;
    bool bote = false;
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

        StartCoroutine(Delay());
    }


    void Update()
    {
        // Debug.Log(gameObject.name);
        // Debug.Log(Mathf.Abs(player.position.x - transform.position.x));
        // Debug.Log(Mathf.Abs(player.position.y - transform.position.y));
        // Código do possuído

        if (Vector2.Distance(player.transform.position, transform.position) < deteccao)
        {
            // Debug.Log("Detectou");
            detected = true;
        }


        if (player.position.x > transform.position.x && facing < 0 || player.position.x < transform.position.x && facing > 0)
        {
            // Debug.Log("1");
        }
        else if (detected && moving && !attack)
        {
            // Debug.Log("2");
            Flip();
        }
        

        if (detected && !attack)
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) < .5f)
            {
                // Debug.Log("3");
                state = 0;
            }
            else
            {
                if (moving)
                {
                    // Debug.Log("4");
                    state = 1;
                }
                else
                {
                    // Debug.Log("5");
                    state = 0;
                }
            }

            if (Vector2.Distance(player.transform.position, transform.position) > 2 * deteccao)
            {
                // Debug.Log("Nao detectou mais");
                detected = false;
                state = 1;
            }
        }
        else
        {
            if (!attack)
            {
                if (moving)
                {
                    // Debug.Log("6");
                    state = 1;
                }
                else
                {
                    // Debug.Log("7");
                    state = 0;
                }
            }
        }

        if(Mathf.Abs(player.position.y - transform.position.y) < 1 && Mathf.Abs(player.position.x - transform.position.x) < 4 && !attack)
        {
            // Debug.Log("8");
            StartCoroutine(Attack());
        }

        // Check de vida
        if (life <= 0)
        {
            // Debug.Log("9");
            rb.velocity = new Vector2(0, rb.velocity.y);
            rb.isKinematic = true;
            gameObject.tag = "Untagged";
            gameObject.layer = 8;
            death = true;
            anim.SetBool("Death", true);
        }
        else
        {
            // Debug.Log("10");
            anim.SetBool("Death", false);
            death = false;
            gameObject.layer = 4;
            gameObject.tag = "dano";
            rb.isKinematic = false;
        }


        // Flip
        if (!Physics2D.OverlapCircle(groundCheck.position, .2f, mask) || Physics2D.OverlapCircle(new Vector2(groundCheck.position.x, transform.position.y), .2f, mask))
        {
            if (!detected && !attack)
            {
                // Debug.Log("11");
                Flip();
            }
            else
            {
                if (!attack)
                {
                    // Debug.Log("12");
                    state = 0;
                }
            }
        }

        if (Physics2D.OverlapCircle(new Vector2(groundCheck.position.x, transform.position.y), .2f, mask2))
        {
            if (!detected && !attack)
            {
                 Debug.Log("11");
                Flip();
            }
            else
            {
                if (!attack)
                {
                     Debug.Log("12");
                    state = 0;
                }
            }
        }


        // Movimentação
        switch (state)
        {
            case 0:
                anim.SetInteger("State", 0);
                audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Mobs/Coral/Idle corrompida");
                audioSource.loop = true;
                audioSource.Play();
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;
            case 1:
                anim.SetInteger("State", 1);
                audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Mobs/Coral/Idle corrompida");
                audioSource.loop = true;
                audioSource.Play();
                if (!death)
                    rb.velocity = new Vector2(velocidade, rb.velocity.y);
                break;
            case 2:
                anim.SetInteger("State", 2);
                
                if (!death && bote)
                    rb.velocity = new Vector2(velocidade * 5, rb.velocity.y);
                break;
        }

        //Debug.Log("!!!!!!!!!!!!!!ACABOU!!!!!!!!!!!");
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
        if (collision.gameObject.tag == "ataque")
        {
            life--;
        }

        if(collision.gameObject.tag == "dano")
        {
            life--;
        }
    }


    private void OnBecameInvisible()
    {
        if (death)
        {
            Destroy(gameObject);
        }
    }


    public void AttackEnd()
    {
        state = 0;
        moving = true;
        attack = false;
        bote = false;
    }


    IEnumerator Delay()
    {
        yield return new WaitForSeconds(Random.Range(1, 6));
        while (true)
        {
            moving = true;
            yield return new WaitForSeconds(tempoMovendo);
            yield return new WaitUntil(()=> !attack);
            moving = false;
            yield return new WaitForSeconds(tempoParado);
            yield return new WaitUntil(() => !attack);
        }
    }

    IEnumerator Attack()
    {
        attack = true;
        moving = false;
        state = 0;
        yield return new WaitForSeconds(tempoAtaque);
        state = 2;
        yield return new WaitForSeconds(.2f);
        bote = true;
        audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Mobs/Coral/atack corrompida");
        audioSource.loop = false;
        audioSource.Play();
    }
    #endregion
}
