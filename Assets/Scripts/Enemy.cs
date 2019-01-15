using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    #region Delegates and Events
    public delegate void EmptyDelegate();
    public static event EmptyDelegate EnemyPassedEvent;
    #endregion

    #region Editor Variables
    [SerializeField]
    [Tooltip("The slowest an enemy will travel.")]
    private float m_MinSpeed = 3;

    [SerializeField]
    [Tooltip("The fastest an enemy will travel.")]
    private float m_MaxSpeed = 7;

    [SerializeField]
    [Tooltip("The explosion to display when an enemy is hit.")]
    private ParticleSystem m_HitExplosion;
    #endregion
    
    #region Cached References
    private Rigidbody2D cr_Rb;
    #endregion

    #region Initialization
    private void Awake()
    {
        cr_Rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        cr_Rb.velocity = Vector2.right * Random.Range(m_MinSpeed, m_MaxSpeed); ;
    }
    #endregion
    
    #region OnEnable and OnDisable
    private void OnEnable()
    {
        GameManager.GameEndedEvent += Die;
    }

    private void OnDisable()
    {
        GameManager.GameEndedEvent -= Die;
    }
    #endregion

    #region Collision Methods
    private void OnTriggerExit2D(Collider2D other)
    {
        EnemyPassedEvent?.Invoke();
        Die();
    }
    #endregion

    #region Death Methods
    public void Hit()
    {
        ParticleSystem ps = Instantiate(m_HitExplosion, transform.position, Quaternion.identity);
        Destroy(ps.gameObject, 2f);
        Die();
    }

    private void Die()
    {   
        Destroy(gameObject);
    }
    #endregion
}
