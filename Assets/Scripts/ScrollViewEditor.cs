using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewEditor : MonoBehaviour
{
    public ScrollRect scrollView;
    public TMP_Text textComponent;
    public TMP_InputField inputField;
    public Button editButton;
    public Button saveButton;

    private void Start()
    {
        inputField.gameObject.SetActive(false);
        saveButton.gameObject.SetActive(false);
        editButton.onClick.AddListener(EnableEditing);
        saveButton.onClick.AddListener(SaveAndExitEditing);
    }

    public void EnableEditing()
    {
        inputField.gameObject.SetActive(true);
        saveButton.gameObject.SetActive(true);
        scrollView.gameObject.SetActive(false);
        editButton.gameObject.SetActive(false);
        inputField.text = textComponent.text;
    }

    public void SaveAndExitEditing()
    {
        textComponent.text = inputField.text;
        inputField.gameObject.SetActive(false);
        saveButton.gameObject.SetActive(false);
        scrollView.gameObject.SetActive(true);
        editButton.gameObject.SetActive(true);
    }
}
