using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RAIN.Core;

public class Ghoul : MonoBehaviour
{

    private AIRig ghoulAiRig;
    private AI ghoulAi;

    // Use this for initialization
    void Start()
    {
        ghoulAiRig = gameObject.GetComponentInChildren<AIRig>();
        ghoulAi = ghoulAiRig.AI;
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    // Method to stop the AI and child components from updating
    public void Disable()
    {
        if (ghoulAi.Started)
        {
            ghoulAi.IsActive = false;
            gameObject.SetActive(false);
        }
    }

    // Method to start the AI and child components after being disabled
    public void Enable()
    {
        if (ghoulAi.Started & !ghoulAi.IsActive)
        {
            ghoulAi.IsActive = true;
            gameObject.SetActive(true);
        }
    }
}
