using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coral_base : MonoBehaviour
{
    Animator Animin;
    [SerializeField]BoxCollider2D zona_de_dano;
    GameObject chara;
    bool morto = false;
    AudioSource coral_audio;
    // Start is called before the first frame update
    void Start()
    {
        coral_audio = GetComponent<AudioSource>();
        coral_audio.clip =  Resources.Load<AudioClip>("Efeitos sonoros/Mobs/Coral/Mato idle");
        coral_audio.loop = true;
        coral_audio.Play();
        chara = GameObject.FindGameObjectWithTag("Player");
        Animin = gameObject.GetComponent<Animator>();
        
        zona_de_dano.enabled = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(chara.transform.position.x > transform.position.x && !morto)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(!morto)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        //Animin.SetBool("primeiro_ataque", true);
      


    }
    void ataque()
    {
        if (Animin.GetBool("primeiro_ataque") == false)
        {
            coral_audio.clip = Resources.Load<AudioClip>("Efeitos sonoros/Mobs/Coral/Saindo do mato + atack simples");
            coral_audio.loop = false;
            coral_audio.Play();
        }
        if (Animin.GetBool("primeiro_ataque") == true)
        {
            coral_audio.clip = Resources.Load<AudioClip>("Efeitos sonoros/Mobs/Coral/atack simples");
            coral_audio.loop = false;
            coral_audio.Play();
        }
        zona_de_dano.enabled = !zona_de_dano.enabled;
        
    }
    void fimdeataque()
    {
        coral_audio.clip = Resources.Load<AudioClip>("Efeitos sonoros/Mobs/Coral/idle");
        coral_audio.loop = true;
        coral_audio.Play();
        Animin.SetBool("ataque", false);
    }
    void dead()
    {
        coral_audio.clip = Resources.Load<AudioClip>("Efeitos sonoros/Mobs/Coral/Morrendo");
        coral_audio.loop = false;
        coral_audio.Play();
        morto = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ataque")
        {
            zona_de_dano.enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            Animin.SetBool("morte", true);
            
            
        }
    }
    private void OnBecameInvisible()
    {
        if(morto == true)
        {
            Destroy(this.gameObject);
        }
    }

}
