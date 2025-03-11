using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LaymsCoolNotifs
{
    public class Notifications : MonoBehaviour
    {
        public static GameObject CreateNotification(string message, bool useTMP, Color textColor, int textSize)
        {
            GameObject gameObject = new GameObject("Notification");
            GameObject gameObject2 = new GameObject("Notification Canvas");
            gameObject2.AddComponent<Canvas>().renderMode = 0;
            gameObject2.AddComponent<CanvasScaler>();
            gameObject2.AddComponent<GraphicRaycaster>();
            gameObject.transform.SetParent(gameObject2.transform);
            gameObject.AddComponent<RectTransform>();
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, 75f);
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
            gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(350f, -37f);
            if (useTMP)
            {
                TextMeshProUGUI textMeshProUGUI = gameObject.AddComponent<TextMeshProUGUI>();
                textMeshProUGUI.color = textColor;
                textMeshProUGUI.fontSize = (float)textSize;
                textMeshProUGUI.text = message;
                return gameObject;
            }
            Text text = gameObject.AddComponent<Text>();
            text.color = textColor;
            text.fontSize = textSize;
            text.text = message;
            return gameObject;
        }
    }
}
