using UnityEngine;
using System.Collections;
using System;

namespace AIFrame
{
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T instance = null;

        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        protected virtual void Awake()
        {
            instance = GetComponent<T>();
        }

    }
}
