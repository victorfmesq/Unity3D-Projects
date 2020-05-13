using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float rotSpeed;
    private float rotation;
    public float gravity;

    private Vector3 MoveDirection;   
    private CharacterController _controller; // componente que detecta todo o ambiente do Terrain (faz a função do OnCollisionEnter)
    private Animator _animator;

    bool attackIsReady = true;

    List<Transform> EnemyList = new List<Transform>(); // -> criando uma list que recebera um inimigo toda vez que o player atacar o mesmo (Para evitar BUGS) 
    public float colliderRadius; // -----------------------> area de atuação do ataque do player

    public float enemyDamage = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.Move();
        this.GetMouseInput();
    }

    void Move()
    {
        // Movement
        if (_controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.W)) // start move
            {
                if (_animator.GetBool("Attack1") == false)
                {
                    _animator.SetBool("Walking", true);
                    _animator.SetInteger("Transition", 1);

                    MoveDirection = Vector3.forward * speed; 
                    MoveDirection = transform.TransformDirection(MoveDirection); // permite que vc movimente o personagem em uma determinada direção
                }
                else
                {
                    _animator.SetBool("Walking", false);

                    MoveDirection = Vector3.zero; // zera o Vector3 (0f, 0f, 0f)
                    StartCoroutine(Attack(1));
                }
            }
            if (Input.GetKeyUp(KeyCode.W)) // stop move
            {
                _animator.SetBool("Walking", false);
                _animator.SetInteger("Transition", 0);

                MoveDirection = Vector3.zero; // zera o Vector3 (0f, 0f, 0f)
            }
        }

        // Rotation
        rotation += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime; // soma, em rotation, input horizontal (A ou D) * velocidade de rotação * variação do tempo
        transform.eulerAngles = new Vector3(0, rotation, 0); // muda a rotação no eixo Y

        // Gravity
        MoveDirection.y -= gravity * Time.deltaTime; // subtrai o valor da gravidade de MoveDirection para que o personagem nao atravesse o chao

        // Effective Movementation
        _controller.Move(MoveDirection * Time.deltaTime);

    }

    void GetMouseInput()
    {
        if (_controller.isGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_animator.GetBool("Walking"))
                {
                    _animator.SetBool("Walking", false);
                    _animator.SetInteger("Transition", 0);
                }
                if  (!_animator.GetBool("Walking"))
                {
                    // Attack
                    StartCoroutine(Attack(0));
                }
            }
        }
    }

    IEnumerator Attack(int transitionValue)
    {
        if (attackIsReady == true)
        {
            attackIsReady = false;
            _animator.SetBool("Attack1", true);
            _animator.SetInteger("Transition", 2);

            yield return new WaitForSeconds(0.5f);

            GetEnemyRange();

            foreach (Transform enemies in EnemyList)
            {
                // executar ação de dano no inimigo
                Enemy enemy = enemies.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.GetHit(enemyDamage);
                }
            }

            yield return new WaitForSeconds(0.8f);

            _animator.SetInteger("Transition", transitionValue);
            _animator.SetBool("Attack1", false);
            attackIsReady = true;

        }
    }

    void GetEnemyRange()
    {
        EnemyList.Clear(); // limpa a lista pra nao bugar pois quando matamos um inimigo ele deve ser apagado da lista
        // OverlapSphere cria um colisor em forma de esfera invisivel e passamos a posição dele (posição atual do personagem + 1 no eixo Z * radius pra dar um tamanho para ela)
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                EnemyList.Add(c.transform); // adiciona um inimido na lista
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, colliderRadius);
    }
}
