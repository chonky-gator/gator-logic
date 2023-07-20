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
            
            Assert.That(referenceOf.kind, Is.EqualTo(ReferenceKind.Null));
            Assert.That(referenceOf.Value, Is.Null);
        }
        
        [Test]
        public void AssignNonUnityObjects()
        {
            var referenceOf = ATestReference();
            var reference = new TestNonUnityObjectWithInterface();

            referenceOf.Value = reference;
            
            Assert.That(referenceOf.kind, Is.EqualTo(ReferenceKind.SerializedReference));
            Assert.That(referenceOf.Value, Is.Not.Null);
            Assert.That(referenceOf.Value, Is.EqualTo(reference));
        }
        
        [Test]
        public void AssignUnityObjects()
        {
            var referenceOf = ATestReference();
            var reference = ATestUnityObject();

            referenceOf.Value = reference;
            
            Assert.That(referenceOf.kind, Is.EqualTo(ReferenceKind.UnityObject));
            Assert.That(referenceOf.unityObject, Is.EqualTo(reference));
            Assert.That(referenceOf.Value, Is.Not.Null);
            Assert.That(referenceOf.Value, Is.EqualTo(reference));
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
