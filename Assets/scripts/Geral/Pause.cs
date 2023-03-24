using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu, pauseButton;

    Transform deathObj;
    static Vector3 vel = Vector3.zero;

    bool settings = false;

    void Start()
    {
        PlayerPrefs.SetInt("pause", 0);
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);

        deathObj = GameObject.FindGameObjectWithTag("deathHud").transform;
    }

    
    public void PauseGame()
    {
        PlayerPrefs.SetInt("pause", 1);
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        pauseMenu.transform.GetChild(2).gameObject.SetActive(true);
        pauseMenu.transform.GetChild(3).gameObject.SetActive(true);
        pauseMenu.transform.GetChild(4).gameObject.SetActive(false);
        settings = false;
        pauseButton.SetActive(false);
    }

    public void PlayGame()
    {
        PlayerPrefs.SetInt("pause", 0);
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void Menu()
    {
        StartCoroutine(transicao());
    }

    public void SettingsMenu()
    {
        pauseMenu.transform.GetChild(2).gameObject.SetActive(settings);
        pauseMenu.transform.GetChild(3).gameObject.SetActive(settings);
        pauseMenu.transform.GetChild(4).gameObject.SetActive(!settings);
        settings = !settings;
    }


    IEnumerator transicao()
    {
        Time.timeScale = .8f;

        float t = Time.time + .73f;

        deathObj.position = deathObj.position + Vector3.right * 150;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        while (Time.time < t)
        {
            deathObj.position = Vector3.SmoothDamp(deathObj.position, player.transform.position, ref vel, .27f);
            yield return null;
        }

        Time.timeScale = 1;
        PlayerPrefs.SetInt("pause", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}
