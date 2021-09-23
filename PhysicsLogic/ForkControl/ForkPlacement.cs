using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkPlacement : MonoBehaviour
{
    public NameTransformDic MaterialsPos;
    public Collider bc;

    public ForkMaterials crtMaterials;

    private void Awake()
    {
        if (bc == null)
        {
            bc = GetComponent<Collider>();
        }
    }

    public bool CanSet(string materialsName)
    {
        return MaterialsPos.Check(materialsName);
    }

    public bool SetForkMaterials(ForkMaterials forkMaterials)
    {
        if (MaterialsPos.GetTransform(forkMaterials.MaterialsName,out var v))
        {
            forkMaterials.transform.SetParent(v);
            forkMaterials.transform.localPosition = Vector3.zero;
            forkMaterials.transform.localEulerAngles = new Vector3(-90,0,0);
            bc.enabled = false;
            crtMaterials = forkMaterials;
            return true; 
        }
        return false;
    }


    public void Clear()
    {
        crtMaterials = null;
        bc.enabled = true;
    }

    public void Clean()
    {
        if (crtMaterials)
        {
            Destroy(crtMaterials.gameObject);
            crtMaterials = null;
            bc.enabled = true;
        }
    }
}

[System.Serializable]
public class NameTransformDic
{
    public NameTransformPair[] Dic;

    public bool Check(string materialsName)
    {
        foreach (var item in Dic)
        {
            if (item.name == materialsName)
            {
                return true;
            }
        }
        return false;
    }

    public bool GetTransform(string materialsName, out Transform pos)
    {
        pos = null;
        foreach (var item in Dic)
        {
            if (item.name == materialsName)
            {
                pos = item.transform;
                return true;
            }
        }
        return false;
    }

    [System.Serializable]
    public class NameTransformPair
    {
        public string name;
        public Transform transform;
    }
}
