using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUIController : MonoBehaviour
{
    private PlayerStats player;
    private Animator anim;
    void Start()
    {
        player = (PlayerStats)GetComponentInParent<HealthBar>().myStats;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player.currentEnergy == 0)
        {
            anim.SetInteger("energy", 0);
        }
        else if (player.currentEnergy == player.GetMaxEnergyValue())
        {
            anim.SetInteger("energy", 2);
        }
        else if (player.currentEnergy >= player.useEnergyOnceNeed)
        {
            anim.SetInteger("energy", 1);
        }
    }
}
