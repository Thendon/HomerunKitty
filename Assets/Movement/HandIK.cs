using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIK : MonoBehaviour
{
    public Transform target;
    public Quaternion offset;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, target.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, target.rotation * offset);

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, target.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, target.rotation * offset);

        animator.SetLookAtWeight(0.5f);
        animator.SetLookAtPosition(target.position);
    }
}
