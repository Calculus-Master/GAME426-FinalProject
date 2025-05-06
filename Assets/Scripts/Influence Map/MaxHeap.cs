using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHeap<T> 
{
    private T[] _heap;
    private int _size;
    private int _cap;
    private const int StartCap = 10;
    private Func<T, T, int> maxComparisonFunction;
    
    private Dictionary<T, int> keys;

    public bool ContainsKey(T key)
    {
        return keys.ContainsKey(key);
    }
    
    public void IncreaseKey(T key)
    {
        keys[key] = Swim(key);
    }

    public void DecreaseKey(T key)
    {
        keys[key] = Sink(key);
    }

    public void RemoveKey(T key)
    {
        T swapped = _heap[--_size];
        Swap(_size, keys[key]);
        keys.Remove(key);
        if (keys.ContainsKey(swapped))
        {
            Swim(swapped);
            Sink(swapped);            
        }
    }
    
    public MaxHeap( Func<T, T, int> maxComparisonFunction)
    {
        keys = new Dictionary<T, int>();
        this.maxComparisonFunction = maxComparisonFunction;
        _heap = new T[StartCap];
        _size = 0;
        _cap = StartCap;
    }

    public void InsertNode(T node)
    {
        if (_size >= _cap)
        {
            ResizeHeap();
        }
        keys.Add(node, _size);
        _heap[_size++] = node;
        keys[node] = Swim();
    }


    public T PopMax()
    {
        if (_size == 0)
        {
            throw new Exception("Heap is empty");
        }

        T max = _heap[0];
        Swap(0, --_size);
        Sink();
        keys.Remove(max);
        return max;
    }

    public bool IsEmpty()
    {
        return _size == 0;
    }

    private int Swim(T key)
    {
        return SwimHelper( keys[key], key);
    }

    private int Swim()
    {
        T swimmer = _heap[_size - 1];
        var currentIndex = _size - 1;
        return SwimHelper(currentIndex, swimmer);
    }

    private int SwimHelper(int currentIndex, T swimmer)
    {
        var parentIndex = (currentIndex - 1) / 2;
        while (parentIndex >= 0)
        {
            //swap with parent until no longer smaller
            if (maxComparisonFunction(_heap[parentIndex], swimmer) < 0)
            {
                Swap(parentIndex, currentIndex);
                currentIndex = parentIndex;
                parentIndex = (parentIndex - 1) / 2;
            }
            else
            {
                break;
            }
        }

        return currentIndex;
    }

    private int Sink(T key)
    {
        return SinkHelper(keys[key], key);    
    }
    
    private int Sink()
    {
        var currentIndex = 0;
        var sinker = _heap[0];
        return SinkHelper(currentIndex, sinker);
    }

    private int SinkHelper(int currentIndex, T sinker)
    {
        var leftChildIndex = LeftChildIndex(currentIndex);
        var rightChildIndex = RightChildIndex(currentIndex);
        while (leftChildIndex < _size)
        {
            //want to swap with the bigger of the two children
            var maxChildIndex = leftChildIndex;
            if (rightChildIndex < _size)
            {
                maxChildIndex = maxComparisonFunction.Invoke(_heap[leftChildIndex], _heap[rightChildIndex]) >= 0
                    ? leftChildIndex
                    : rightChildIndex;
            }

            //keep sinking until there are no children bigger than the sinker
            if (maxComparisonFunction(_heap[maxChildIndex], sinker) > 0)
            {
                Swap(maxChildIndex, currentIndex);
                currentIndex = maxChildIndex;
                leftChildIndex = LeftChildIndex(currentIndex);
                rightChildIndex = RightChildIndex(currentIndex);
            }
            else
            {
                break;
            }
        }

        return currentIndex;
    }

    private static int RightChildIndex(int currentIndex)
    {
        var rightChildIndex = 2 * currentIndex + 2;
        return rightChildIndex;
    }

    private static int LeftChildIndex(int currentIndex)
    {
        var leftChildIndex = 2 * currentIndex + 1;
        return leftChildIndex;
    }

    private void Swap(int i, int j)
    {
        (_heap[i], _heap[j]) = (_heap[j], _heap[i]);
    }

    private void ResizeHeap()
    {
        var temp = _heap;
        _cap = 2 * _cap;
        _heap = new T[_cap];
        for (var i = 0; i < _size; i++)
        {
            _heap[i] = temp[i];
        }
    }
}