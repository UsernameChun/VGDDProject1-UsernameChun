using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update

    #region Editor Variables
    [SerializeField]
    [Tooltip("How much health this enemy has")]
    private int m_MaxHealth;
    [SerializeField]
    [Tooltip("How fast this enemy is")]
    private float m_Speed;
    [SerializeField]
    [Tooltip("How much damage does this enemy do per frame")]
    private float m_Damage;

    [SerializeField]
    [Tooltip("The explosion that occurs when this object dies")]
    private ParticleSystem m_DeathExplosion;

    [SerializeField]
    [Tooltip("The probability that this enemy will drop a health pill")]
    private float m_HealthPillDropRate;

    [SerializeField]
    [Tooltip("The type of health pill this enemy drops")]
    private GameObject m_HealthPill;

    [SerializeField]
    [Tooltip("The number of points killing this enemy gives")]
    private int m_Score;
    #endregion

    #region Private Variables
    private float p_curHealth;

    private float p_curSpeed;
    #endregion

    #region Cached Components

    private Rigidbody cc_Rb;
    #endregion

    #region Cached References
    private Transform cr_Player;
    #endregion

    #region Initialization

    private void Awake() {
        p_curHealth = m_MaxHealth;
        cc_Rb = GetComponent<Rigidbody>();
        p_curSpeed = m_Speed;
        // freeze();
    }

    private void Start() {
        cr_Player = FindObjectOfType<PlayerController>().transform;
    }
    #endregion

    #region Main Updates

    private void FixedUpdate() {
        Vector3 dir = transform.position - cr_Player.position;
        dir.Normalize();
        cc_Rb.MovePosition(cc_Rb.position - p_curSpeed * dir * Time.fixedDeltaTime);

    }
    #endregion

    #region Collision Methods
    private void OnCollisionStay(Collision collision) {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().DecreaseHealth(m_Damage);
        }
    }

    #endregion

    #region Health/Damage Methods
    public void DecreaseHealth(float amount) {
        p_curHealth -= amount;
        if (p_curHealth <= 0)
        {
            ScoreManager.singleton.IncreaseScore(m_Score);
            if (Random.value < m_HealthPillDropRate)
            {
                Instantiate(m_HealthPill, transform.position, Quaternion.identity);
            }
            Instantiate(m_DeathExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    #endregion

    #region Slowdown methods
    public void freeze() {
        Debug.LogError("Freezing");
        p_curSpeed = 0;
    }

    public void unfreeze() {
        Debug.LogError("Unfreezing");
        p_curSpeed = m_Speed;
    }
    #endregion
}
