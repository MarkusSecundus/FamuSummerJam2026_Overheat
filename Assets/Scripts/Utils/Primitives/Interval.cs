using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MarkusSecundus.Utils.Primitives
{
    public static class Interval
    {
        public static Interval<Vector3> Make(Vector3 a, Vector3 b) => new Interval<Vector3>(a.Min(b), a.Max(b));

        public static Interval<Vector2Int> Make(Vector2Int a, Vector2Int b) => new Interval<Vector2Int>(a.Min(b), a.Max(b));

        public static float Clamp(this float f, Interval<float> i) => Mathf.Clamp(f, i.Min, i.Max);
        public static Vector2 Clamp(this Vector2 v, Interval<Vector2> i) => new Vector2(v.x.Clamp(i.X()), v.y.Clamp(i.Y()));
        public static Vector3 Clamp(this Vector3 v, Interval<Vector3> i) => new Vector3(v.x.Clamp(i.X()), v.y.Clamp(i.Y()), v.z.Clamp(i.Z()));

        public static Interval<Vector2> AsInterval(this Rect rect) => new Interval<Vector2>(rect.min, rect.max);

        public static Interval<float> X(this Interval<Vector2> self) => new Interval<float>(self.Min.x, self.Max.x);
        public static Interval<float> Y(this Interval<Vector2> self) => new Interval<float>(self.Min.y, self.Max.y);

        public static Interval<float> X(this Interval<Vector3> self) => new Interval<float>(self.Min.x, self.Max.x);
        public static Interval<float> Y(this Interval<Vector3> self) => new Interval<float>(self.Min.y, self.Max.y);
        public static Interval<float> Z(this Interval<Vector3> self) => new Interval<float>(self.Min.z, self.Max.z);

        public static float Normalize(this float f, Interval<float> i) => (f.Clamp(i) - i.Min) / (i.Max - i.Min);

        public static bool Contains(this Interval<float> i, float f) => i.Min <= f && f < i.Max;
        public static bool Contains(this Interval<int> i, int t) => i.Min <= t && t < i.Max;

        public static Interval<Vector3> Enlarge(this Interval<Vector3> a , Interval<Vector3> b) => new Interval<Vector3>(new Vector3(Mathf.Min(a.Min.x, b.Min.x), Mathf.Min(a.Min.y, b.Min.y), Mathf.Min(a.Min.z, b.Min.z)), new Vector3(Mathf.Max(a.Max.x, b.Max.x), Mathf.Max(a.Max.y, b.Max.y), Mathf.Max(a.Max.z, b.Max.z)));
        public static Interval<Vector2Int> Enlarge(this Interval<Vector2Int> a , Interval<Vector2Int> b) => new Interval<Vector2Int>(new Vector2Int(Mathf.Min(a.Min.x, b.Min.x), Mathf.Min(a.Min.y, b.Min.y)), new Vector2Int(Mathf.Max(a.Max.x, b.Max.x), Mathf.Max(a.Max.y, b.Max.y)));

        public static float Average(this Interval<float> self) => (self.Min + self.Max) * 0.5f;

        public static float Lerp(this Interval<float> self, float t) => Mathf.Lerp(self.Min, self.Max, t);

        public static Interval<TOut> Transform<TIn, TOut>(this Interval<TIn> self, Func<TIn, TOut> transform) => new Interval<TOut>(transform(self.Min), transform(self.Max));
    }

    /// <summary>
    /// Object for defining intervals
    /// </summary>
    /// <typeparam name="T">Used numeric type</typeparam>
    [System.Serializable]
    public struct Interval<T>
    {
        /// <summary>
        /// Construct the interval
        /// </summary>
        /// <param name="min">Lower bound of the interval</param>
        /// <param name="max">Upper bound of the interval</param>
        public Interval(T min, T max) => (Min, Max) = (min, max);

        /// <summary>
        /// Lower bound of the interval
        /// </summary>
        public T Min;
        /// <summary>
        /// Upper bound of the interval
        /// </summary>
        public T Max;

        public override string ToString() => $"<{Min} ; {Max}>";
    }

}
