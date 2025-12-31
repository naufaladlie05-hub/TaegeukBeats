using UnityEngine;
using Spine.Unity;

public class GhostColor : MonoBehaviour
{
    public Color myColor = Color.red; 

    void Start()
    {
        var anim = GetComponent<SkeletonAnimation>();

        if (anim != null)
        {
            anim.skeleton.SetColor(myColor);
        }
    }
}