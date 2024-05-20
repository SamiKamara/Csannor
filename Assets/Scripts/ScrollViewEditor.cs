using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewEditor : MonoBehaviour
{
    public ScrollRect scrollView; // Reference to the ScrollView
    public TMP_Text textComponent; // Reference to the TextMeshPro text component in the ScrollView
    public TMP_InputField inputField; // Reference to the TMP_InputField
    public Button editButton; // Reference to the Edit button
    public Button saveButton; // Reference to the Save button

    private void Start()
    {
        // Initialize the TMP_InputField and buttons
        inputField.gameObject.SetActive(false); // Hide TMP_InputField at the start
        saveButton.gameObject.SetActive(false); // Hide Save button at the start

        // Add listeners to the buttons
        editButton.onClick.AddListener(EnableEditing);
        saveButton.onClick.AddListener(SaveAndExitEditing);
    }

    private void EnableEditing()
    {
        // Show the TMP_InputField and Save button
        inputField.gameObject.SetActive(true);
        saveButton.gameObject.SetActive(true);

        // Hide the ScrollView and Edit button
        scrollView.gameObject.SetActive(false);
        editButton.gameObject.SetActive(false);

        // Set the TMP_InputField's text to the current text in the ScrollView
        inputField.text = textComponent.text;
    }

    private void SaveAndExitEditing()
    {
        // Update the TextMeshPro text component with the TMP_InputField's text
        textComponent.text = inputField.text;

        // Hide the TMP_InputField and Save button
        inputField.gameObject.SetActive(false);
        saveButton.gameObject.SetActive(false);

        // Show the ScrollView and Edit button
        scrollView.gameObject.SetActive(true);
        editButton.gameObject.SetActive(true);
    }
}
