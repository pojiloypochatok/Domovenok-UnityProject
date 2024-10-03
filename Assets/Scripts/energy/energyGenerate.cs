using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class energyGenerate : MonoBehaviour
{
    int maxEnergy = 10;
    private float timeToNextEnergy;
    private float energyGenerationInterval = 20f;

    [SerializeField]
    private TextMeshProUGUI energyCounter;

    void Start()
    {
        string[] parts = energyCounter.text.Split('/');
        int currentEnergyCounter = int.Parse(parts[0]);

        currentEnergyCounter = maxEnergy;
        timeToNextEnergy = energyGenerationInterval;
        UpdateEnergyDisplay(currentEnergyCounter);
    }

    void Update(){
        string[] parts = energyCounter.text.Split('/');
        int currentEnergyCounter = int.Parse(parts[0]);
        timeToNextEnergy -= Time.deltaTime;
        if (timeToNextEnergy <= 0)
        {
            GenerateEnergy(currentEnergyCounter);
            timeToNextEnergy = energyGenerationInterval;
        }
    }

    private void GenerateEnergy(int currentEnergyCounter)
    {
        if (currentEnergyCounter < maxEnergy)
        {
            currentEnergyCounter++;
            UpdateEnergyDisplay(currentEnergyCounter);
        }
    }

    private void UpdateEnergyDisplay(int currentEnergyCounter)
    {
        energyCounter.text = currentEnergyCounter + "/" + maxEnergy;
    }
}

