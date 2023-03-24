using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu_controller : MonoBehaviour
{
    [SerializeField] GameObject play, menu, newGame, Continue, Exit;
    [SerializeField] Image Painel;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("end"))
        {
            PlayerPrefs.DeleteAll();
        }

        Time.timeScale = 1;
        PlayerPrefs.SetInt("pause", 0);
        if (!PlayerPrefs.HasKey("fase"))
        {
            Continue.GetComponent<Button>().interactable = false;
        }
        if(SceneManager.GetActiveScene().name == "menu_scene")
        {
            newGame.SetActive(false);
            Continue.SetActive(false);
            Exit.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1;
    }
    public void OnclickPlay()
    {
        Painel.sprite = Resources.Load<Sprite>("sprites/Menu/TELA MENU");
        newGame.SetActive(true);
        Continue.SetActive(true);
        Exit.SetActive(true);
        play.SetActive(false);
    }
    public void OnClickNewGame()
    {
        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("pause", 0);

        /*
        PlayerPrefs.SetFloat("x", -8.5f);
        PlayerPrefs.SetFloat("y", 32.4f);
        PlayerPrefs.SetString("c1", "c1");
        PlayerPrefs.SetString("c2", "c2");
        PlayerPrefs.SetString("c3", "c3");
        */
        StartCoroutine(Music_menu.clique());
        StartCoroutine(newgame());

    }
    IEnumerator newgame()
    {
        yield return new WaitUntil(() => Music_menu.audiosource.time > Music_menu.audiosource.clip.length - 0.1f);
        SceneManager.LoadSceneAsync("Fase 1");
    }
    public void OnClickContinue()
    {
        if (PlayerPrefs.HasKey("fase"))
        {
            StartCoroutine(Music_menu.clique());
            StartCoroutine(continuar());
            
        }
        
        
    }
    IEnumerator continuar()
        {
        yield return new WaitUntil(() => Music_menu.audiosource.time > Music_menu.audiosource.clip.length - 0.1f);
        
        SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("fase"));
    }
    public void onconfigclick()
    {
        SceneManager.LoadSceneAsync("configs");
    }
    public void OnClickExit()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
    public void OnClickExitConfig()
    {
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync("menu_scene");
    }
}
