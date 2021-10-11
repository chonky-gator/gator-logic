using System;
using UnityEngine;


namespace GatOR.Logic.Tests
{
    public class UnityObjectMother
    {
        public static GameObject AGameObject(string name = "TEST", params Type[] components)
        {
            return new GameObject(name, components);
        }

        public static T AComponent<T>(GameObject gameObject = null) where T : Component
        {
            return gameObject.AddComponent<T>();
        }
    }
}