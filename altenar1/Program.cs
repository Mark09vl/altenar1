using System;
using System.Collections;
using System.Collections.Generic;


namespace altenar1
{

    public interface IQueue<T> : IEnumerable<T>
    {
        public T Dequeue();
        public void Enqueue(T item);
        public int Count { get; }
    }

    public class TestQueue<T> : IQueue<T>
    {
        // Поля
        private T[] _array;
        private int _head;
        private int _tail;

        // Создаем очеред заданного размера
        public TestQueue(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException();
            _array = new T[capacity];
            _head = 0;
            _tail = 0;
            Count = 0;
        }

        /// Количество элементов в очереди
        public int Count { get; private set; }

        /// Удаление элемента из очереди
        public T Dequeue()
        {
            if (Count == 0)
                throw new InvalidOperationException();
            T local = _array[_head];
            _array[_head] = default(T);
            _head = (_head + 1) % _array.Length;
            Count--;
            return local;
        }

        /// Добавление элемента в очередь
        public void Enqueue(T item)
        {
            if (Count == _array.Length)
            {
                var capacity = (int)((_array.Length * 200L) / 100L);
                if (capacity < (_array.Length))
                    capacity = _array.Length;
                SetCapacity(capacity);
            }
            _array[_tail] = item;
            _tail = (_tail + 1) % _array.Length;
            Count++;
        }

        // Изменение размера очереди
        private void SetCapacity(int capacity)
        {
            var destinationArray = new T[capacity];
            if (Count > 0)
            {
                if (_head < _tail)
                    Array.Copy(_array, _head, destinationArray, 0, Count);
                else
                {
                    Array.Copy(_array, _head, destinationArray, 0, _array.Length - _head);
                    Array.Copy(_array, 0, destinationArray, _array.Length - _head, _tail);
                }
            }
            _array = destinationArray;
            _head = 0;
            _tail = (Count == capacity) ? 0 : Count;
        }

        /// Преобразование очереди в массив
        public T[] ToArray()
        {
            var destinationArray = new T[Count];
            if (Count != 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_array, _head, destinationArray, 0, Count);
                    return destinationArray;
                }
                Array.Copy(_array, _head, destinationArray, 0, _array.Length - _head);
                Array.Copy(_array, 0, destinationArray, _array.Length - _head, _tail);
            }
            return destinationArray;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public static class Program
    {
        private static void Main()
        {

            var TestQueue = new TestQueue<int>(1);

            // Добавляем элементы в очередь
            TestQueue.Enqueue(1);
            TestQueue.Enqueue(2);
            TestQueue.Enqueue(3);
            TestQueue.Enqueue(4);
            TestQueue.Enqueue(5);
            TestQueue.Enqueue(6);


            // Получаем очеред в виде массива и выводим
            foreach (int i in TestQueue.ToArray())
            {
                Console.WriteLine(i);
            }
                
            Console.WriteLine();

            // Удаляем все элементы из очереди и выводим
            while (TestQueue.Count > 0)
                Console.WriteLine(TestQueue.Dequeue());

            Console.ReadKey();
        }
    }
}