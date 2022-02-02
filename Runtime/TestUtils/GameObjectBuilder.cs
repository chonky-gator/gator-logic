using System;
using UnityEngine;

namespace GatOR.Logic.TestsUtils
{
    public struct GameObjectBuilder : IBuilder<GameObject>
    {
        public Transform withParent;
        public string withName;
        public Type[] withComponents;


        public GameObjectBuilder WithParent(Transform parent)
        {
            withParent = parent;
            return this;
        }

        public GameObjectBuilder WithParent(GameObject gameObject) => WithParent(gameObject.transform);

        public GameObjectBuilder WithName(string name)
        {
            withName = name;
            return this;
        }

        public GameObjectBuilder WithComponents(Type[] componentTypes)
        {
            withComponents = componentTypes;
            return this;
        }

        public GameObject Build()
        {
            var go = new GameObject(withName, withComponents);
            go.transform.SetParent(withParent);
            return go;
        }

        public static implicit operator GameObject(GameObjectBuilder builder) => builder.Build();
    }
}
