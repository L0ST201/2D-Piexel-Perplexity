// using TMPro;
// using UnityEngine;

// public class LastCompletedTimeDisplay : MonoBehaviour
// {
//     private TextMeshProUGUI lastCompletedTimeText;

//     void Start()
//     {
//         lastCompletedTimeText = GetComponent<TextMeshProUGUI>();
//         if (PlayerPrefs.HasKey("LastCompletedTime"))
//         {
//             float lastCompletedTime = PlayerPrefs.GetFloat("LastCompletedTime");
//             lastCompletedTimeText.text = FormatTime(lastCompletedTime);
//         }
//         else
//         {
//             lastCompletedTimeText.text = "N/A";
//         }
//     }

//     private string FormatTime(float time)
//     {
//         int minutes = Mathf.FloorToInt(time / 60);
//         int seconds = Mathf.FloorToInt(time % 60);
//         int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

//         return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
//     }
// }
