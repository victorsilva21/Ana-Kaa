using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Music_Controller : MonoBehaviour
{
      static AudioSource audiosource;
    GameObject[] controlador;
   static bool fade = false;
    
    
    // Start is called before the first frame update
    
    void Start()
    {
        controlador = GameObject.FindGameObjectsWithTag("music_control");
        if (controlador.Length > 1)
        {
            Destroy(controlador[1].gameObject);
        }
        audiosource = controlador[0].GetComponent<AudioSource>();

        audiosource.clip = Resources.Load<AudioClip>("Musicas/fase1 loop completo");
        if (audiosource.isPlaying != true)
        {
            
            audiosource.Play();
        }
           
        
            
        DontDestroyOnLoad(audiosource);
        
        
        
        
        
    }
   

    // Update is called once per frame
    void FixedUpdate()
    {
       if(fade == false)
        {
            try
            {

                audiosource.volume = Music_menu.volumeMaximo;
            }
            catch
            {
                audiosource.volume = 1;
            }
            
        }
       
        if (SceneManager.GetActiveScene().name != "Fase 1")
        {
            Destroy(this.gameObject);
        }

    }
    public static IEnumerator fade_out()
    {
        fade = true;
        do
        {
            audiosource.volume -= Time.deltaTime;
            yield return new WaitForSeconds(0.05f);

        } while (audiosource.volume != 0);

        audiosource.Stop();
        
        
        
        

    }
    public static void OnBossEnter()
    {
        audiosource.volume = 1;
        audiosource.clip = Resources.Load<AudioClip>("Musicas/Boss1(true) separado/boss1(true) parte 1");
        audiosource.Play();
    }
    public static IEnumerator OnBossmidFight()
    {
        yield return new WaitUntil(() => audiosource.time > audiosource.clip.length - 0.1f);
        
        audiosource.clip = Resources.Load<AudioClip>("Musicas/Boss1(true) separado/boss1(true) parte 2");
        audiosource.Play();
        //
    }
    public static IEnumerator OnBossclimax()
    {
        yield return new WaitUntil(() => audiosource.time > audiosource.clip.length - 0.1f);
        
        audiosource.clip = Resources.Load<AudioClip>("Musicas/Boss1(true) separado/boss1(true) climax");
        audiosource.Play();
        //
    }
    public static void OnBossEnd()
    {
        audiosource.Stop();
        
        audiosource.clip = Resources.Load<AudioClip>("Musicas/Boss1(true) separado/boss1(true) final no loop");
        audiosource.Play();
        audiosource.loop = false;
    }

    public static IEnumerator fade_in()
    {
        fade = true;
        
        do
        {
            
            audiosource.volume += Time.deltaTime;
            yield return new WaitForSeconds(0.05f);

        } while (audiosource.volume < Music_menu.volumeMaximo);
        fade = false;
        
    }
}
