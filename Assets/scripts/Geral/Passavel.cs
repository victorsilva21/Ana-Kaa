using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passavel : MonoBehaviour
{
    Transform target;
    Collider2D col;
    bool passable;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<movimento>().groundCheck;
        passable = GameObject.FindGameObjectWithTag("Player").GetComponent<movimento>().trig;
        col = GetComponent<Collider2D>();
        gameObject.tag = "chao";
    }

    void Update()
    {
        passable = GameObject.FindGameObjectWithTag("Player").GetComponent<movimento>().trig;

        if(target.position.y < transform.position.y + 0 || passable)
        {
            gameObject.layer = 2;
            /*
            if(col.usedByComposite)
            {
                col.usedByComposite = false;
            } //*/
            col.enabled = false;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("chao");
            /*
            if (!col.usedByComposite)
            {
                col.usedByComposite = true;
            } //*/
            col.enabled = true;
        }
    }
}
