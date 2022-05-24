using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontroller : MonoBehaviour
{
   private GameControls _gameControls;
   private PlayerInput _playerInput;
   private Camera _maincamera;
   private Vector2 _moveInput;
   private Rigidbody _rigidbody;
   public float moveMultiplayer; 

   private void OnEnable()
   {
      _rigidbody = GetComponent<Rigidbody>();
      
      _gameControls = new GameControls();

      _playerInput = GetComponent<PlayerInput>();

      _maincamera = Camera.main;

      _playerInput.onActionTriggered += OneActionTriggered; 
   }

   private void OnDisable()
   {
      _playerInput.onActionTriggered -= OneActionTriggered;
   }

   private void OneActionTriggered(InputAction.CallbackContext obj)
   {
      if (obj.action.name.CompareTo(_gameControls.Gameplay.Move.name) != 0)
      {
         _moveInput = obj.ReadValue<Vector2>();
      }
      
   }

   private void Move()
   {
      _rigidbody.AddForce((_maincamera.transform.forward * _moveInput.y +
                           _maincamera.transform.right * _moveInput.x) *
                           moveMultiplayer * Time.fixedDeltaTime); 

   }

   private void FixedUpdate()
   {
      Move(); 
   }
}
