using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Animator animator;

    public Animator GetAnimator()
    {
        if(animator == null)
        {
            Debug.LogError("Animator is null on ");
        }
        return animator;
    }

    public void SetAnimatorBoolToTrue(string boolName)
    {
        animator.SetBool(boolName, true);
    }

    public void SetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void SetBool(string boolName, bool value)
    {
        animator.SetBool(boolName, value);
    }

    public void SetFloat(string floatName, float value)
    {
        Debug.Log("Setting Float " + floatName + " to " + value);
        animator.SetFloat(floatName, value);
    }
}
