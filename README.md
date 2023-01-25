# GatOR Logic

Unity package to help with the development of games and apps. It features interface references on the inspector, a button to create a
ScriptableObject without the need of `[CreateAssetMenu]` and a construct pattern for unity objects.

Note: If enough people require only one of the features, they will be eventually moved to their own 

## ReferenceOf\<T> - Serializing and displaying interface types on the inspector

Since we cannot serialize a interface reference by itself and show it on the inspector with `[SerializeField] private IExample example;`,
this package includes a generic struct `ReferenceOf<T>` that uses Unity's 2020.3 features like generic serialization and polyphormic serialization.

```C#
/*
* Any of these fields will be serialized and shown into the inspector, like it would happen with an UnityEngine.Object
* Of course, it does works as well if you replace "[SerializeField] private" with "public"
*/
[SerializeField] private ReferenceOf<IHealth> health;
private IHealth Health { get => health.Value; set => health.Value = value; }
[SerializeField] private ReferenceOf<IWeapon> weapon;

private void AttackWithWeapon()
{
  // The only downside is that you need to dereference using .Value
  weapon.Value.Attack();
}

public void EquipWeapon(IWeapon weapon)
{
  // You can update the reference value as well
  // It works even if the weapon is a UnityEngine.Object or a serializable/non-serializable value
  this.weapon.Value = weapon;
}

public void Damage(int amount)
{
  // You can create a get/set property to avoid writing .Value every time
  Health.Damage(amount)
}
```

The value in the inspector is shown with the type `<ITest>` in this case and we can set it as a Unity object. Unfortunately, I was only able to display
to accept any Unity Object, but when we assign it, it will filter any value that it's not the interface. Or if it is a GameObject, it will try to get
the first component with that interface.

![ReferenceOf example with Unity Objects](https://user-images.githubusercontent.com/29787965/214464474-d3f573ed-eca7-4aed-bd95-0d0d6343e29b.png)

Alternatively, it will also accept any class that implements that interface and has [Serializable]

![ReferenceOf example with custom serializable class](https://user-images.githubusercontent.com/29787965/214465013-d1a42893-9793-40cc-9139-b6ba12187d8a.png)

