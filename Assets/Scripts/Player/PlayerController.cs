using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("How fast the player should move")]
    private float m_Speed;

    [SerializeField]
    [Tooltip("The transform of the Camera")]
    private Transform m_CameraTransform;

    [SerializeField]
    [Tooltip("A list of all attacks and information about them")]
    private PlayerAttackInfo[] m_Attacks;

    [SerializeField]
    [Tooltip("How much maximumhealth the player has")]
    private int m_MaxHealth;

    [SerializeField]
    [Tooltip("The HUD script")]
    private HUDController m_HUD;
    #endregion

    #region Cached Components
    private Rigidbody cc_Rb;
    #endregion

    #region Cached References
    private Animator cr_Anim;

    private Renderer cr_Renderer;
    #endregion


    #region Private Variables
    // this is the velocity of the player
    private Vector2 p_Velocity;

    private float p_frozenTimer;

    // the default color
    private Color p_DefaultColor;

    //player's current health
    private float p_curHealth;

    #endregion


    #region Initialization
    private void Awake() {
        p_Velocity = Vector2.zero;
        cc_Rb = GetComponent<Rigidbody>();
        p_frozenTimer = 0;
        cr_Anim = GetComponent<Animator>();
        cr_Renderer = GetComponentInChildren<Renderer>();
        p_DefaultColor = cr_Renderer.material.color;
        p_curHealth = m_MaxHealth;

        for (int i = 0; i < m_Attacks.Length; i += 1) 
        {
            PlayerAttackInfo attack = m_Attacks[i];
            attack.Cooldown = 0;

            if (attack.WindUpTime > attack.FrozenTime) {
                Debug.LogError(attack.AttackName + "Has a tim that is larger than the amount of tim ethat the player is p_frozenTimer for");
            }
        }
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region Main Updates
    private void Update() {
        if (p_frozenTimer > 0)
        {
            p_Velocity = Vector3.zero;
            p_frozenTimer -= Time.deltaTime;
            return;
        }
        else {
            p_frozenTimer = 0;
        }

        //Ability use
        for (int i = 0; i < m_Attacks.Length; i += 1) 
        {
            PlayerAttackInfo attack = m_Attacks[i];

            if (attack.isReady())
            {
                if (Input.GetButtonDown(attack.Button)) {
                    p_frozenTimer = attack.FrozenTime;
                    DecreaseHealth(attack.HealthCost);
                    StartCoroutine(UserAttack(attack));
                    break;
                } 
            } else if (attack.Cooldown > 0) 
            {
                attack.Cooldown -= Time.deltaTime;
            }
        }


        // movement
        float forward = Input.GetAxis("Vertical");
        float right = Input.GetAxis("Horizontal");

        // updating the animator
        cr_Anim.SetFloat("Speed", Mathf.Clamp01(Mathf.Abs(forward) + Mathf.Abs(right)));

        // movement controls
        float moveThreshhold = 0.3f;
        if (forward > 0 && forward < moveThreshhold) {
            forward = 0;
        } else if (forward < 0 && forward > moveThreshhold) {
            forward = 0;
        }
        if (right > 0 && right < moveThreshhold){
            right = 0;
        } else if (right < 0 && right > moveThreshhold) {
            right = 0;
        }
        p_Velocity.Set(right, forward);
    }

    private void FixedUpdate() {
        //updates the position of the Player
        cc_Rb.MovePosition(cc_Rb.position + m_Speed * Time.fixedDeltaTime * transform.forward * p_Velocity.magnitude);
        //updates the rotation of the Player
        cc_Rb.angularVelocity = Vector3.zero;
        if (p_Velocity.sqrMagnitude > 0) {
            float angleToRotCam =  Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.up, p_Velocity);
            Vector3 camForward = m_CameraTransform.forward;
            Vector3 newRot = new Vector3(Mathf.Cos(angleToRotCam) * camForward.x - Mathf.Sin(angleToRotCam) * camForward.z, 0, Mathf.Cos(angleToRotCam) * camForward.z + Mathf.Sin(angleToRotCam) * camForward.x);
            float theta = Vector3.SignedAngle(transform.forward, newRot, Vector3.up);
            cc_Rb.rotation = Quaternion.Slerp(cc_Rb.rotation, cc_Rb.rotation * Quaternion.Euler(0, theta, 0), 0.2f);
        }
    }
    #endregion

    #region Health/Dying Methods

    public void DecreaseHealth(float amount) {
        p_curHealth -= amount;
        m_HUD.UpdateHealthBar(1.0f * p_curHealth/m_MaxHealth);
        if (p_curHealth <= 0) 
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void IncreaseHealth(int amount) {
        p_curHealth += amount;
        if (p_curHealth > m_MaxHealth) 
        {
            p_curHealth = m_MaxHealth;
        }
        m_HUD.UpdateHealthBar(1.0f * p_curHealth / m_MaxHealth);
    }
    
    #endregion

    #region Attack Methods
    private IEnumerator UserAttack(PlayerAttackInfo attack)
    {
        cc_Rb.rotation = Quaternion.Euler(0, m_CameraTransform.eulerAngles.y, 0);
        cr_Anim.SetTrigger(attack.TriggerName);
        IEnumerator toColor = ChangeColor(attack.Color, 10);
        StartCoroutine(toColor);
        yield return new WaitForSeconds(attack.WindUpTime);

        Vector3 offset = transform.forward * attack.Offset.z + transform.right * attack.Offset.y + transform.up * attack.Offset.x;
        GameObject go = Instantiate(attack.AbilityGO, transform.position + offset, cc_Rb.rotation);
        go.GetComponent<Ability>().Use(transform.position + offset);
        StopCoroutine(toColor);
        StartCoroutine(ChangeColor(p_DefaultColor, 50));
        yield return new WaitForSeconds(attack.Cooldown);
        attack.ResetCooldown();
    }
    #endregion


    #region Misc Methods
    private IEnumerator ChangeColor(Color newColor, float speed) 
    {
        Color curColor = cr_Renderer.material.color;
        while (curColor != newColor) 
        {
            curColor = Color.Lerp(curColor, newColor, speed/100);
            cr_Renderer.material.color = curColor;
            yield return null;
        }
    }
    #endregion

    #region Collision Methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPill"))
        {
            IncreaseHealth(other.GetComponent<HealthPill>().HealthGain);
            Destroy(other.gameObject);
        }
    }
    #endregion
}
