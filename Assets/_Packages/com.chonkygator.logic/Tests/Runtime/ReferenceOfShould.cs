using System.Collections;
using System.Collections.Generic;
using GatOR.Logic.Properties;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace GatOR.Logic.Tests
{
    [TestOf(typeof(ReferenceOf<>))]
    public class ReferenceOfShould
    {
        [Test]
        public void StartNull()
        {
            var referenceOf = ATestReference();
            
            Assert.That(referenceOf.type, Is.EqualTo(ReferenceOfType.Null));
            Assert.That(referenceOf.Reference, Is.Null);
        }
        
        [Test]
        public void AssignNonUnityObjects()
        {
            var referenceOf = ATestReference();
            var reference = new TestNonUnityObjectWithInterface();

            referenceOf.Reference = reference;
            
            Assert.That(referenceOf.type, Is.EqualTo(ReferenceOfType.SerializedReference));
            Assert.That(referenceOf.Reference, Is.Not.Null);
            Assert.That(referenceOf.Reference, Is.EqualTo(reference));
        }
        
        [Test]
        public void AssignUnityObjects()
        {
            var referenceOf = ATestReference();
            var reference = ATestUnityObject();

            referenceOf.Reference = reference;
            
            Assert.That(referenceOf.type, Is.EqualTo(ReferenceOfType.UnityObject));
            Assert.That(referenceOf.unityObject, Is.EqualTo(reference));
            Assert.That(referenceOf.Reference, Is.Not.Null);
            Assert.That(referenceOf.Reference, Is.EqualTo(reference));
        }

        public static ReferenceOf<ITestInterface> ATestReference() => new ReferenceOf<ITestInterface>();
        
        public static TestUnityObjectWithInterface ATestUnityObject() => ScriptableObject.CreateInstance<TestUnityObjectWithInterface>();

        
        public interface ITestInterface
        {
        }
        
        public class TestNonUnityObjectWithInterface : ITestInterface
        {
        }
        
        public class TestUnityObjectWithInterface : ScriptableObject, ITestInterface
        {
        }
    }
}
