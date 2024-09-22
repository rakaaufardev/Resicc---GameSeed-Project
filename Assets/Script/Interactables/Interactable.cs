using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool IsInteracting { get; protected set; } = false;

    public virtual void Awake()
    {
        gameObject.layer = 6;
    }

    public abstract void OnInteract(Camera playerCamera);
    public abstract void OnFocus();
    public abstract void OnLoseFocus();
    public abstract void OnKeep();
    public abstract void OnThrow();
}
