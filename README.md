| Stage      | Status  |
|:-----------|:--------|
| master     | [![Build status](https://matteobortolazzo.visualstudio.com/CouchDB.NET/_apis/build/status/CI%20-%20Production)](https://matteobortolazzo.visualstudio.com/CouchDB.NET/_build/latest?definitionId=15) |
| Production | ![Release Stable status](https://matteobortolazzo.vsrm.visualstudio.com/_apis/public/Release/badge/ff4c14e0-5b2c-4782-b8ad-eb540731c000/3/4)                                                         |

Install it from NuGet: [https://www.nuget.org/packages/ModelValidation.Test](https://www.nuget.org/packages/ModelValidation.Test)

# ModelValidation.Test

Small Framework to check that models are validated properly.

I can be also useful for a **TDD** (test driven development) approach to model development.

It works virtually with any testing framework!

It checks that all attributes are tested.

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
            modelSetup.CheckObject(os => os.IsInvalidWith(r => r.Surname, "Organa"));
            modelSetup.CheckObject(os => os.IsInvalidWith(r => r.Age, 42));

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

## When test fails

* Validation do not fails after changing properties values;
* Create valid model function return an invalid model;
* Not all attributes fails at least with one provided property value.