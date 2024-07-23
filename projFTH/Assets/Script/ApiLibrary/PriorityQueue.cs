using System.Collections.Generic;

namespace Script.ApiLibrary
{
    public class PriorityQueue<T>
    {
        private readonly List<(T element, int priority)> _elements = new();

        public int Count => _elements.Count;

        public void Enqueue(T element, int priority)
        {
            _elements.Add((element, priority));
        }

        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < _elements.Count; i++)
            {
                if (_elements[i].priority < _elements[bestIndex].priority)
                {
                    bestIndex = i;
                }
            }

            T bestElement = _elements[bestIndex].element;
            _elements.RemoveAt(bestIndex);
            return bestElement;
        }
    }
}