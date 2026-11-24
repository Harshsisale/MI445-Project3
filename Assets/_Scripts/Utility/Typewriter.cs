
using UnityEngine;
using System.Collections;
using TMPro;

public class Typewriter : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    [SerializeField] private Transform followObject;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, 0);
    [SerializeField] private float typingSpeed = 50;
    [SerializeField] private float duration = 2; //to keep the text active

    private Coroutine typingCoroutine;

    private int characterCount;

    public bool IsTyping { get; private set; }

    private void Awake()
    {
        if (textField == null)
        {
            Debug.LogError("Text field is not assigned to ", this);
            enabled = false;
            return;
        }
        hideText();
    }

    private void LateUpdate()
    {
        if (followObject == null)
        {
            return;
        }

        transform.position = followObject.position + offset;
        transform.rotation = Quaternion.identity;
    }

    public void showMessage(string message)
    {
        hideText();

        textField.text = message;
        textField.enabled = true;
        textField.maxVisibleCharacters = int.MaxValue;
        textField.ForceMeshUpdate();

        characterCount = textField.textInfo.characterCount;
        textField.maxVisibleCharacters = 0;

        IsTyping = true;
        typingCoroutine = StartCoroutine(typingInProgress());
    }

    //skip animation
    public void completeTyping()
    {
        if (IsTyping)
        {
            cancelRoutine();

            IsTyping = false;
            textField.maxVisibleCharacters = characterCount;
            typingCoroutine = StartCoroutine(keepMessageActive());
        }
        else
        {
            return;
        }
    }

    public void hideText()
    {
        cancelRoutine();
        IsTyping = false;

        if (textField != null)
        {
            textField.enabled = false;
        }
    }

    private IEnumerator typingInProgress()
    {
        float visibleCharacters = 0;

        while (visibleCharacters < characterCount)
        {
            visibleCharacters += typingSpeed * Time.deltaTime;

            textField.maxVisibleCharacters = Mathf.Min(
                Mathf.FloorToInt(visibleCharacters),
                characterCount);

            yield return null;
        }

        IsTyping = false;

        yield return new WaitForSeconds(duration);

        typingCoroutine = null;
        textField.enabled = false;
    }

    private IEnumerator keepMessageActive()
    {
        yield return new WaitForSeconds(duration);

        typingCoroutine = null;
        textField.enabled = false;
    }

    private void cancelRoutine()
    {
        if (typingCoroutine == null)
        {
            return;
        }

        StopCoroutine(typingCoroutine);
        typingCoroutine = null;
    }

    private void OnDisable()
    {
        hideText();
    }
}
