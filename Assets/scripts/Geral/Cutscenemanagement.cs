using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using Analog;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Cutscenemanagement : MonoBehaviour
{
    //  //  // DECLARA��ES //  //  //
    CinemachineVirtualCamera VCam;
    CinemachineFramingTransposer transp;
    GameObject d_box;                 // dialogue box
    TMP_Text d_text;               // dialogue text
    GameObject[] hud;
    joystick joy;                   //
    Vector2 t_default;              // Transpose default
    Vector2 t_current;              // Transpose current
    public bool locked = false;     // Camera locked
    public bool running = false;    // cutscene
    [SerializeField] float time = .1f;  // Dialogue time
    bool touched = false;       // Skip?
    bool run = false;       // dialogue
    int joy_stage = 0;
    [SerializeField] GameObject end;

    Transform deathObj;
    static Vector3 vel = Vector3.zero;


    void Start()
    {
        deathObj = GameObject.FindGameObjectWithTag("deathHud").transform;
        VCam = GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>();
        transp = VCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        t_default = new Vector2(transp.m_ScreenX, transp.m_ScreenY);
        t_current = new Vector2(t_default.x, t_default.y);
        d_box = GameObject.FindGameObjectWithTag("Dialogue");
        d_text = d_box.transform.GetChild(0).GetComponent<TMP_Text>();
        d_box.SetActive(false);
        hud = GameObject.FindGameObjectsWithTag("Hud controls");
        end.SetActive(false);

        for (int i = 0; i < hud.Length; i++)
        {
            if (hud[i].name == "joystick")
            {
                joy = hud[i].transform.GetChild(0).GetComponent<joystick>();
            }
        }
    }


    void Update()
    {
        transp.m_ScreenX = t_current.x;
        transp.m_ScreenY = t_current.y;
        if(Input.touchCount > 0 && Input.GetTouch(0).position.y < Screen.height / 2 &&  PlayerPrefs.GetInt("pause") == 0)
        {
            Touch t = Input.GetTouch(0);

            switch (t.phase)
            {
                case TouchPhase.Began:
                    touched = true;
                    break;
                case TouchPhase.Ended:
                    //touched = false;
                    break;
            }
        }

        
        if(joy.t_camera != joy_stage)
        {
            if (!locked)
            {
                Look(joy.t_camera);
            }
            joy_stage = joy.t_camera;
        }        
    }


    public void GetCutscene(string s, int l = 2)
    {
        if (l == 1)
        {
            locked = true;
        }
        else if(l == 0)
        {
            locked = false;
        }


        switch (s)
        {
            case "LookDown":
                Look(1);
                break;

            case "LookDownL":
                locked = true;
                Look(1);
                break;

            case "LookDownJoy":
                if (!locked)
                    Look(1);
                break;

            case "LookUp":
                Look(2);
                break;

            case "LookUpL":
                locked = true;
                Look(2);
                break;

            case "LookUpJoy":
                if(!locked)
                    Look(2);
                break;

            case "LookDefault":
                locked = false;
                Look();
                break;

            case "LookDefaultJoy":
                if (!locked)
                    Look();
                break;

            case "c1":
                StartCoroutine(Scene(1));
                break;

            case "c2":
                StartCoroutine(Scene(2));
                break;

            case "c3":
                StartCoroutine(Scene(3));
                break;

            case "c3.5":
                StartCoroutine(Scene(35));
                break;

            case "c4":
                StartCoroutine(Scene(4));
                break;

            case "c5":
                StartCoroutine(Scene(5));
                break;
        }
    }


   


    void Look(int i = 0, GameObject target = null)
    {
        switch (i)
        {
            case 0:
                t_current.y = t_default.y;
                break;
            case 1:
                t_current.y = .1f;
                break;
            case 2:
                t_current.y = .9f;
                break;
        }

        if(target != null)
        {
            VCam.Follow = target.transform;
        }
        else
        {
            VCam.Follow = gameObject.transform;
        }
    }


    void HudDisable(bool set)
    {
        for (int i = 0; i < hud.Length; i++)
        {
            if (set == false)
            {
                Debug.Log("0");
                joy.Stop(true);
            }
            else
            {
                Debug.Log("2.5");
                joy.Stop(false);
            }

            if (hud[i].name != "joystick")
            hud[i].SetActive(set);
        }
    }


    IEnumerator Scene(int scene)
    {
        running = true;
        touched = false;
        locked = true;
        d_box.SetActive(true);
        HudDisable(false);
        switch (scene)
        {
            case 1:
                if (!PlayerPrefs.HasKey("c1"))
                {
                    Look(0,GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("Ah, Tainara! Finalmente chegou. Pronta para come�ar? Quero ver o quanto voc� evoluiu esses dias."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Olha, eu j� pedi desculpas por aquele acidente com o jacar�. N�o precisava ter trocado o lugar de treino s� por isso. Eu consigo dar conta."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("Voc� tem que prestar mais aten��o da pr�xima vez para n�o confundir eles com um tronco de novo. Deixe desse medo bobo."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Mas voc� fala isso por que n�o viu os dentes dele! Parece um lagarto gigante. Urgh!!"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    StartCoroutine(RunDialogue("E ainda acho que eu consigo dessa vez."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("Relaxe crian�a. Voc� precisa praticar em outro ambiente tamb�m. E tenho meus motivos para estar aqui."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Que motivos? � melhor que n�o tenham a ver com jacar�s e troncos."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("Se voc� conseguir chegar no final podemos conversar."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Ha! N�o se preocupe. Eu te espero l�."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    PlayerPrefs.SetString("c1", "c1");
                }
                break;
            case 2:
                if(!PlayerPrefs.HasKey("c2"))
                { 
                    Look();
                    StartCoroutine(RunDialogue("J� est� aqui? Voc� ainda tem que me explicar como voc� chega assim t�o r�pido."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(1, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("Bem... Digamos que eu n�o tenho anima��o para andar."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Mas o que raios isso quer dizer?"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(1, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("N�o importa. Eu fiz algo para voc�. Descendo aqui embaixo ter�o umas flores. Elas v�o salvar seu progresso e voc� vai voltar nelas se cair."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(1);
                    StartCoroutine(RunDialogue("(Se mova para baixo e pule para descer de plataformas.)"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    PlayerPrefs.SetString("c2", "c2");
                }
                break;

            case 3:
                if (!PlayerPrefs.HasKey("c3"))
                {
                    Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("O caminho foi bloqueado por estes espinhos. N�o vai ter como passar por aqui."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Estes espinhos n�o estava sempre a�?"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("N�o. J� usei este caminho antes. Algo fez estas vinhas crescerem mais do que deveriam."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Eu... Devia me preocupar? Seu tom de voz me diz que voc� sabe de algo."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("Voc� tem que se concentrar em terminar o seu treino. Use essas paredes e pule de uma para outra para subir como eu te ensinei. Te encontro l� em cima"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Mas..."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    StartCoroutine(RunDialogue("Certo. Tudo bem."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(2);
                    StartCoroutine(RunDialogue("(Pule na parede clicando em pulo enquanto no ar encostando em uma parede)"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    PlayerPrefs.SetString("c3", "c3");
                }
                break;

            case 35:
                if (!PlayerPrefs.HasKey("c3.5"))
                {
                    Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("Demorou mais do que eu imaginava. Ainda est� viva?"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Haha. N�o foi t�o dif�cil. Eu s� estava... Er... Me aquecendo."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("Bem, j� que est� t�o pronta, vamos para um desafio de verdade com obst�culos vivos. O que acha?"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Obst�culos vivos? Voc� quer dizer que eu vou ter que lutar contra animais?"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("Sim. O caminho a frente est� cheio deles. Mas lembre-se que eles s�o vidas como voc�. Segure os seus golpes e apenas nocauteie eles."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Pode deixar! Eu odiaria ferir um animal de verdade. "));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("(Use o bot�o de ataque para nocautear inimigos.)"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    PlayerPrefs.SetString("c3.5", "c3.5");
                }
                break;

            case 4:
                if (!PlayerPrefs.HasKey("c4"))
                {
                    //Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("Tainara!! Venha r�pido! Eu n�o tenho muito tempo!"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Anhag�?! O que houve?!"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("Tem alguma coisa de errada com a floresta. Eu estou sentindo minha liga��o com este plano se quebrando."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Mas o qu�? Por qu�? O que foi que aconteceu?! Voc� n�o pode sumir assim!! Tem que haver uma forma de evitar isso!!!"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("Sinto muito, mas n�o tem nada o que fazer. Apenas volte para a cabana!"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("O qu�?! Claro que n�o!! Eu irei descobrir o que aconteceu!! N�o vou deixar voc� sumir assim."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("N�o!! � muito perigoso. Os animais dessa regi�o foram corrompidos. Eles se transformaram em bestas agressivas. N�o � seguro. Volte agora!"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look();
                    StartCoroutine(RunDialogue("Eu..."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    StartCoroutine(RunDialogue("..."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    StartCoroutine(RunDialogue("Eu vou descobrir o que est� acontecendo. N�o posso deixar voc� sumir. E esse tamb�m � minha floresta."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    Look(0, GameObject.FindGameObjectWithTag("Anhaga"));
                    StartCoroutine(RunDialogue("Tainara..."));

                    yield return new WaitUntil(() => !run);
                    PlayerPrefs.SetString("c4Aux", "c4Aux");

                    yield return new WaitUntil(() => touched); touched = false;

                    StartCoroutine(RunDialogue("..."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    StartCoroutine(RunDialogue("Eu vou descobrir..."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    PlayerPrefs.SetString("c4", "c4");
                }
                break;
            case 5:
                if (!PlayerPrefs.HasKey("c5"))
                {
                    StartCoroutine(RunDialogue("Parece que as criaturas corrompidas vieram daqui. Se eu seguir por essa trilha talvez eu ache alguma resposta...?"));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    StartCoroutine(RunDialogue("Bem s� tem uma forma de descobrir."));

                    yield return new WaitUntil(() => !run); yield return new WaitUntil(() => touched); touched = false;

                    PlayerPrefs.SetString("end", "end");

                    PlayerPrefs.SetString("c5", "c5");
                }
                break;
        }
        Look(0, null);
        d_box.SetActive(false);
        if (!PlayerPrefs.HasKey("end"))
        {
            HudDisable(true);
            locked = false;
            running = false;
        }
        else
        {
            StartCoroutine(transicao());
        }
    }

    IEnumerator RunDialogue(string text, float speed = 0)
    {
        run = true;
        d_text.text = "";
        float t;
        if(speed == 0)
        {
            t = time;
        }
        else
        {
            t = speed;
        }

        for (int i = 0; i < text.Length; i++)
        {
            d_text.text += text[i];
            yield return new WaitForSeconds(t);
            if(touched)
            {
                d_text.text = text;
                touched = false;
                i = text.Length;
            }
        }
        run = false;
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

        end.SetActive(true);
        deathObj.gameObject.SetActive(false);
    }

    public void Quit()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}
