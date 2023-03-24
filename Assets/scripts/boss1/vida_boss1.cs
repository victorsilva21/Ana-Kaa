using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class vida_boss1 : MonoBehaviour
{
    [Header("atributos do boss")]
    [SerializeField, Tooltip("vida do boss"), Range(5, 20)] float vida = 12;
    [SerializeField, Tooltip("vida do boss"), Range(1, 5)] float cd = 2;
    float tempo,vidainicial;
    Animator animator;
    Image hudboss;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        vidainicial = vida;
        hudboss = GameObject.FindGameObjectWithTag("bossHud").GetComponent<Image>();
        tempo = cd;
    }

    // Update is called once per frame
    void Update()
    {
        hudboss.fillAmount = vida / vidainicial;
        if(tempo > 0)
        {
            tempo -= Time.deltaTime;
        }
        if(hudboss.fillAmount < 0.5f)
        {
            StartCoroutine( Music_Controller.OnBossclimax());
        }
        else if(hudboss.fillAmount < 0.8f)
        {
           StartCoroutine(Music_Controller.OnBossmidFight());
        }

        if(vida <= 0)
        {
            Music_Controller.OnBossEnd();
            PlayerPrefs.SetString("bossDead", "bossDead");
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ataque"))
        {
            vida--;
            animator.SetTrigger("dano_levado");
            tempo = cd;
        }
    }
}
