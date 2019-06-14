# ModelValidation.Test

Small Framework to check that models are validated properly.

I can be also useful for a **TDD** (test driven development) approach to model development.

It works virtually with any testing framework!

Install it from NuGet: [https://www.nuget.org/packages/ModelValidation.Test](https://www.nuget.org/packages/ModelValidation.Test)

## Main features
* Checks that model validation actually fails with wrong values.
* Checks that all properties are tested.
* Checks that all class level validation attributes are tested.
* Checks that all property level validation attributes are tested.
* Checks that error messages are actually correct.

## Example

C# query example:

```csharp
[YoungSkywalker] // Surname == Skywalker && Age < 25
public class Rebel
{
    [Required]
    [MaxLength(10)]
    public string Name { get; set; }

    [Required]
    public string Surname { get; set; }

    [Range(10, 900)]
    public int Age { get; set; }
}

[Fact]
public void Test_Luke()
{
    ModelValidator.Test(
        // Create valid model function
        () => new Rebel
        {
            Name = "Luke",
            Surname = "Skywalker",
            Age = 18
        },
        // Test setup function
        modelSetup => 
        {
            // Test object level attributes
            modelSetup.CheckClass(os => os.IsInvalidWith(r => r.Surname, "Organa"));
            modelSetup.CheckClass(os => os.IsInvalidWith(r => r.Age, 42));

            // Test property level attributes
            modelSetup.CheckProperty(r => r.Name, ps => ps.IsInvalidWith(null).IsInvalidWith("Lukelongname"));
            modelSetup.CheckProperty(r => r.Age, ps => ps.IsInvalidWith(901).IsInvalidWith(9));
        });
}

// Test that validation attributes return the correct message
[Fact]
public void Test_Stormtrooper()
{
    ModelValidator.Test(
        () => new Stormtrooper
        {
            IsCloned = true,
            Leader = "Palpatine"
        },
        modelSetup =>
        {
            modelSetup.CheckObject(os => os.IsInvalidWith(r => r.IsCloned, false), "Trooper must be a clone.");

            modelSetup.CheckProperty(r => r.Leader, ps => ps.IsInvalidWith(null, "Sith leader is required."));
        });
}
```

## Built-in extensions methods

There are a some built-in extension methods to help writing properties tests faster:

* `ps.IsRequired()`
* `ps.HasMaxLenght(int)`
* `ps.HasMinLenght(int)`
* `ps.HasMinValue(int)`
* `ps.HasMaxValue(int)`
* `ps.InRange(int, int)`
* `ps.HasMinValue(double)`
* `ps.HasMaxValue(double)`
* `ps.InRange(double, double)`

```csharp
// Example
[Fact]
ModelValidator.Test(
    () => new Stormtrooper
    {
        IsCloned = true,
        Leader = "Palpatine"
    },
    modelSetup =>
    {
        modelSetup.CheckProperty(r => r.Leader, ps => ps.IsRequired());
    });
```

## Options

The `Test` method accept an option object as third parameter.

* **CheckPropertiesCoverage:** *(default: true)* Checks that all properties are tested.
* **CheckClassAttributesCoverage:** *(default: true)* Checks that all class level attributes are tested.
* **ServiceProviderSetupAction:** Use this function to add services to using during validation. 