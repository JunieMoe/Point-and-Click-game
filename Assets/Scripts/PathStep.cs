using UnityEngine;

public struct PathStep
{
    public Transform node;
    public float speed;
    public string animationTrigger;

    public PathStep(Transform node, float speed, string animationTrigger)
    {
        this.node = node;
        this.speed = speed;
        this.animationTrigger = animationTrigger;
    }
}
