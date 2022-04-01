using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PincherController : MonoBehaviour
{
    [SerializeField] private PincherFingerController fingerAController;
    [SerializeField] private PincherFingerController fingerBController;
    [SerializeField] private float gripSpeed = 3.0f;

    public JointState jointState { get; set; }

    public Action<bool> OnOpen;
    public Action<bool> OnClose;

    // Grip - the extent to which the pincher is closed. 0: fully open, 1: fully closed.
    private float grip;

    private void Start()
    {
        float gripChange = (float)jointState * gripSpeed * Time.fixedDeltaTime;
        float gripGoal = CurrentGrip() + gripChange;
        grip = Mathf.Clamp01(gripGoal);

        OnOpen?.Invoke(grip == 0);
        OnClose?.Invoke(grip == 1);
    }

    void FixedUpdate()
    {
        UpdateGrip();
        UpdateFingersForGrip();
    }

    /// <summary>
    /// 通过两个夹爪的插值平均数取得当前插值
    /// </summary>
    /// <returns></returns>
    private float CurrentGrip()
    {
        float meanGrip = (fingerAController.CurrentGrip() + fingerBController.CurrentGrip()) / 2.0f;
        return meanGrip;
    }

    private void UpdateGrip()
    {
        if (jointState != JointState.Fixed)
        {
            float gripChange = (float)jointState * gripSpeed * Time.fixedDeltaTime;
            float gripGoal = CurrentGrip() + gripChange;
            float temp = Mathf.Clamp01(gripGoal);

            if (temp == 0 && grip != 0)
            {
                OnOpen?.Invoke(true);
            }
            if (temp != 0 && grip == 0)
            {
                OnOpen?.Invoke(false);
            }
            if (temp == 1 && grip != 1)
            {
                OnClose?.Invoke(true);
            }
            if (temp != 1 && grip == 1)
            {
                OnClose?.Invoke(false);
            }
            grip = temp;
        }
    }

    private void UpdateFingersForGrip()
    {
        fingerAController.UpdateGrip(grip);
        fingerBController.UpdateGrip(grip);
    }
}
