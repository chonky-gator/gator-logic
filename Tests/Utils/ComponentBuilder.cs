using UnityEngine;

namespace GatOR.Logic.Tests.Utils
{
	public struct ComponentBuilder<T> : IBuilder<T> where T : Component
	{
		private GameObject forGameObject;

		public ComponentBuilder<T> ForGameObject(GameObject gameObject)
		{
			forGameObject = gameObject;
			return this;
		}

		public T Build() => UnityObjectMother.AComponent<T>(forGameObject);

		public static implicit operator T(ComponentBuilder<T> builder) => builder.Build();
	}
}