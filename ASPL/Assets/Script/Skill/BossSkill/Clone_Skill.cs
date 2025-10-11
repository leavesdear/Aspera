using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]

    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private Transform parent;
    private void CreatClone(bool _cloneFaceLeft)
    {
        GameObject newClone = Instantiate(clonePrefab, boss.transform.position, Quaternion.identity, parent);
        newClone.GetComponentInChildren<CloneController>().SetBoolCloneFaceLeftValue(_cloneFaceLeft);
    }

    public void UseSkill(bool _cloneFaceLeft)
    {
        CreatClone(_cloneFaceLeft);
    }
}
