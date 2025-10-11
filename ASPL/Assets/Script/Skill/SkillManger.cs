using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManger : MonoBehaviour
{
    public static SkillManger Instance;
    public Smash_skill smash { get; private set; }
    public Clash_Skill clash { get; private set; }
    //public Dash_Skill dash { get; private set; }
    public CloneClash_Skill cloneClash { get; private set; }
    public Clone_Skill clone { get; private set; }
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
        smash = GetComponent<Smash_skill>();
        clash = GetComponent<Clash_Skill>();
        //dash = GetComponent<Dash_Skill>();
        cloneClash = GetComponent<CloneClash_Skill>();
        clone = GetComponent<Clone_Skill>();
        //crystal = GetComponent<Crystal_Skill>();
    }
}
