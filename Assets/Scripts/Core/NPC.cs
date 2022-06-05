using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public DialogMessage dialogMessage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Speak()
    {
        DialogManager.Instance.ShowMessage(dialogMessage);
    }

    public void OnInteract()
    {
        Speak();
    }
}
