using System;
using UnityEngine;

namespace GatOR.Logic.Tests.Utils
{
    public struct GameObjectBuilder : IBuilder<GameObject>, IBuilder<Transform>
    {
        private Transform withParent;
        private string withName;
        private Type[] withComponents;

        private Vector3 withPosition;
        private Quaternion? withRotation;
        private Vector3? withScale;
        private bool localSpace;


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


        public GameObjectBuilder WithPosition(Vector3 position)
        {
            withPosition = position;
            return this;
        }

        public GameObjectBuilder WithRotation(Quaternion rotation)
        {
            withRotation = rotation;
            return this;
        }

        public GameObjectBuilder WithScale(Vector3 scale)
        {
            withScale = scale;
            return this;
        }

        public GameObjectBuilder OnLocalSpace()
        {
            localSpace = true;
            return this;
        }

        public GameObjectBuilder OnWorldSpace()
        {
            localSpace = false;
            return this;
        }


        public GameObject Build()
        {
            var go = new GameObject(withName, withComponents);
            var transform = go.transform;
            transform.SetParent(withParent);

            if (localSpace)
            {
                transform.localPosition = withPosition;
                if (withRotation.HasValue)
                    transform.localRotation = withRotation.Value;
            }
            else
            {
                transform.position = withPosition;
                if (withRotation.HasValue)
                    transform.rotation = withRotation.Value;
            }

            if (withScale.HasValue)
                transform.localScale = withScale.Value;

            return go;
        }

        public Transform BuildAsTransform() => Build().transform;

        Transform IBuilder<Transform>.Build() => BuildAsTransform();

        public static implicit operator GameObject(GameObjectBuilder builder) => builder.Build();
        public static implicit operator Transform(GameObjectBuilder builder) => builder.Build().transform;
    }
}
