using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BlasterShot : MonoBehaviour
{
   [SerializeField] float _speed = 15f;

   public void Launch(Vector3 direction)
   {
      direction.Normalize();
      transform.up = direction;
      GetComponent<Rigidbody>().velocity = direction * _speed;
   }

   void OnCollisionEnter(Collision other)
   {
      Destroy(gameObject);
   }

   private void Start()
   {
      Destroy(gameObject,5f);
   }
}
