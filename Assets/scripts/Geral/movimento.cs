using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Analog;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class movimento : MonoBehaviour
{
   
    // ANOTAÇÕES
    /* 
        Todos os códigos considerados como importantes e/ou reutilizáveis
    foram comentados e guardados no fim do código para caso haja necessidade
    de usar eles depois.

        Existem marcações no código indicando onde estavam estes códigos.
    - Iggy
    */
    
    //  //  // DECLARAÇÕES //  //  //
    #region 
    // INSPECTOR
    [Header("Movimento geral")]
    [SerializeField, Tooltip("Velocidade do movimento"), Range(0.01f, 7f)] float speed = 1.7f;
    [SerializeField, Tooltip("suavização do movimento"), Range(0, 0.3f)] float smooth = .1f;
    [SerializeField, Tooltip("Layer que representa o chão")] LayerMask mask;
    [SerializeField, Tooltip("Objeto vazio que detecta o chão")] public Transform groundCheck;
    [SerializeField, Tooltip("Tamanho do detector do chão")] float detectorChao = .2f;

    [Header("Pulo")]
    [SerializeField, Tooltip("Força do movimento")] float jumpForce = 16;
    [SerializeField, Tooltip("Suavização no ar (porcentagem em relação à suavização padrão)")] float airSmooth = 3;
    [SerializeField, Tooltip("Força do corte de pulo (soltar botão)"), Range(0, 1)] float jumpCutHeight = .87f;
    [SerializeField, Tooltip("Tempo em segundos de imprecisão permitido"), Range(0, .7f)] float toleranciaPulo = .2f;

    [Header("Dash")]
    [SerializeField, Tooltip("Velocidade do dash"), Range(1, 5)] float forcaDash = 1.37f;
    [SerializeField, Tooltip("Distância percorrida pelo dash"), Range(0, 10)] float distanciaDash = 4f;
    [SerializeField, Tooltip("Tempo de recarga do dash em segundos"), Range(0, 10)] float cooldownDash = 1f;

    [Header("Tiro")]
    [SerializeField, Tooltip("Tempo do cooldown"), Range(10, 20)] int tiro_limite ;
    float tiro_time;
    

    // OBJETOS EXTERNOS
    Collider2D attack;          // Colisor do ataque
    Rigidbody2D rb;             // RigidBody deste objeto
    Animator animin;// Componente de animação
    AudioSource audioSource;
    //Collider2D trig;            // Trigger para atravesar o chão
    BoxCollider2D col;      // Colisor do objeto
    Transform deathObj;
    Cutscenemanagement cutscene;

    // VARIÁVEIS AUXILIARES
        // movimento
    bool ladoDir = true;    // Virado para a direita?
    bool ativajoy = true;
        // pulo
    bool liberaChao = false;    // Está em contato com o chão?
    bool jumpCut = false;       // Botão pulo pressionado ainda?
    float prePulo, puloPos;     // Tempo de imprecisão do pulo
    float puloPosParede;        // Tempo de imprecisão
    public bool trig = false;   // Trigger para atravessar o chão

        // dash
    public bool dash = false;   // Dash liberado?
    bool dashing = false;       // Está fazendo dash?
    Vector3 dashStartPos;       // Posição de início do dash
    Vector2 dir;                // Direção do dash
    float dd;                   // Distância à percorrer
    bool dashTrig;              // Auxiliar de finalização

        // outros
    static Vector2 respawn;                 // Posição de respawn
    private Vector3 vel = Vector3.zero;     // Burocracia de código
    private Vector3 vel2 = Vector3.zero;
    bool alive = true;
    bool dying = false;
    bool anima_andar;
    #endregion

    //  //  // FUNÇÕES PRINCIPAIS //  //  //
    #region

    void Start()
    {
        //  Coleta todos os componentes necessários.
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Tainara/Correndo grama");
        rb = GetComponent<Rigidbody2D>();
        animin = GetComponent<Animator>();
        attack = GameObject.Find("melee zone").GetComponent<Collider2D>();
        deathObj = GameObject.FindGameObjectWithTag("deathHud").GetComponent<Transform>();
        //trig = groundCheck.GetComponent<Collider2D>();
        col = GetComponent<BoxCollider2D>();
        cutscene = GetComponent<Cutscenemanagement>();

        PlayerPrefs.SetInt("fase", SceneManager.GetActiveScene().buildIndex);
        if(PlayerPrefs.HasKey("x"))
        {
            respawn = new Vector2(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"));
        }
        else
        {
            respawn = transform.position;
        }

        //  Move para o spawnpoint e desativa colisor de combate.
        transform.position = respawn;
        attack.enabled = false;

        // Animação do começo
        StartCoroutine(Death(true));
    }


    void FixedUpdate()
    {
        
       
        animin.SetFloat("Yvel", rb.velocity.y);
        float tiro_fim = Mathf.FloorToInt(tiro_time);
        
        //GameObject.Find("tirobt").GetComponent<Image>().fillAmount = tiro_fim /  tiro_limite;
        // Cronômetros
        prePulo -= Time.fixedDeltaTime; puloPos -= Time.fixedDeltaTime; puloPosParede -= Time.fixedDeltaTime;

        // COLISOR CHÃO
        if(Physics2D.OverlapCircle(groundCheck.position, detectorChao, mask))   //  Cria um círculo de colisão com base nos parámetros predefinidos.
        {
            //  Se no chão seta a variável e ativa a tolerância de pulo.
            if(liberaChao != true)
            {
                audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Tainara/Queda grama");
                audioSource.loop = false;

                audioSource.Play();
            }
            liberaChao = true;
            
            if (rb.velocity.y < 0.3f)
            {
                animin.SetBool("jumping", false);
            }
            puloPos = toleranciaPulo;
            
        }
        else
        {

            //  Se não, seta variável.
            liberaChao = false;
        }

        // COLISOR PAREDE
        if (Physics2D.OverlapCircle(attack.transform.position, detectorChao, mask))   //  Cria um círculo de colisão com base nos parámetros predefinidos.
        {
            //  Se na parede ativa a tolerância de pulo.
            puloPosParede = toleranciaPulo;
        } 

        // PULO
        if (prePulo > 0 && puloPos > 0 && alive) 
        {
            animin.SetBool("jumping", true);
            animin.SetBool("walking", false);
            animin.SetBool("attacking", false);
            audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Tainara/Pulo");
            audioSource.loop = false;
            audioSource.Play();
            //  Caso tenha apertado em pulo e estado no chão em um tempo menor que a tolerancia de pulo, pule e ajuste as variáveis.
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            puloPos = 0;
            prePulo = 0;
            liberaChao = false;
        }

        // PULO NA PAREDE
        if (prePulo > 0 && puloPosParede > 0 && puloPos <= 0 && alive)
        {
            //  Caso tenha apertado em pulo e estado na parede em um tempo menor que a tolerancia de pulo, pule na direção oposta da parede e ajuste as variáveis.
            if (ladoDir)
            {
                rb.velocity = new Vector2(-jumpForce, jumpForce  /1.3f);
            }
            else
            {
                rb.velocity = new Vector2(jumpForce, jumpForce / 1.3f);
            }
            Flip();
            //StartCoroutine(movimentoParede());
            puloPosParede = 0;
            prePulo = 0;
        }

            // Suavização no ar
        float s = smooth;   //  Variável de suavização usada no código.
        if (!liberaChao)
        {
            // Se estiver no ar, atualize suavização.
            s *= airSmooth;
        }

        // MOVIMENTO
        if(joystick.horizontal > 1 && alive || joystick.horizontal < -1 && alive)
        {
            int sign = -1;
            if(joystick.horizontal > 0)
            {
                sign = 1;
            }
            if(liberaChao == true)
            {
                anima_andar = true;
                audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Tainara/Correndo grama");
                audioSource.loop = true;
                if(audioSource.isPlaying == false)
                {
                    audioSource.Play();
                }
                
            }
           

            Vector3 velocidade = Vector3.zero;

            if (Mathf.Abs(joystick.horizontal) > 2.75f)
            {
                velocidade = new Vector2(5 * sign * speed, rb.velocity.y);   //  Vetor da velocidade.
            }
            else
            {
                velocidade = new Vector2(1.5f * sign * speed, rb.velocity.y);   //  Vetor da velocidade.
            }
            rb.velocity = Vector3.SmoothDamp(rb.velocity, velocidade, ref vel, s);  //  Aplicação da suavização.
        }
        else
        {
            if(alive)
            rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(0, rb.velocity.y), ref vel, s);  //  Aplicação da suavização.
        }
       if(joystick.horizontal == 0 && anima_andar == true || liberaChao == false && anima_andar == true)
        {
            audioSource.Stop();
            audioSource.loop = false;
            anima_andar = false;
        }
            // Corte de pulo
        if (jumpCut && rb.velocity.y > 0 && alive)
        {
            // Se parou de clicar em pular e subindo, diminua velocidade.
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutHeight);
        }

        // LIMITE QUEDA
        if(rb.velocity.y < -20)
        {
            rb.velocity = new Vector2(rb.velocity.x, -20);
        }

        //•1 

        // DASH
        Dash(true);
    }


    private void Update()
    {
        // ELIMINAR EXPLOIT
        if (!animin.GetBool("attacking"))
        {
            attack.enabled = false;
        }


        if (tiro_time < tiro_limite)
        {
            tiro_time += Time.deltaTime;
        }
        // VIRAR PERSONAGEM
        if (joystick.horizontal > 1 /*&& ativajoy == true*/)
        {
            //  Se estiver andando para a direita rode a animação.
            animin.SetBool("walking", true);
            if (!ladoDir)
            {
                //  Se estiver virado para o lado errado, vire.
                Flip();
            }
        }
        else if (joystick.horizontal < -1 /*&& ativajoy == true*/)
        {
            //  Se estiver andando para a esquerda rode a animação.
            animin.SetBool("walking", true);
            if (ladoDir)
            {
                //  Se estiver virado para o lado errado, vire.
                Flip();
            }
        }
        else
        {
            //  Se estiver parado, pare a animação de andar.
            animin.SetBool("walking", false);
        }
    }
    #endregion

    //  //  // PROCEDIMENTOS //  //  //
    #region
    private void Flip()
    {
        //  Mude o lado na variável.
        ladoDir = !ladoDir;

        //  Muda o lado do objeto usando escala.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    public void Pulo(bool on)
    {
        if (on)
        {
            //  Se apertado, atualiza variável de apertando.
            jumpCut = false;
            if (joystick.vertical > -2.75f)
            {
                //  Se não estiver olhando para baixo pule antes da tolerância acabar.
                prePulo = toleranciaPulo;
            }
            else
            {
                //  Se olhando para baixo, lige colisor que desativa chão.
                if (GameObject.FindGameObjectWithTag("chao_atravessavel"))
                {
                    GameObject.FindGameObjectWithTag("chao_atravessavel").GetComponent<Collider2D>().usedByEffector = false;
                    GameObject.FindGameObjectWithTag("chao_atravessavel").layer = 8;
                }
                //trig = true;
                liberaChao = false;
                StartCoroutine(LigarChao(false));
            }
        }
        else
        {
            //  Se soltado, interrompa pulo.
            jumpCut = true;
        }
        //•2
    }

    public void Ataque_som1()
    {
        audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Tainara/ataque leve");
        audioSource.loop = false;
        
            audioSource.Play();
        
    }
    public void Ataque_som2()
    {
        audioSource.clip = Resources.Load<AudioClip>("Efeitos sonoros/Tainara/ataque pesado");
        audioSource.loop = false;

        audioSource.Play();

    }

    public void Ataque()
    {
        //  Se botão de ataque apertado, rode animação e ligue colisos de ataque.
        attack.enabled = true;
        animin.SetBool("attacking", true);
        animin.SetBool("jumping", false);

    }


    public void Tiro()
    {
        if (tiro_time >= tiro_limite)
        {
            Instantiate(Resources.Load("prefabio/tiro"), transform.position, Quaternion.identity);
            tiro_time = 0;
        }
        
    }

    void AtaqueFim(string sonio)
    {
        //  Quando ataque terminado, desligue colisor e animação.
        animin.SetBool("attacking", false);
        attack.enabled = false;
    }


    public void Dash(bool mode)
    {
        if (!mode && dash)
        {
            //  Se em início, ligue dashing, desligue variável interferente, ative cooldown e lembre a posição inicial.
            dash = false;
            dashing = true;
            jumpCut = false;
            dashStartPos = transform.position;
            Debug.Log("d0");
            StartCoroutine(DashCooldown());
            RaycastHit2D hit;

            if (Mathf.Abs(joystick.horizontal) + Mathf.Abs(joystick.vertical) > 2)
            {
                //  Se joystick estiver apontando, pegue direção e guarde se na distância do dash tem obstáculos.
                dir = Vector2.ClampMagnitude(new Vector2(joystick.horizontal, joystick.vertical), 1);
                //*1*
                //hit = Physics2D.CapsuleCast(new Vector2(transform.position.x, transform.position.y) + col.offset, col.size, col.direction, 0, new Vector2(joystick.horizontal, joystick.vertical), distanciaDash + 1.3f);
                hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y) + col.offset, col.size, 0, new Vector2(joystick.horizontal, joystick.vertical), distanciaDash + 1.3f);
            }
            else if (ladoDir)
            {
                //  Se não e estiver virado para a direita, use direção direita e guarde se na distância do dash tem obstáculos.
                dir = Vector2.right;
                //*2*
                //hit = Physics2D.CapsuleCast(new Vector2(transform.position.x, transform.position.y) + col.offset, col.size, col.direction, 0, Vector2.right, distanciaDash + 1.3f);
                hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y) + col.offset, col.size, 0, Vector2.right, distanciaDash + 1.3f);
            }
            else
            {
                //  Se não, use direção esquerda e guarde se na distância do dash tem obstáculos.
                dir = Vector2.left;
                //*3*
                //hit = Physics2D.CapsuleCast(new Vector2(transform.position.x, transform.position.y) + col.offset, col.size, col.direction, 0, Vector2.left, distanciaDash + 1.3f);
                hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y) + col.offset, col.size, 0, Vector2.left, distanciaDash + 1.3f);
            }

            if (hit)
            {
                //  Se tiver obstáculos, dash até o obstáculo.
                dd = hit.distance - 1.3f;
            }
            else
            {
                //  Se sem obstáculos, dash até distância máxima.
                dd = distanciaDash;
            }

            if (joystick.vertical < -3 || !liberaChao)
            {
                //  Se apontado para o chão ou não estiver no chão, permitir atravessar.
                //trig = true;
                if (liberaChao)
                {
                    //  Se no chão, regular força e atualizar variável.
                    dd += 1.3f;
                    liberaChao = false;
                }
            }
            else if (joystick.vertical < 0 && liberaChao)
            {
                //  Se no chão e apontando para o chão, ajusta direção para puramente horizontal.
                dir = Vector2.ClampMagnitude(new Vector2(joystick.horizontal * 5, 0), 1);
            }
        }
        else // ---------------------------------------------------------------------------------------------------------------------------------------
        {
            //  Após início
            if (dashing && Vector2.Distance(transform.position, dashStartPos) < dd)
            {
                //  Se não alcançou o destino do dash, continue.
                rb.velocity = Vector2.zero;
                rb.velocity = new Vector2(dir.x * jumpForce * forcaDash, dir.y * jumpForce * forcaDash);
                dashTrig = true;
            }
            else
            {
                if (dashTrig && Vector2.Distance(transform.position, dashStartPos) > dd - .2f)
                {
                    //  Se dash acabou e passou 0.2 do alvo, desligue atravesar o chão e desacelere.
                    //StartCoroutine(LigarChao(false));
                    dashTrig = false;
                    jumpCut = true;
                    rb.velocity = new Vector2(rb.velocity.x * .73f, rb.velocity.y * .87f);
                }
                //  Atualize status do dash
                dashing = false;
            }
        }
    }


    //•3


    //•4
    #endregion

    //  //  // PROCEDIMENTOS DO UNITY //  //  //
    #region

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("dano") && !dashing)
        {
            // Se atingido fora do dash, respawne.
            animin.SetTrigger("death");
            rb.gravityScale = 0;
            rb.velocity = ((-collision.transform.position + transform.position).normalized*6);
            StartCoroutine(Death(false));
        }
        if (collision.gameObject.CompareTag("music_fade_trigger"))
        {
            StartCoroutine(Music_Controller.fade_out());
            
        }
        if (collision.gameObject.CompareTag("musica_boss_trigger"))
        {
            Music_Controller.OnBossEnter();

        }
        if (collision.gameObject.CompareTag("Respawn"))
        {
            // Se encostou em checkpoint, dê o checkpoint.
            StartCoroutine(Checkpoint());
        }

        if(collision.gameObject.layer == 9)
        {
            cutscene.GetCutscene(collision.gameObject.tag);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("dano") && !dashing)
        {
            // Se atingido fora do dash, respawne.
            animin.SetTrigger("death");
            rb.gravityScale = 0;
            rb.velocity = ((-collision.transform.position + transform.position).normalized*6);
            StartCoroutine(Death(false));
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        //  Após passar do chão, ligue ele de novo.
        //StartCoroutine(LigarChao(true));
    }
    #endregion

    //  //  // COROUTINES //  //  //
    #region
    /*IEnumerator movimentoParede()
    {
        if ((rb.velocity.x > 0 && joystick.horizontal < 0) || (rb.velocity.x < 0 && joystick.horizontal > 0))
        {
            bool dir = ladoDir;
            ativajoy = false;
            speed = speed * -1;
            yield return new WaitUntil(() => liberaChao == true || joystick.horizontal == 0 || dir != ladoDir); //(rb.velocity.x <= -2.3f && joystick.horizontal < 0) || (rb.velocity.x >= 2.3f && joystick.horizontal > 0));
            speed = speed * -1;
            ativajoy = true;
        }
        yield return null;
    }
    */
    

    IEnumerator Checkpoint()
    {
        //  Espere até estar no chão e ative o checkpoint.
        yield return new WaitUntil(()=> liberaChao);
        respawn = transform.position;
        PlayerPrefs.SetFloat("x", respawn.x);
        PlayerPrefs.SetFloat("y", respawn.y);
        PlayerPrefs.Save();

        yield return null;
    }
  
    
    public void LC()
    {
        if (GameObject.FindGameObjectWithTag("chao_atravessavel"))
        {
            GameObject.FindGameObjectWithTag("chao_atravessavel").GetComponent<Collider2D>().usedByEffector = false;
            GameObject.FindGameObjectWithTag("chao_atravessavel").layer = 8;
        }
        //trig = true;
        liberaChao = false;
        StartCoroutine(LigarChao(false));
    }


    IEnumerator LigarChao(bool instantaneo)
    {
        if (!instantaneo)
        {
            //  Se não for instantâneo, espere e desligue.
            yield return new WaitForSeconds(0.5f);
            if (GameObject.FindGameObjectWithTag("chao_atravessavel"))
            {
                GameObject.FindGameObjectWithTag("chao_atravessavel").GetComponent<Collider2D>().usedByEffector = true;
                GameObject.FindGameObjectWithTag("chao_atravessavel").layer = 7;
            }
            //trig = false;
        }
        else
        {
            //  Se for intantâneo, desligue.
            if (GameObject.FindGameObjectWithTag("chao_atravessavel"))
            {
                GameObject.FindGameObjectWithTag("chao_atravessavel").GetComponent<Collider2D>().usedByEffector = true;
                GameObject.FindGameObjectWithTag("chao_atravessavel").layer = 7;
            }
            //trig = false;
        }
        yield return null;
    }


    IEnumerator DashCooldown()
    {
        //  Desliga o dash, espera e liga de novo.
        dash = false;
        Debug.Log("d1");
        yield return new WaitForSeconds(cooldownDash);
        Debug.Log("d2");
        yield return new WaitUntil(() => liberaChao && !dash);
        Debug.Log("d3");
        dash = true;
    }


    IEnumerator Death(bool start)
    {
        if (!dying)
        {
            float t = Time.time + .73f;

            Vector3 d;
            if (start)
            {
                deathObj.localPosition = Vector3.zero;
                d = new Vector3(respawn.x - 60f, respawn.y);
            }
            else
            {
                dying = true;
                alive = false;
                deathObj.position = deathObj.position + Vector3.right * 150;
                d = transform.position;
            }

            while (Time.time < t)
            {
                rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(0, rb.velocity.y), ref vel, .05f);
                deathObj.position = Vector3.SmoothDamp(deathObj.position, d, ref vel2, .27f);
                yield return null;
            }

            if (!start)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    
    
    #endregion

    //  //  // TESTE DE CÓDIGOS //  //  //
    #region
    

    #endregion
}

