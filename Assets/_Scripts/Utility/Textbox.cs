using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Textbox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform boxRoot;        // The transform you want to scale
    [SerializeField] private TMP_Text textField;    // The TMP text on the box
    [SerializeField] private BoxCollider2D coll2D;


    [Header("Animation Settings")]
    public float expandDuration = 0.25f;
    public float lineThickness = 0.05f; // thin line height
    private Coroutine showCoroutine; // reference to the delayed show coroutine


    private bool isOpen = false;

    private Vector3 closedScale;
    private Vector3 openScale;


    void Awake()
    {
        if (boxRoot == null) boxRoot = transform;
        coll2D = GetComponent<BoxCollider2D>();

        openScale = boxRoot.localScale;
        closedScale = new Vector3(openScale.x, lineThickness, openScale.z);

        // Start completely invisible
        boxRoot.localScale = new Vector3(openScale.x, 0f, openScale.z);
        textField.gameObject.SetActive(false);
    }

    public void ShowTextbox()
    {
        if (isOpen) return;
        isOpen = true;

        StartCoroutine(AnimateOpen());
    }

    public void HideTextbox()
    {
        if (!isOpen) return;
        isOpen = false;

        textField.gameObject.SetActive(false);
        StartCoroutine(AnimateClose());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (showCoroutine != null) StopCoroutine(showCoroutine);
            showCoroutine = StartCoroutine(DelayedShow());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (showCoroutine != null)
            {
                StopCoroutine(showCoroutine);
                showCoroutine = null;
            }

            HideTextbox();
        }
    }
    
    private IEnumerator AnimateOpen()
    {
        float t = 0f;

        boxRoot.gameObject.SetActive(true);
        boxRoot.localScale = closedScale;

        while (t < expandDuration)
        {
            t += Time.deltaTime;
            float p = t / expandDuration;

            boxRoot.localScale = Vector3.Lerp(closedScale, openScale, p);

            yield return null;
        }

        textField.gameObject.SetActive(true);
    }

    private IEnumerator AnimateClose()
    {
        float t = 0f;

        while (t < expandDuration)
        {
            t += Time.deltaTime;
            float p = t / expandDuration;

            boxRoot.localScale = Vector3.Lerp(openScale, closedScale, p);

            yield return null;
        }

        boxRoot.localScale = closedScale;
        boxRoot.gameObject.SetActive(false);
    }

    private IEnumerator DelayedShow()
    {
        yield return new WaitForSeconds(0.3f);

        ShowTextbox();
    }
}
