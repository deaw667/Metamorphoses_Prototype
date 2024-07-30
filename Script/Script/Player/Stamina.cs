using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    #region singleton

    public static Stamina instance;
    public float staminaRegenRate;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    #endregion
    public Image staminabar;
    public float staminaamount;
    public float maxstamina = 100f;
    // Start is called before the first frame update
    void Start()
    {
        staminaamount = maxstamina;
    }

    private void UpdateCurrentStamina()
    {
        staminabar.fillAmount = staminaamount / 100f;
    }

    public void DrainStamina(float amount)
    {
        staminaamount -= Time.deltaTime * amount;
    }

    public void RegenStamina()
    {
        if(staminaamount < 100f)
        {
            staminaamount += Time.deltaTime * staminaRegenRate;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentStamina();
        RegenStamina();

        if(staminaamount <= 50)
        {
            PlayerController.Instance.SlowDownPlayer(2f);
            //Debug.Log("Stop Running You Gonna Die!!!");
        }

    }
}
