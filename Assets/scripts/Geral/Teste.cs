using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Teste : MonoBehaviour
{
    [SerializeField] float targetX;
    [SerializeField] float strenght;
    float gravity,angle;
    Vector2 pos;

    Rigidbody2D rb;
    [SerializeField] Transform target;

    private void Start()
    {
        pos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        gravity = GetComponent<Rigidbody2D>().gravityScale;
        targetX = target.position.x;
    }

    public void Shoot()
    {


        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        rb.AddForce(dir * strenght);
    }

    public void Restart()
    {
        transform.position = pos;
    }
}
