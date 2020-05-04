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

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        // Movement
        if (_controller.isGrounded)
{
            if (Input.GetKey(KeyCode.W)) // start move
            {
                MoveDirection = Vector3.forward * speed; 
                MoveDirection = transform.TransformDirection(MoveDirection); // permite que vc movimente o personagem em uma determinada direção
            }
            if (Input.GetKeyUp(KeyCode.W)) // stop move
            {
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
}
