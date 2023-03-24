using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    GameObject personagem;
    // Start is called before the first frame update
    void Start()
    {
        personagem = GameObject.Find("personagem teste");

    }

    // Update is called once per frame
    void Update()
    {
       transform.position = Vector3.MoveTowards(transform.position, new Vector3(personagem.transform.position.x, personagem.transform.position.y, -10), 100);
        //transform.position = new Vector3(personagem.transform.position.x, personagem.transform.position.y, -10);
    }
}
