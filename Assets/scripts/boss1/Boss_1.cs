using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1 : MonoBehaviour
{
    GameObject ataque_1;     
    Rigidbody2D rb;
    BoxCollider2D colisor;
    CircleCollider2D ataque3;
    Animator animator;
    Vector3 inicial;
    AudioSource audioSource;
    bool voltar = false;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        ataque3 = gameObject.GetComponent<CircleCollider2D>();
        ataque3.enabled = false;
        colisor = gameObject.GetComponent<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        ataque_1 = Resources.Load<GameObject>("prefabio/tiro_veneno_boss");
        
        inicial = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
            
    }
    public void onfinishstart()
    {
        StartCoroutine(ataquesSelect());
        
    }
    public void oncuspestart()
    {
        animator.SetBool("cuspe", false);
    }
    public void ongiro()
    {

        audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Carbunculo/Ataque 2/Giro");
        audioSource.loop = true;
        audioSource.Play();
    }
    public void onbatida()
    {
        audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Carbunculo/Ataque 2/ataque");
        audioSource.loop = false;
        audioSource.Play();
    }
    public void onespinho()
    {
        ataque3.enabled = true;
    }
    public void offespinho()
    {
        ataque3.enabled = false;
    }
    public void liberavolta()
    {
        voltar = true;
    }
    IEnumerator ataquesSelect()
    {
        
        
        int seletor = Random.Range(1, 4);
        switch (seletor)
        {
            case 1:
                audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Carbunculo/Melhor preparação");
                audioSource.Play();
                animator.SetBool("cuspe", true);
                yield return new WaitUntil(() => animator.GetBool("cuspe") == false);
                audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Carbunculo/Rodrigo cuspindo");
                audioSource.Play();
                Instantiate(ataque_1, transform.position, Quaternion.identity);
                animator.SetBool("cuspe", false);
                yield return new WaitForSeconds(13);


                break;
            case 2:
                yield return new WaitForSeconds(2f);
                animator.SetBool("ataque2", true);
                audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Carbunculo/Ataque 2/Pulo");
                audioSource.loop = false;
                audioSource.Play();
                Vector3 posicao = GameObject.Find("Personagem").transform.position;
                rb.AddForce(new Vector2(0, 12.5f),ForceMode2D.Impulse);
                colisor.isTrigger = true;
                do
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(posicao.x, transform.position.y, transform.position.z), 0.25f);
                    yield return new WaitForSeconds(0.01f);
                } while (Vector3.Distance(transform.position, new Vector3(posicao.x, transform.position.y, transform.position.z)) > 0.5f);
                yield return new WaitUntil(() => Vector3.Distance(transform.position, new Vector3(posicao.x,inicial.y,transform.position.z)) < 0.7f );
                colisor.isTrigger = false;
                animator.SetBool("ataque2_partefinal", true);
                
                animator.SetBool("ataque2", false);
                yield return new WaitForSeconds(2);
                transform.localScale = new Vector3(-1, 1, 1);
                do
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(inicial.x, transform.position.y, transform.position.z), 0.7f);
                    yield return new WaitForSeconds(0.01f);
                } while (Vector3.Distance(transform.position, inicial) > 1f );
                Debug.Log("teste sonic");
                transform.localScale = new Vector3(1, 1, 1);
                animator.SetBool("ataque2_partefinal", false);

                yield return new WaitForSeconds(4);


                break;
            case 3:
                
                rb.gravityScale = 0;
                colisor.isTrigger = true;
                animator.SetBool("ataque3_start", true);
                do
                {
                    transform.Translate(0, 25 * Time.deltaTime, 0);
                    yield return new WaitForSeconds(0.01f);
                } while (transform.position.y < 6.5f);
                transform.position = new Vector3(87.69f, transform.position.y, transform.position.z);
                
                
                rb.gravityScale = 3;
                yield return new WaitUntil(() => Vector3.Distance(transform.position, new Vector3(transform.position.x, inicial.y, transform.position.z)) < 0.5f);
                colisor.isTrigger = false;
                rb.gravityScale = 1;
                animator.SetBool("ataque3_mid", true);
                animator.SetBool("ataque3_start", false);
                yield return new WaitUntil(() => ataque3.enabled == true);
                yield return new WaitUntil(() => voltar == true);
                transform.localScale = new Vector3(-1, 1, 1);
                voltar = false;

                do{
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(inicial.x, transform.position.y, transform.position.z), 0.7f);
                    yield return new WaitForSeconds(0.01f);
                } while (Vector3.Distance(transform.position, inicial) >1) ;
                Debug.Log("teste sonic");
                transform.localScale = new Vector3(1, 1, 1);
                animator.SetBool("ataque3_mid", false);
                yield return new WaitForSeconds(4);


                break;
        }
        StartCoroutine(ataquesSelect());
    }
}
