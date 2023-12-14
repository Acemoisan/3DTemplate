using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Animator animator;

    public void SetAnimatorBoolToTrue(string boolName)
    {
        animator.SetBool(boolName, true);
    }

    public void SetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
}
