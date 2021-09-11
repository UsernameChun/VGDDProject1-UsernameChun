using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    // Start is called before the first frame update
    #region Editor Variables
    [SerializeField]
    [Tooltip("All the main information about this particular ability")]
    protected AbilityInfo m_Info;
    #endregion
    #region Cached Components
    protected ParticleSystem cc_PS;
    #endregion

    #region Initialization
    private void Awake() {
        cc_PS = GetComponent<ParticleSystem>();

    }
    #endregion

    #region Use Methods
    public abstract void Use(Vector3 spawnPos);
    #endregion
}
