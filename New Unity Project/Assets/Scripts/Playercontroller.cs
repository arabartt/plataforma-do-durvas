using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontroller : MonoBehaviour
{
   // número de moedas coletadas 
   public int coins = 0; 
   
   
   
   private GameControls _gameControls;
   private PlayerInput _playerInput;
   private Camera _maincamera;
   private Vector2 _moveInput;
   private Rigidbody _rigidbody;
   
   // diz se o jogador está no chão ou não 
   private bool _isGrounded; 
   
   public float moveMultiplayer;
   public float maxVelocity; 
   // Distancia que o raio vai percorrer procurando algo para bater 
   public float rayDistance;
   // Mascara de colisão com o chão 
   public LayerMask layerMask;

   public float jumpForce;

   private void OnEnable()
   {
      // associa a variável o componente rigidibody presente no objeto do jogador no unity 

      _rigidbody = GetComponent<Rigidbody>();
      // instancia um novo objeto a classe Gamecontrols 
      _gameControls = new GameControls();
      // associa a varável o componente PlayerInput presente no objeto do jogador no Unity
      _playerInput = GetComponent<PlayerInput>();
      // Associa a nossa varável o valor presente na variável main classe Camea, que é
      _maincamera = Camera.main;
      // Inscrevendo o delegate para a função que é chamada quando uma tecla/botão no control 
      _playerInput.onActionTriggered += OneActionTriggered;
      
      
   }

   private void OnDisable()
   {
      _playerInput.onActionTriggered -= OneActionTriggered;
   }

   private void OneActionTriggered(InputAction.CallbackContext obj)
   {
      if (obj.action.name.CompareTo(_gameControls.Gameplay.Move.name) == 0)
      {
         _moveInput = obj.ReadValue<Vector2>();
      }
      
      //Compara se a informação trazida do obj é referente ao comando de pular
      if (obj.action.name.CompareTo(_gameControls.Gameplay.Jump.name) == 0)
      {
         if (obj.performed) Jump();
      }
      
   }

   private void Move()
   {
      Vector3 camForward = _maincamera.transform.forward;

      Vector3 camRight = _maincamera.transform.right;

      camForward.y = 0;
      camRight.y = 0; 
      
      _rigidbody.AddForce((camForward * _moveInput.y +
                          camRight * _moveInput.x) *
                           moveMultiplayer * Time.fixedDeltaTime); 

   }

   private void FixedUpdate()
   {
      Move();
      LimitVelocity();
   }

   private void LimitVelocity()
   {
      // pega a velocidade no eixo x (usando a função Abs para ignorar o sinal negativo caso tenha)
      Vector3 velocity = _rigidbody.velocity;

      if (Mathf.Abs(velocity.x) > maxVelocity) velocity.x = Mathf.Sign(velocity.x) * maxVelocity;
     
      velocity.z = Mathf.Clamp(velocity.z, -maxVelocity, maxVelocity);
      

      _rigidbody.velocity = velocity; 
      
   }
   // vai ser usado para checar se o jogador está no chão ou não 
   private void CheckGround()
   {
      RaycastHit collision;

      if (Physics.Raycast(transform.position, Vector3.down, out collision, rayDistance, layerMask))
      {
         _isGrounded = true; 
      }
      else
      {
         _isGrounded = false;
      }
      
      
   }
   //função que vai ser chamada para fazer o jogador pula
   public void Jump()
   {
      if (_isGrounded)
      {
         _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
      }
      
   }
   
   private void Update()
   {
      CheckGround();
   }

   private void OnDrawGizmos()
   {
      Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.yellow);
   }

   // função que checa se um objeto entrou em um colisor setado com istrigged
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Coin"))
      {
         //Aumente o número de coins de um jogador em uma unidade 
         coins ++;
         //destrua o objeto do coin 
         Destroy(other.gameObject);
         
      }
      
   }
}
