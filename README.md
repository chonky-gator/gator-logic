# GatOR Logic

Unity package to help with the development of games and apps. It features interface references on the inspector, a button to create a
ScriptableObject without the need of `[CreateAssetMenu]` and a Dependency Injection pattern for unity objects.

Note: If enough people require only one of the features, they will be eventually moved to their own 

## ReferenceOf\<T> - Serializing and displaying interface types on the inspector

Since we cannot serialize a interface reference by itself and show it on the inspector with `[SerializeField] private IExample example;`,
this package includes a generic struct `ReferenceOf<T>` that uses Unity's 2020.3 features like generic serialization and polyphormic serialization.

```C#
/*
* Any of these fields will be serialized and shown into the inspector, the same way it would
* happen with an UnityEngine.Object reference
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

The value in the inspector is shown with the type `<ITest>` in this case and we can set it as a Unity object. Unfortunately, I was only able to
display an object field that accepts any Unity Object, but when we assign it, it will filter any value that it's not the interface we want. Or
if it is a GameObject, it will try to get the first component with that interface.

![ReferenceOf example with Unity Objects](https://user-images.githubusercontent.com/29787965/214464474-d3f573ed-eca7-4aed-bd95-0d0d6343e29b.png)

Alternatively, it will also accept any class that implements that interface and has [Serializable]

![ReferenceOf example with custom serializable class](https://user-images.githubusercontent.com/29787965/214465013-d1a42893-9793-40cc-9139-b6ba12187d8a.png)

## [CreateAssetButton] - Easily create new ScriptableObjects when needed

This utility property drawer allows us to add the `[CreateAssetButton]` attribute to a field that references any ScriptableObject, this allows
us to skip adding `[CreateAssetMenu]` to the ScriptableObject class to avoid dealing with too many assets cluttering the Assets>Create menu and
creating the assets the moment we need them.

![CreateAssetButton on the inspector](https://user-images.githubusercontent.com/29787965/214557565-d26e6f2f-1609-4e2f-b1c7-7f3654fb02c4.png)

```C#
[CreateAssetButton] public TestSettings settings;
```

## IConstructable - Easily create dependency injection with Unity objects

Dependency Injection is a common pattern to help us decouple code and explicitly define dependencies. Normally the DI would be found in the
constructor, but because Unity objects can't be constructed because they are already instantiated, we have to pass the dependencies to a method
that initializes everything.

Luckily, this package has some interfaces and exceptions to help with this. You can add `IConstructable<...>` and `this.ThrowIfAlreadyConstructed()`
which help us repeat this design pattern.

```C#
// IConstructable 
public class Enemy : MonoBehaviour, IConstructable<IHealth, DifficultySettings>
{
  // I would reccommend to use private serialized fields to only allow the constructor
  // to modify these values.
  [SerializeField] private ReferenceOf<IHealth> health;
  [SerializeField] private DifficultySettings difficulty;

  // Setting this IConstructable property with a private set is the reccommended way
  // so we can set it to "true" when we construct the object
  public bool IsConstructed { get; private set; }
  
  public void Construct(IHealth health, DifficultySettings difficulty)
  {
    // This safeguards when IsConstructed is true. It will throw an
    // AlreadyConstructedException to avoid executing the code below.
    this.ThrowIfAlreadyConstructed();
    
    // We can also detect errors early, like null values
    this.health.Value = health ?? throw new ArgumentNullException(nameof(health));
    // With unity objects we can use the utility method .OrThrowNullArgument(argumentName)
    this.difficulty = difficulty.OrThrowNullArgument(nameof(difficulty));
    
    IsConstructed = true; // Do NOT forget to set IsConstructed to true
  }
  
  public void Damage(int amount)
  {
    // We can safeguard with a Debug.Assert to detect bugs related to the object not
    // being constructed yet.
    this.AssertAlreadyConstructed();
    this.ThrowIfNotConstructed(); // Or you can safeguard with throw if you prefer it that way
    health.Value.Damage(amount);
  }
}
```
