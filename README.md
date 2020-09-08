# DomainModeling Cheat Sheet

## ValueObject

```csharp
public class Point : ValueObject
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }
    
    // yield return all properties of the ValueObject
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}
```

```csharp
public class Name : SimpleValueObject<string>
{
    public Name(string value)
        : base(value) {}
}

new Name("foo") == new Name("foo") // true
```

```csharp
public class Amount : ComparableValueObject<decimal>
{
    public Amount(decimal value)
        : base(value) {}
}

new Amount(2.4m) > new Amount(1.9m) // true
```

```csharp
public class Description : StringBasedValueObject
{
    public Description(string value)
        : base(value) {}
}

var desc = new Description("Best keyboard ever");
(string)desc == "Best keyboard ever" // true
```

```csharp
public class PersonId : Id<int>
{
    public PersonId(int value)
        : base(value) {}
}

public class PonyId : Id<int>
{
    public PonyId(int value)
        : base(value) {}
}

var personId = new PersonId(1);
var ponyId = new PonyId(1);
personId == new PersonId(1) // true
personId == ponyId // false
(int)personId == 1 // true
```

## Entity

```csharp
public class LaptopScreen : Entity<int>
{
    public LaptopScreen(int id, Size size)
        : base(id)
    {
        Size = size;
    }

    public Size Size { get; }
}

var screen = new LaptopScreen(1, Size.Inch(24));
var screenBis = new LaptopScreen(1, Size.Inch(32));
screen == screenBis // true
```

## AggregateRoot

```csharp
public class Person : AggregateRoot<PersonId>
{
    public Person(PersonId id, PersonName name, DateTime birthDay)
        : base(id)
    {
        Name = name;
        BirthDay = birthDay;
    }

    public PersonName Name { get; }
    public DateTime BirthDay { get; }
}

var clarkKent =
    new Person(
        1,
        new PersonName("Clark", "Kent"),
        new DateTime(1967, 04, 08));

var superman =
    new Person(
        1,
        new PersonName("Super", "Man"),
        new DateTime(-3027, 03, 31));

clarkKent == superman // true
```