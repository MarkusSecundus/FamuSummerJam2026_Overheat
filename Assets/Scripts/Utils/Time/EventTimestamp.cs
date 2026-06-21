namespace MarkusSecundus.Utils.Time
{
	public struct EventTimestamp
	{
		public bool TryConsume(double timeoutSeconds, bool useFixedTime = false)
		{
			if(Peek(timeoutSeconds, useFixedTime))
			{
				double currentTime = useFixedTime ? UnityEngine.Time.fixedTimeAsDouble : UnityEngine.Time.timeAsDouble;
				LastTimestamp_seconds = currentTime;
				return true;
			}
			return false;
		}

		public bool Peek(double timeoutSeconds, bool useFixedTime = false)
		{
			double currentTime = useFixedTime ? UnityEngine.Time.fixedTimeAsDouble : UnityEngine.Time.timeAsDouble;
			double requiredTime = LastTimestamp_seconds + timeoutSeconds;
			return (currentTime >= requiredTime);
		}

		public void Postpone(float amount_seconds)
		{
			LastTimestamp_seconds += amount_seconds;
		}
		public double LastTimestamp_seconds { get; private set; }

		public static EventTimestamp Make() => new EventTimestamp { LastTimestamp_seconds = double.NegativeInfinity };
	}
}
