using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public  class Music_menu : MonoBehaviour
{
    [SerializeField] static GameObject deathObj;
    static Vector2 posicaoFinal = new Vector2(20f, -19.05f), posicaoinicial;
    static Vector3 vel = Vector3.zero;
    public static AudioSource audiosource;
    GameObject[] controlador;
    public static float volumeMaximo = 1;
    Slider controle_de_volume;

    static bool trocadecena = false;
    // Start is called before the first frame update
    void Start()
    {

        
        if (SceneManager.GetActiveScene().name == "configs")
        {
            controle_de_volume = FindObjectOfType<Slider>();
        }
        controlador = GameObject.FindGameObjectsWithTag("music_control");
        if (controlador.Length > 1)
        {
            Destroy(controlador[1].gameObject);
        }
        trocadecena = false;
        posicaoFinal = new Vector2(20f, -19.05f);

        deathObj = GameObject.Find("transicao");
        posicaoinicial = deathObj.transform.position;
        audiosource = controlador[0].GetComponent<AudioSource>();

        Debug.Log(trocadecena);
        Debug.Log("1");

       

        DontDestroyOnLoad(this.gameObject);
       
        if(audiosource.isPlaying != true)
        {
            audiosource.clip = Resources.Load<AudioClip>("Musicas/menu separado/menu(inicio)");
            audiosource.Play();
        }

       
        Debug.Log("2");
        StartCoroutine(musicainicial());
        StartCoroutine(interrupdor());
        
    }
    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "configs")
        {
            controle_de_volume = FindObjectOfType<Slider>();
        }
        if (SceneManager.GetActiveScene().name != "Fase 1")
        {
            deathObj = GameObject.Find("transicao");
            StartCoroutine(transicao());
        }
       
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(controle_de_volume != null)
        {
            volumeMaximo = controle_de_volume.value;
        }
        audiosource.volume = volumeMaximo;
      if (trocadecena == true)
        {
            Destroy(this.gameObject);
        }
        if (SceneManager.GetActiveScene().name == "Fase 1")
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator interrupdor()
    {
        Debug.Log("4");
        yield return new WaitUntil(() => trocadecena == true);
        StopAllCoroutines();
        audiosource.Stop();
    }
     IEnumerator transicao()
    {
        // Colocar dentro das declarações
        // Burocracia de código

        Debug.Log("5");
        float t = Time.time + .73f;

        
        if(deathObj.transform.position.x > posicaoFinal.x)
        {
            while (Time.time < t)
            {

                deathObj.transform.position = Vector3.SmoothDamp(deathObj.transform.position, posicaoFinal, ref vel, .27f);
                yield return null;
            }

        }
        else if(deathObj.transform.position.x != posicaoinicial.x)
        {
            while (Time.time < t)
            {

                deathObj.transform.position = Vector3.SmoothDamp(deathObj.transform.position, posicaoinicial, ref vel, .27f);
                yield return null;
            }
        }
       
        
       

    }
     IEnumerator musicainicial()
    {
        Debug.Log("3");
        yield return new WaitUntil(() => audiosource.time > 3.5f);
            StartCoroutine(transicao());
            yield return new WaitUntil(() => audiosource.time > audiosource.clip.length - 0.1f);

            audiosource.clip = Resources.Load<AudioClip>("Musicas/menu separado/menu(meio)");
            audiosource.Play();
            yield return new WaitUntil(() => audiosource.time > audiosource.clip.length - 0.1f);
            audiosource.clip = Resources.Load<AudioClip>("Musicas/menu separado/menu(loop calmo)");
            audiosource.Play();
            audiosource.loop = true;


    }
    public static IEnumerator clique()
    {
        
        audiosource.clip = Resources.Load<AudioClip>("Musicas/menu separado/menu(fim no loop)");
        audiosource.loop = false;
        audiosource.Play();
        float t = Time.time + .73f;
        while (Time.time < t)
        {

            deathObj.transform.position = Vector3.SmoothDamp(deathObj.transform.position, posicaoinicial, ref vel, .27f);
            yield return null;
        }
        
        yield return new WaitUntil(() => audiosource.time > audiosource.clip.length - 0.1f);
        trocadecena = true;
        
        yield return null;
    }
}
