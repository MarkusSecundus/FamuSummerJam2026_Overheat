using UnityEngine;
using UnityEngine.Events;

namespace MarkusSecundus.Utils.Behaviors.Actions
{
    public class CountedAction : MonoBehaviour
    {
        [SerializeField] int MaxPerformedCount = 1;
        [SerializeField] UnityEvent Action;

        int _counter = 0;

        public void DoPerform()
        {
            if (_counter >= MaxPerformedCount) return;
            ++_counter;
            Action?.Invoke();
        }
        public void Increment(int toAdd)
        {
            _counter += toAdd;
        }
    }
}
