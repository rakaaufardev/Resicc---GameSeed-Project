using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : Interactable
{
    public override void OnFocus()
    {
        print("lagi liat" + gameObject.name);
    }

    public override void OnInteract()
    {
        print("lagi interact" + gameObject.name);
    }

    public override void OnLoseFocus()
    {
        print("lagi gak liat" + gameObject.name);
    }
}
