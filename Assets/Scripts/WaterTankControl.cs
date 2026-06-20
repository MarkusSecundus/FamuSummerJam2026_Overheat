using MarkusSecundus.Utils.Primitives;
using UnityEngine;

public class WaterTankControl : MonoBehaviour
{
	[SerializeField] Interval<float> YScaleInterval;
	[SerializeField] Transform WaterVisualizer;

	public float Value { get; private set; }

	public void SetValue(float newValue)
	{
		newValue = newValue.Clamp01();
		Value = newValue;
		float newScale = YScaleInterval.Lerp(newValue);
		WaterVisualizer.localScale = WaterVisualizer.localScale.With(y: newScale); 
	}
}
