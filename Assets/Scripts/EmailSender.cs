using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using TMPro;

public class EmailSender : MonoBehaviour
{
    public bool debugSend;
    public TMP_Text MessageContentTMP;

    private void Update()
    {
        if (debugSend)
        {
            SendMail();
            debugSend = false;
        }
    }

    public void SendMail()
    {
        StartCoroutine(SendMailCoroutine());
    }

    private IEnumerator SendMailCoroutine()
    {
        var task = Execute();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.IsFaulted)
        {
            Debug.LogError($"Failed to send email: {task.Exception?.Message}");
        }
        else
        {
            Debug.Log("Email sent successfully!");
        }
    }

    private async Task Execute()
    {
        string apiKey = PlayerPrefs.GetString("InjectedKey", "");
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("SendGrid API Key is not set.");
            return;
        }

        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(PlayerPrefs.GetString("SenderAddress", ""), "Sender");
        var subject = "Subject Here";
        var to = new EmailAddress(PlayerPrefs.GetString("ReceiverAddress", ""), "Receiver");
        var plainTextContent = MessageContentTMP.text;
        var htmlContent = "<strong>" + MessageContentTMP.text + "</strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        try
        {
            var response = await client.SendEmailAsync(msg);
            Debug.Log($"Status Code: {response.StatusCode}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to send email: {e.Message}");
        }
    }
}
