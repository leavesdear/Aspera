using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillManager : MonoBehaviour
{
    public static BossSkillManager Instance;
    //public Dash_Skill dash { get; private set; }
    //public Clone_Skill clone { get; private set; }
    //public Crystal_Skill crystal { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        //dash = GetComponent<Dash_Skill>();
        //clone = GetComponent<Clone_Skill>();
        //crystal = GetComponent<Crystal_Skill>();
    }
}
