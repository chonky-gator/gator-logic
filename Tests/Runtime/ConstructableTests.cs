using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace GatOR.Logic.Tests
{
    public class ConstructableTests
    {
        public class TestConstructable : IConstructable
        {
            public bool IsConstructed { get; set; }

            public TestConstructable(bool isConstructed) => IsConstructed = isConstructed;
        }

        [Test]
        public void ThrowsAlreadyConstructed()
        {
            var constructable = new TestConstructable(true);

            Assert.That(constructable.ThrowIfAlreadyConstructed,
                Throws.Exception.TypeOf<AlreadyConstructedException>());
        }

        [Test]
        public void ThrowsNotConstructed()
        {
            var constructable = new TestConstructable(false);

            Assert.That(constructable.ThrowIfNotConstructed,
                Throws.Exception.TypeOf<NotConstructedException>());
        }
    }
}
