using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public float MaxEnergy = 1;
    public float MinEnergy = 0;
    public float DecreaseSpeedOfEnergy =0.05f;
    public Image EnergyImageBar;
    public float currentEnergyValue;
    //[SerializeField] private ThePlayerView PlayerViewScript; ADAPTARLO A ESTE PROYECTO

    private void Start()
    {
        currentEnergyValue = MaxEnergy;
        //PlayerViewScript.BatteryBar += BatteryUI; ADAPTARLO A ESTE PROYECTO
    }
    //HACER LO QUE ESTÁ AHORA EN ESTE UPDATE en una función de acá y dicha función suscribírla a un action en UVLantern(simular lo de )
    /*private void Update()
    {
        BatteryUI();
        
    }*/
    public void Update()
    {
        EnergyRecoveryIdleStateFunction();
        //HuntingModeEnergyConsumption();
    }

    public void HuntingModeEnergyConsumption()
    {
        currentEnergyValue = Mathf.Lerp(currentEnergyValue, MinEnergy, DecreaseSpeedOfEnergy * Time.deltaTime);

        EnergyImageBar.fillAmount = currentEnergyValue / MaxEnergy;
    }

    public void EnergyRecoveryIdleStateFunction()
    {
        currentEnergyValue = Mathf.Lerp(currentEnergyValue, MaxEnergy, DecreaseSpeedOfEnergy * Time.deltaTime);

        EnergyImageBar.fillAmount = currentEnergyValue / MaxEnergy;
    }
}