//  //  // CÓDIGO MORTO //  //  //
#region
//•1
/*/transform.Translate(new Vector2(joystick.horizontal*direcao * Time.deltaTime,0));
//Debug.Log(joystick.horizontal);
 if (Input.touchCount == 1)
 {
     Touch touch1 = Input.GetTouch(0);

     switch (touch1.phase)
     {
         case TouchPhase.Began:
             contagem++;

             break;
     }
 }
 else if (Input.touchCount == 2)
 {
     Touch touch1 = Input.GetTouch(1);

     switch (touch1.phase)
     {
         case TouchPhase.Began:
             contagem++;

             break;
     }
 }
 if (tempo > 0 && contagem == 1)
 {
     tempo -= Time.deltaTime;
 }
 else if (contagem == 2 && liberaChao == true)
 {
     rb.AddForce(new Vector2(0, 500));

     contagem = 0;
     tempo = 0.5f;
     liberaChao = false;
 }
 else if (contagem != 0)
 {
     contagem = 0;
     tempo = 0.5f;
 }
*/

//•2
/*Instantiate(tiroObj, transform.position, Quaternion.identity); 
 contagem = 0;
 tempo = 0.5f;
*/

//•3
/*private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("chao"))
    {
        liberaChao = true;

    }
} //*/

