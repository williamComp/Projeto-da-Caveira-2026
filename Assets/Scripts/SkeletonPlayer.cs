using UnityEngine;

public class SkeletonPlayer : MonoBehaviour
{
    // --------------------------------------------------
    // CONFIGURAÇÕES DE MOVIMENTO
    // --------------------------------------------------

    [Header("Movimento")]
    [SerializeField] private float velocidadeAndar = 2f;
    [SerializeField] private float velocidadeCorrer = 4f;
    [SerializeField] private float velocidadeAndarTras = 1.5f;
    [SerializeField] private float velocidadeRotacao = 120f;


    // --------------------------------------------------
    // CONFIGURAÇÕES DE PULO
    // --------------------------------------------------

    [Header("Pulo")]
    [SerializeField] private float alturaPulo = 1.5f;


    // --------------------------------------------------
    // CONFIGURAÇÕES DE GRAVIDADE
    // --------------------------------------------------

    [Header("Gravidade")]
    [SerializeField] private float gravidade = -9.81f;


    // --------------------------------------------------
    // COMPONENTES
    // --------------------------------------------------

    private Animator animator;
    private CharacterController controller;


    

    //Variáveis globais - estados do personagem
    private bool estaMovendo;
    private bool estaMovendoParaTras;
    private bool estaCorrendo;
    private bool estaNoChao;
    private bool estaCaindo;
    private bool estaPulando;

    //Variáveis globais - velocidades
    private float velocidadeAtual;
    private float velocidadeVertical;

    //Variáveis globais - moeda
    private int moedasColetadas = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    public void ColetarMoeda()
    {
        moedasColetadas++;

        Debug.Log("Moedas coletadas: " + moedasColetadas);
    }


    void Movimento()
    {
        bool pressionandoW = Input.GetKey(KeyCode.W);
        bool pressionandoS = Input.GetKey(KeyCode.S);

        estaMovendo =
            pressionandoW && !pressionandoS;

        estaMovendoParaTras =
            pressionandoS && !pressionandoW;
    }

    void Corrida()
    {
        // Só pode correr quando estiver indo para frente.
        estaCorrendo =
            estaMovendo &&
            (Input.GetKey(KeyCode.LeftShift) ||
             Input.GetKey(KeyCode.RightShift));
    }

    void Ataque()
    {
        // O botão esquerdo do mouse executa o ataque.
        // Por enquanto, o ataque só pode ser realizado no chão.
        if (Input.GetMouseButtonDown(0) && controller.isGrounded)
        {
            animator.SetTrigger("Attack");
        }
    }

    void Rotacao()
    {
        float direcaoRotacao = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            direcaoRotacao = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direcaoRotacao = 1f;
        }

        transform.Rotate(
            0f,
            direcaoRotacao * velocidadeRotacao * Time.deltaTime,
            0f
        );
    }

    void VelocidadeHorizontal()
    {
        velocidadeAtual = 0f;

        if (estaMovendo)
        {
            if (estaCorrendo)
            {
                velocidadeAtual = velocidadeCorrer;
            }
            else
            {
                velocidadeAtual = velocidadeAndar;
            }
        }
        else if (estaMovendoParaTras)
        {
            velocidadeAtual = -velocidadeAndarTras;
        }
    }

    void Gravidade()
    {
        estaNoChao = controller.isGrounded;

        if (estaNoChao && velocidadeVertical < 0f)
        {
            velocidadeVertical = -2f;
        }
    }

    void Pulo()
    {
        // Só permite pular se estiver no chão.
        if (estaNoChao && Input.GetKeyDown(KeyCode.Space))
        {
            velocidadeVertical =
                Mathf.Sqrt(
                    alturaPulo * -2f * gravidade
                );
        }

        velocidadeVertical +=
            gravidade * Time.deltaTime;
    }

    void MovimentoFinal()
    {
        // Cria inicialmente o movimento para frente ou para trás.
        Vector3 movimento =
            transform.forward * velocidadeAtual;

        // Acrescenta o movimento vertical causado pela gravidade.
        movimento.y = velocidadeVertical;

        // Executa os movimentos juntos.
        controller.Move(
            movimento * Time.deltaTime
        );
    }

    void DeteccaoQueda()
    {
         // A caveira está caindo quando:
        //
        // 1. não está encostando no chão;
        // 2. sua velocidade vertical está apontando para baixo.
        estaCaindo =
            !controller.isGrounded &&
            velocidadeVertical < -0.1f;
    }

    void DeteccaoPulo()
    {
        // A caveira está pulando enquanto estiver no ar
        // e sua velocidade vertical estiver apontando para cima.
        estaPulando =
            !controller.isGrounded &&
            velocidadeVertical > 0.1f;
    }

    void Animacoes()
    {
        animator.SetBool(
            "IsMoving",
            estaMovendo
        );

        animator.SetBool(
            "IsRunning",
            estaCorrendo
        );

        animator.SetBool(
            "IsMovingBackward",
            estaMovendoParaTras
        );

        animator.SetBool(
            "IsFalling",
            estaCaindo
        );

        animator.SetBool(
            "IsJumping",
            estaPulando
        );
    }

    void Update()
    {
        // --------------------------------------------------
        // MOVIMENTO PARA FRENTE E PARA TRÁS
        // --------------------------------------------------
        Movimento();
        


        // --------------------------------------------------
        // CORRIDA
        // --------------------------------------------------
        Corrida();
        


        // --------------------------------------------------
        // ATAQUE
        // --------------------------------------------------
        Ataque();


        // --------------------------------------------------
        // ROTAÇÃO
        // --------------------------------------------------
        Rotacao();
        


        // --------------------------------------------------
        // VELOCIDADE HORIZONTAL
        // --------------------------------------------------
        VelocidadeHorizontal();
        


        // --------------------------------------------------
        // GRAVIDADE
        // --------------------------------------------------
        Gravidade();
        


        // --------------------------------------------------
        // PULO
        // --------------------------------------------------
        Pulo();
        


        // --------------------------------------------------
        // MOVIMENTO FINAL
        // --------------------------------------------------
        MovimentoFinal();
        


        // --------------------------------------------------
        // DETECÇÃO DE QUEDA
        // --------------------------------------------------
        DeteccaoQueda();
       


        // --------------------------------------------------
        // DETECÇÃO DE PULO
        // --------------------------------------------------
        DeteccaoPulo();
        


        // --------------------------------------------------
        // ANIMAÇÕES
        // --------------------------------------------------
        Animacoes();
        
    }
}