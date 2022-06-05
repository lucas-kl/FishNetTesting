using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DialogMessage
{

    public string title;

    [TextArea(1, 10)]
    public string[] sequences;

    public DialogMessage(string title, string[] seq)
    {
        this.title = title;
        this.sequences = seq;
    }
}
