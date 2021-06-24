using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public static int total;
    public Image[] healthbar;

    private void Start()
    {
        health = healthbar.Length;
    }

    private void Update()
    {
        for (int i = 0; i < healthbar.Length; i++)
        {
            if (i < health) healthbar[i].enabled = true;
            else healthbar[i].enabled = false;
        }
    }
}
