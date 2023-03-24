using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
namespace Analog{
    public class joystick : MonoBehaviour
    {

        bool cameraCoold = false;
        bool cc = false;
        public int t_camera = 0;
        public bool t_enable = true;
        Vector2 posicaoinicial;
        bool clique = false;
        Collider2D col;
       [SerializeField] static public float horizontal, vertical;
        RectTransform movimento;
         public GameObject bolaext;
        Cutscenemanagement cutscene;

        bool cut = false;
     
        
        // Start is called before the first frame update
        void Start()
        {
            movimento = GetComponent<RectTransform>();
            col = bolaext.GetComponent<Collider2D>();
            cutscene = GameObject.FindGameObjectWithTag("Player").GetComponent<Cutscenemanagement>();
        }


        // Update is called once per frame
        void Update()
        {
            posicaoinicial = bolaext.transform.position ;
            horizontal = ((movimento.transform.position.x - posicaoinicial.x) / (col.bounds.size.x / 2)) * 5;
            vertical = ((movimento.transform.position.y - posicaoinicial.y) / (col.bounds.size.x / 2)) * 5;

            if (Input.touchCount ==1 && !cut)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchpos = Camera.main.ScreenToWorldPoint(touch.position);
                float distancia = 0;

                switch (touch.phase)
                {
                    case TouchPhase.Began:

                        distancia = Vector2.Distance(touchpos, movimento.transform.position);
                        if (distancia < col.bounds.size.x / 2)
                        {
                            clique = true;
                            Vector2 offset = Vector2.ClampMagnitude((touchpos - posicaoinicial), col.bounds.size.x/2);
                            movimento.transform.position = new Vector2(posicaoinicial.x + offset.x, posicaoinicial.y + offset.y);
                        }
                        
                        break;
                    case TouchPhase.Moved:

                        if (clique == true)
                        {
                            
                            Vector2 offset = Vector2.ClampMagnitude((touchpos - posicaoinicial), col.bounds.size.x/2);
                            movimento.transform.position = new Vector2(posicaoinicial.x + offset.x, posicaoinicial.y + offset.y);
                        }

                        break;
                    case TouchPhase.Ended:
                        
                        movimento.transform.position = posicaoinicial;
                        clique = false;
                        break;
                }
            }
            else if (Input.touchCount > 1 && !cut)
            {
                Touch touch;
                float distancia1 = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), movimento.transform.position);
                float distancia2 = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position), movimento.transform.position);
                if(distancia1 > distancia2)
                {
                    touch = Input.GetTouch(1);
                }
                else
                {
                    touch = Input.GetTouch(0);
                }
                
                Vector2 touchpos = Camera.main.ScreenToWorldPoint(touch.position);
                float distancia = 0;

                switch (touch.phase)
                {
                    case TouchPhase.Began:

                        distancia = Vector2.Distance(touchpos, movimento.transform.position);
                        if (distancia < col.bounds.size.x / 2)
                        {
                            clique = true;
                            Vector2 offset = Vector2.ClampMagnitude((touchpos - posicaoinicial), col.bounds.size.x / 2);
                            movimento.transform.position = new Vector2(posicaoinicial.x + offset.x, posicaoinicial.y + offset.y);
                        }
                        break;
                    case TouchPhase.Moved:

                        if (clique == true)
                        {

                            Vector2 offset = Vector2.ClampMagnitude((touchpos - posicaoinicial), col.bounds.size.x / 2);
                            movimento.transform.position = new Vector2(posicaoinicial.x + offset.x, posicaoinicial.y + offset.y);
                        }

                        break;
                    case TouchPhase.Ended:

                        movimento.transform.position = posicaoinicial;
                        clique = false;
                        break;
                }
            }

            if (clique && vertical > 2.75f || clique && vertical < -2.75f)
            {
                if (cameraCoold)
                {
                    if(vertical > 4)
                    {
                        if(t_camera != 2)
                        {
                            //cutscene.GetCutscene("LookUpJoy", 2);
                            t_camera = 2;
                        }
                    }
                    else
                    {
                        if(t_camera != 1)
                        {
                            //cutscene.GetCutscene("LookDownJoy", 2);
                            t_camera = 1;
                        }
                    }
                }
                else
                {
                    if (!cc)
                    {
                        StartCoroutine(CameraCooldown());
                    }
                }
            }
            else
            {
                cc = false;
                cameraCoold = false;
                if(t_camera != 0)
                {
                    //cutscene.GetCutscene("LookDefaultJoy", 2);
                    t_camera = 0;
                }
            }
        }

        public void Stop(bool stop)
        {
            if (stop)
            {
                Debug.Log("1");
                movimento.transform.position = posicaoinicial;
                clique = false;
                posicaoinicial = bolaext.transform.position;
                horizontal = ((movimento.transform.position.x - posicaoinicial.x) / (col.bounds.size.x / 2)) * 5;
                vertical = ((movimento.transform.position.y - posicaoinicial.y) / (col.bounds.size.x / 2)) * 5;
                cut = true;
                bolaext.GetComponent<Image>().enabled = false;
                GetComponent<Image>().enabled = false;
                Debug.Log("2");
            }
            else
            {
                Debug.Log("3");
                cut = false;
                bolaext.GetComponent<Image>().enabled = true;
                GetComponent<Image>().enabled = true;
                Debug.Log("4");
            }
        }

        IEnumerator CameraCooldown()
        {
            cc = true;
            yield return new WaitForSeconds(1.3f);
            cameraCoold = true;

        }
    }
}