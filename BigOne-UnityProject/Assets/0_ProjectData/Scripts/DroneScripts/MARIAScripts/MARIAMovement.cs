using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MARIAMovement : MonoBehaviour
{
    [SerializeField]
    protected bool isMovementEnabled = false;

    [Header("Audio")]
    public AudioClip activationSFX;

    [Range(0f, 1f)]
    public float activationShaderValue;
    public float activationShaderChangeSpeed = 1;

    protected AudioSource audioSource;
    protected Animator animator;

    private MeshRenderer rend;
    private bool colorTransitionStopped = false;

    protected MARIABoredom mariaBoredom;

    protected virtual void Awake()
    {
        animator = GameObject.Find("Maria").GetComponent<Animator>();

        mariaBoredom = GetComponentInChildren<MARIABoredom>();

        audioSource = gameObject.GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        Transform espressioni = animator.transform.Find("espressioni facciali");

        rend = espressioni.GetComponent<MeshRenderer>();
    }

    public virtual void OnSelected()
    {
        audioSource.clip = activationSFX;
        audioSource.Play();
        StartCoroutine(UpdateMovementColor());
    }

    public virtual void StopMovement()
    {
        colorTransitionStopped = true;
        StopCoroutine(UpdateMovementColor());
        isMovementEnabled = false;
    }

    IEnumerator UpdateMovementColor()
    {
        colorTransitionStopped = false;

        float initColorValue = rend.materials[0].GetFloat("_Color");

        float direction = Mathf.Sign(activationShaderValue - initColorValue);

        while (!colorTransitionStopped && initColorValue != activationShaderValue)
        {
            initColorValue += Time.deltaTime * direction * activationShaderChangeSpeed;

            if (initColorValue * direction > activationShaderValue * direction)
            {
                initColorValue = activationShaderValue;
            }

            rend.material.SetFloat("_Color", initColorValue);

            yield return null;
        }

        colorTransitionStopped = false;
    }
}