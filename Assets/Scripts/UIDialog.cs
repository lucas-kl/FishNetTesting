using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIDialog : MonoBehaviour
{
    public TMPro.TextMeshProUGUI titleText;
    public TMPro.TextMeshProUGUI contentText;
    public PlayerControlInputs playerControlInputs;

    public delegate void OnDialogEnded();
    public OnDialogEnded onDialogEnded;

    private Queue<string> _queue = new Queue<string>();

    public float clickTimeout = 0.25f;
    private float clickTimeDelta = 0;



    private void Awake()
    {
        //this.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Mouse.current.leftButton.isPressed && clickTimeDelta <= 0.0)
        {
            ShowNextText();
            clickTimeDelta = clickTimeout;
        }

        if(clickTimeDelta > 0.0f)
        {
            clickTimeDelta -= Time.deltaTime;
        }
    }


    public void SetData(DialogMessage msg)
    {

        titleText.text = msg.title;
        contentText.text = "";

        foreach (string seq in msg.sequences)
        {
            _queue.Enqueue(seq);
        }

        this.gameObject.SetActive(true);
        ShowNextText();
    }

    public void ShowNextText()
    {
        StopAllCoroutines();
        if (_queue.Count <= 0)
        {
            onDialogEnded();
            this.gameObject.SetActive(false);
            contentText.text = "";
            return;
        }

        contentText.text = "";
        StartCoroutine(RenderText());
    }
    
    private IEnumerator RenderText()
    {
        if (_queue.Count <= 0)
            yield break;

        string seq = _queue.Dequeue();
        for (int i = 0; i < seq.Length; i++)
        {
            contentText.text += seq[i];
            yield return new WaitForSeconds(0.05f);
        }
    }


}
