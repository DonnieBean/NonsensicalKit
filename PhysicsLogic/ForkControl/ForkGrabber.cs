using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkGrabber : MonoBehaviour
{
    public NameTransformDic MaterialsPos;

    public Collider bc;

    public ForkMaterials crtMaterials;

    private void Start()
    {
        if (bc==null)
        {
            bc = GetComponent<Collider>();
        }
    }

    public bool CanSet(string materialsName)
    {
      return  MaterialsPos.Check(materialsName);
    }

    public bool SetForkMaterials(ForkMaterials forkMaterials)
    {
        if (MaterialsPos.GetTransform(forkMaterials.MaterialsName, out var v))
        {
            forkMaterials.transform.SetParent(v);
            forkMaterials.transform.localPosition = Vector3.zero;
            forkMaterials.transform.localEulerAngles = new Vector3(-90, 0, 0);
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
        Destroy(crtMaterials.gameObject);
           crtMaterials = null;
        bc.enabled = true;
    }
}