//•4
/*public void dash1()
    {
        //int layerMask = 0 << 3;                                                                             // Layer de colisão

        //player.transform.GetChild(1).transform.localPosition = new Vector2(dash, 0);                      // Alvo do dash
        //Debug.DrawRay(player.transform.position, player.transform.right, Color.cyan);
        //Debug.DrawLine(player.transform.position, player.transform.GetChild(1).transform.position, Color.red);
        RaycastHit2D hit;
        Vector2 dashposition;
        hit = Physics2D.Raycast(transform.position, new Vector2(joystick.horizontal,joystick.vertical),3);      // testar se tem colisão

        dashposition = hit.point;
        Vector2 dashoposicao = Vector2.ClampMagnitude(-(dashposition),1);
        Debug.Log(dashposition);
        if(hit == true)
        {
            transform.position = (dashposition + dashoposicao);
        }
        else
        {
            transform.Translate(Vector3.ClampMagnitude(new Vector3(joystick.horizontal, joystick.vertical, 0), 3));
        }
        
    } //*/

//*1*
//hit = Physics2D.Raycast(transform.position, new Vector2(joystick.horizontal, joystick.vertical), distanciaDash + 1.3f);

//*2*
//hit = Physics2D.Raycast(transform.position, Vector2.right, distanciaDash + 1.3f);

//*3*
//hit = Physics2D.Raycast(transform.position, Vector2.left, distanciaDash + 1.3f);
#endregion
