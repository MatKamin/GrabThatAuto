using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class DisplayCurrentTime : MonoBehaviour
{
    private Text timeText;

    void Start()
    {
        timeText = GetComponent<Text>();

        if (timeText == null)
        {
            Debug.LogError("No Text component found on this GameObject. Please assign this script to a GameObject with a Text component.");
            return;
        }

        StartCoroutine(UpdateTimeCoroutine());
    }

    private IEnumerator UpdateTimeCoroutine()
    {
        while (true)
        {
            // Get the current system time in HH:mm format
            string currentTime = DateTime.Now.ToString("HH:mm");

            // Set the text of the Text component
            timeText.text = currentTime;

            // Wait for 60 seconds before updating again
            yield return new WaitForSeconds(60);
        }
    }
}
