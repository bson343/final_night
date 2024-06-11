using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public interface IRegisterable
{
    public void Init();
}

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator instance;
    public static ServiceLocator Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<ServiceLocator>();
                instance.Init();
            }
            return instance;
        }
    }

    private IDictionary<object, IRegisterable> services;

    private void Init()
    {
        services = new Dictionary<object, IRegisterable>();
    }

    public T GetService<T>() where T : MonoBehaviour, IRegisterable
    {
        if (!services.ContainsKey(typeof(T)))
        {
            // 없으면 자식 중에 있는지 확인하고 초기화해준다.
            T manager = FindObjectOfType<T>();
            if(manager != null)
            {
                services[typeof(T)] = manager;
                manager.Init();

                return (T)services[typeof(T)];
            }
            
            Assert.IsTrue(false, "존재하지 않는 매니저 호출");
            return default(T);
        }

        return (T)services[typeof(T)];
    }
}