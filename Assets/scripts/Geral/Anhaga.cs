using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anhaga : MonoBehaviour
{

    GameObject player;
    SpriteRenderer sprite;
    bool run = false;
    float t = 0;



    Color cor;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        RestartPos();
        sprite = player.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(transform.position.x > player.transform.position.x)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if (PlayerPrefs.HasKey("c3"))
        {
            if (PlayerPrefs.HasKey("c4Aux"))
            {
                sprite.color = Color.Lerp(Color.white, new Color32(215,215,255,125), Mathf.PingPong(Time.time, 1));
            }
            else
            {
                if (!run)
                {
                    run = true;
                    cor = sprite.color;
                }
                t += Time.deltaTime / 2;

                sprite.color = Color.Lerp(cor, Color.clear, t);
            }

        }
    }

    private void OnBecameInvisible()
    {
        RestartPos();
        //Debug.Log("Invisível");
    }

    void RestartPos()
    {
        if (!PlayerPrefs.HasKey("c1"))
        {
            transform.position = new Vector2(-283.4375f, -0.4376f);
        }
        else if (!PlayerPrefs.HasKey("c2"))
        {
            transform.position = new Vector2(-178.1875f, 47.6249f);
        }
        else if (!PlayerPrefs.HasKey("c3"))
        {
            transform.position = new Vector2(-26.4375f, 27.5625f);
        }
        else if (!PlayerPrefs.HasKey("c3.5"))
        {
            transform.position = new Vector2(-81.9375f, 81.6249f);
        }
        else if (!PlayerPrefs.HasKey("c4"))
        {
            transform.position = new Vector2(21.8125f, 30.6248f);
        }
        else
        {
            transform.position = new Vector2(-400, -100);
        }
    }
}
