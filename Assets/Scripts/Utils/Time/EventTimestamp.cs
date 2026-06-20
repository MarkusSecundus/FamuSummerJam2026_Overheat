namespace MarkusSecundus.Utils.Time
{
	public struct EventTimestamp
	{
		public bool TryConsume(double timeout_seconds, bool useFixedTime = false)
		{
			double currentTime = useFixedTime ? UnityEngine.Time.fixedTimeAsDouble : UnityEngine.Time.timeAsDouble;
			double requiredTime = LastTimestamp_seconds + timeout_seconds;
			if (currentTime >= requiredTime)
			{
				LastTimestamp_seconds = currentTime;
				return true;
			}
			return false;
		}

		public double LastTimestamp_seconds { get; private set; }

		public static EventTimestamp Make() => new EventTimestamp { LastTimestamp_seconds = double.NegativeInfinity };
	}
}
