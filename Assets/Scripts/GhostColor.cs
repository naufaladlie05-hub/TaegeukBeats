using UnityEngine;
using Spine.Unity;

/// <summary>
/// Script sederhana untuk mengubah warna skeleton animasi Spine secara manual via code.
/// </summary>
public class GhostColor : MonoBehaviour
{
    public Color myColor = Color.red; // spine manual color

    void Start()
    {
        var anim = GetComponent<SkeletonAnimation>();

        if (anim != null)
        {
            anim.skeleton.SetColor(myColor);
        }
    }
}