using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
   // número de moedas coletadas 
   public int coins = 0; 

   // número de prismas coletados 
   public int prisms = 0; 
   
   // Referencia elemento interface de texto 
   public TMP_Text prismText;
   
   // Guarda um areferência para os controllers que criamos no InputAction
   private GameControls _gameControls;
   private PlayerInput _playerInput;
   private Camera _maincamera;
   private Vector2 _moveInput;
   private Rigidbody2D _rigidbody;
   
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

      _rigidbody = GetComponent<Rigidbody2D>();
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
      _rigidbody.AddForce(_moveInput * moveMultiplayer * Time.fixedDeltaTime); 

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
      RaycastHit2D collision = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, layerMask);
      
      if (collision.collider != null)
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
         _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
         
         // Manda a notificação da mudança do valor de coins 
         PlayerObserverManager.CoinsChanged(coins);
         
         //destrua o objeto do coin 
         Destroy(other.gameObject);
         
      }
      if (other.CompareTag("Prism"))
      {
         //Aumente o número de coins de um jogador em uma unidade 
         prisms ++;
         
         // Atualizar o numero de coins na interface 

         //prismText.text = prisms.ToString();
         
         // Manda a notificação da mudança do valor de coins 
         PlayerObserverManager.PrismChanged(prisms);
         
         //destrua o objeto do coin 
         Destroy(other.gameObject);
         
      }
      
   }
   
}
