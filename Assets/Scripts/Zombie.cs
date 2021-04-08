using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Zombie : MonoBehaviour
{
    public static event Action<Zombie> Died;
    [SerializeField] float _attackRange = 1f;
    [SerializeField] int _health = 2;
    
    NavMeshAgent _navMeshAgent;
    Animator _animator;
    int _currentHealth;
    bool Alive => _currentHealth > 0;


    void Awake()
    {
        _currentHealth = _health;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _navMeshAgent.enabled = false;
    }

    void OnCollisionEnter(Collision other)
    {
        var blasterShot = other.collider.GetComponent<BlasterShot>();
        
        if (blasterShot != null)
        {
            _currentHealth--;
            
            if (_currentHealth <= 0)
            {
                var globalPositionOfContact = other.contacts[0].point;
                var relativePositionOfBullet = transform.InverseTransformPoint(globalPositionOfContact);
                Die(relativePositionOfBullet);
            }
            else
            {
                TakeHit();
            }
        }
    }

    void TakeHit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetTrigger("Hit");
    }

    void Die(Vector3 relativePositionOfBullet)
    {
        if (relativePositionOfBullet.z < 0)
            _animator.SetTrigger("DieFront");
        if (relativePositionOfBullet.z >= 0)
            _animator.SetTrigger("DieBack");
        
        GetComponent<Collider>().enabled = false;
        _navMeshAgent.enabled = false;
        Died?.Invoke(this);
        Destroy(gameObject,5f);
    }

    void Update()
    {
        if(!Alive)
            return;
        var player = FindObjectOfType<PlayerMovement>();
        if(_navMeshAgent.enabled)
            _navMeshAgent.SetDestination(player.transform.position);
        
        if (Vector3.Distance(transform.position,player.transform.position) < _attackRange)
        {
            Attack();
        }
    }

    void Attack()
    {
        _animator.SetTrigger("Attack");
        _navMeshAgent.enabled = false;
    }

    #region AnimationsCallback

    void AttackComplete()
    {
        if(Alive)
            _navMeshAgent.enabled = true;
    }
    
    void AttackHit()
    {
        Debug.Log("Player killed");
        SceneManager.LoadScene(0);
    }
    
    void HitComplete()
    {
        if (Alive)
            _navMeshAgent.enabled = true; 
    }

    // void IdleEnd()
    // {
    //     
    // }
    
    #endregion
    
    public void StartWalking()
    {
        _animator.SetBool("Moving",true);
        _navMeshAgent.enabled = true;
    }
    
}
