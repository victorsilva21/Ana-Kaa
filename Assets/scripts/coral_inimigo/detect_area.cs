using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detect_area : MonoBehaviour
{
    Animator Animin;
    // Start is called before the first frame update
    void Start()
    {
        Animin = gameObject.GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Animin.GetBool("primeiro_ataque") == false)
        {
            Animin.SetBool("primeiro_ataque", true);


        }
        else if(collision.gameObject.tag == "Player")
        {
            Animin.SetBool("ataque", true);

        }
    }
}
