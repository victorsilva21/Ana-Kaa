using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projetil_2 : MonoBehaviour
{
    GameObject cano;
    // Start is called before the first frame update
    void Start()
    {
        cano = GameObject.Find("inimigo");
        transform.rotation = cano.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (cano == null)
        {
            Destroy(this.gameObject);
        }
        else if(Vector2.Distance(transform.position, cano.transform.position) > 10)
        {
            Destroy(this.gameObject);
        }
        

        
        transform.Translate(new Vector2(7 * Time.deltaTime, 0));
        
        

    }
}
