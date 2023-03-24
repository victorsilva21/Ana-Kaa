using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class checkpoint_view : MonoBehaviour
{
    [SerializeField]SpriteRenderer spriteRenderer;
    [SerializeField] Sprite flor1, flor2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            spriteRenderer.sprite = flor2;
        }
    }
    
}
