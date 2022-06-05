using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;
    public UIDialog dialogUI;
    private bool _isOpened = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        dialogUI.onDialogEnded += () =>
        {
            _isOpened = false;
        };

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Message
    public void ShowMessage(DialogMessage msg)
    {
        if (_isOpened)
            return;
        dialogUI.SetData(msg);
        _isOpened = true;
    }
}
