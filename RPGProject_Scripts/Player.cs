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

            yield return new WaitForSeconds(1.3f);

            _animator.SetInteger("Transition", transitionValue);
            _animator.SetBool("Attack1", false);
            attackIsReady = true;

        }
    }
}
