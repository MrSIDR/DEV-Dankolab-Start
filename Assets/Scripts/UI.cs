using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    private InputField heightInputField;
    [SerializeField]
    private InputField widthInputField;
    [SerializeField]
    private InputField colorInputField;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private GameObject message;
    [SerializeField]
    private Image messageImage;
    [SerializeField]
    private Text header;
    [SerializeField]
    private Text description;
    [SerializeField]
    private Text points;

    [SerializeField]
    private float stepAlpha = 0.01f;
    [SerializeField]
    private float timeUpdateAlpha = 0.01f;

    private int height;
    private int width;
    private int color;

    private void Start()
    {
        FieldManager.GettingPoints += UpdatePoints;

        message.SetActive(false);
    }

    private void OnDestroy()
    {
        FieldManager.GettingPoints -= UpdatePoints;
    }

    /// <summary>
    /// Метод проверяет валидность входных данных.
    /// </summary>
    public void ClickOnButton()
    {
        AudioManager.instance.ClickButton();

        if(heightInputField.text != "" && widthInputField.text != "" && colorInputField.text != "")
        {
            height = Convert.ToInt32(heightInputField.text);
            width = Convert.ToInt32(widthInputField.text);
            color = Convert.ToInt32(colorInputField.text);

            //Debug.Log("height = " + height + "\nwidth = " + width + "\ncolor = " + color);

            if (height >= 10 && height <= 50 && width >= 10 && width <= 50 && color >= 2 && color <= FieldManager.instance.characters.Count)
            {
                FieldManager.instance.ClickOnButton(width, height, color);
            }
            else StartCoroutine(ShowMessage());
        }
        else StartCoroutine(ShowMessage());
    }

    /// <summary>
    /// Корутина для вывод изображения с возможными входными данными.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowMessage()
    {
        if (!message.activeInHierarchy)
        {
            message.SetActive(true);

            float alpha = 1f;

            messageImage.color = new Color(1f, 1f, 1f, alpha);
            header.color = new Color(0f, 0f, 0f, alpha);
            description.color = new Color(0f, 0f, 0f, alpha);

            while (messageImage.color.a >= 0)
            {
                alpha -= stepAlpha;

                messageImage.color = new Color(1f, 1f, 1f, alpha);
                header.color = new Color(0f, 0f, 0f, alpha);
                description.color = new Color(0f, 0f, 0f, alpha);

                yield return new WaitForSecondsRealtime(timeUpdateAlpha);
            }

            message.SetActive(false);
        }
    }

    /// <summary>
    /// Метод выводит количество очков.
    /// </summary>
    private void UpdatePoints()
    {
        points.text = ": " + FieldManager.instance.GivePoints();
    }
}
