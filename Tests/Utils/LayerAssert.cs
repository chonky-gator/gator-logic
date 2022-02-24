using NUnit.Framework;
using UnityEngine;


namespace GatOR.Logic.Tests.Utils
{
    public static class LayerAssert
    {
        public static int Exists(string name)
        {
            int index = LayerMask.NameToLayer(name);
            Assert.That(index, Is.GreaterThanOrEqualTo(0), () => $"Layer \"{name}\" doesn't exist.");
            return index;
        }

        public static int HasName(int index, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Assert.That(LayerMask.LayerToName(index), Is.Empty);
                return index;
            }
            int actualIndex = Exists(name);
            Assert.That(actualIndex, Is.EqualTo(index));
            return actualIndex;
        }
    }
}
