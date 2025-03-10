using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DialogSystem
{
    public class DialogWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshPro dialogText;
        private SpriteRenderer sprite;

        private void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
            dialogText.color = Color.clear;
        }
        public void SetText(string text)
        {
            dialogText.text = text;
        }
        public void DestroyWindow()
        {
            Destroy(gameObject);
        }
        private void Update()
        {
            Color textColor = dialogText.color;
            textColor.a = sprite.color.a;
            dialogText.color = textColor;
        }
    }
}
