using System.Collections;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textComponent;
    [TextArea(3, 10)] [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed;
    private int index;
    private delegate void DialogAudio();
    private DialogAudio playDialogAudio;

    private void Start() {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (textComponent.text == lines[index]) {
                NextLine();
            } else {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    private void StartDialogue() {
        index = 0;
        StartCoroutine(TypeLine());
    }

    private void PlayPlayerDialog() {
        SoundManager.Instance.Play("VoiceDialog");
    }

    private void PlayVoiceDialog() {
        SoundManager.Instance.Play("PlayerDialog");
    }

    private IEnumerator TypeLine() {
        playDialogAudio = index % 2 == 0 ? PlayPlayerDialog : PlayVoiceDialog;

        foreach (char character in lines[index]) {
            textComponent.text += character;
            playDialogAudio();
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine() {
        if (index < lines.Length - 1) {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
    }
}
