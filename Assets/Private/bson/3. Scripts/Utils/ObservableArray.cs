using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class ObservableArray<T>
{
    private T[] _array;

    // 값 변경 시 발생할 이벤트
    public event Action OnValueChanged;

    // 생성자에서 배열 초기화
    public ObservableArray(int size)
    {
        _array = new T[size];
    }

    public ObservableArray(T[] array)
    {
        _array = new T[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            _array[i] = array[i];
        }
    }

    // 인덱서를 통해 배열 요소에 접근하고 값을 변경할 수 있음
    public T this[int index]
    {
        get
        {
            return _array[index];
        }
        set
        {
            if (!_array[index].Equals(value))
            {
                _array[index] = value;
                // 값이 변경되었을 때 이벤트 발생
                OnValueChanged?.Invoke();
            }
        }
    }

    // 배열 길이
    public int Length => _array.Length;

    public object[] ToArrayString()
    {
        object[] ret = new object[_array.Length];
        
        for (int i = 0; i < _array.Length; i++)
        {
            ret[i] = _array[i].ToString();
        }

        return ret;
    }
}
