using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MarkusSecundus.Utils.Behaviors.GUI
{
    public class TMProFormatter : MonoBehaviour
    {
        [SerializeField] string Format;

        TMP_Text _text;
        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            if (string.IsNullOrWhiteSpace(Format))
                Format = _text.text;
        }

        public void SetText(params object[] args) => _text.text = string.Format(Format, args);

        public void SetTextWithIntArgument(int arg) => SetText(arg);

		public void SetTextWithFloatArgument(float arg) => SetText(arg);
		public void SetTextWithStringArgument(string arg) => SetText(arg);

	}
}