using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRandomizer : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        float rand = Random.Range(0f, 1.5f);
        Invoke("StartAnim", rand);
    }

    void StartAnim()
    {
        anim.SetTrigger("AnimTrigger");
    }
}
