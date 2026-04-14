# Gilded Rose starting position in C# xUnit

## Build the project

Use your normal build tools to build the projects in Debug mode.
For example, you can use the `dotnet` command line tool:

``` cmd
dotnet build GildedRose.sln -c Debug
```

## Run the Gilded Rose Command-Line program

For e.g. 10 days:

``` cmd
GildedRose/bin/Debug/net8.0/GildedRose 10
```

## Run all the unit tests

``` cmd
dotnet test
```

# Explanation of changes

## Stage 1: Unit tests

A TDD approach is particularly usedul for refactoring, so I began by writing unit tests for `GildedRose.UpdateQuality` to cover the requirements specified in the root readme. Apart from conjured items still needing to be implemented, this uncovered two bugs:

- Quality was capped at 50, rather than 40.
- Backstage passes decreased in quality when `SellIn` was greater than 7. The specification is a little unclear on this case but I interpretted it as saying that quality should increase by 1.

I also found myself writing a test called `UpdateQuality_UpdatesSellInAndQuality`, which made me think that `UpdateQuality` would be better called `UpdateItems`, since it changes both values. I have not made that change to avoid breaking any checks that might be run against the solution.

## Stage 2: Refactor

The `UpdateQuality` method was a mess of nested conditions, special cases and magic numbers. My goal was separate the logic for each type of item, so they can each be understood or updated individually.

`UpdateQuality` now handles the logic that is shared for all item types, with the helper `CalcQualityChange` encapsulating the logic to work out how quality should change for each item. That is further split into methods for each type of item, so that adding a new item just requires adding a new method and updating `CalcQualityChange` to call it.

Note: I have left the special case for Sulfuras not updating `SellIn` in `UpdateQuality` since all other item types work the same. If another special case was needed in the future, it would probably be worth extracting the logic in a similar way to quality.

If the logic to update quality was needed in more than one place, it could be extracted from `GildedRose` to a new service class. This could then be injected into `GildedRose` and any other classes as a dependency.

Another option would be to take a more object-oriented approach and add an `Item.Update` method that defines the default logic. We could then create sub-classes for the other item types, which would override `Update` with their own logic.

## Item names

The specification talks about "legendary items", "backstage passes" and "conjured items", but the code uses specific item names like "Conjured Mana Cake". I have left that logic in place for the refactor but it would be worth clarifying whether e.g. there could be other conjured items like "Conjured Mana Draft". If so, there are a few ways it could be handled:

- Modify `CalcQualityChange` to check if `Item.Name` includes "conjured" or "backstage pass". This would not work for legendary items though, since they do not have "legendary" in their name.
- Add `IsLegendary`, `IsBackstagePass` and `IsConjured` properties to `Item`.
- Use the object-oriented approach described above.

Another possibility to consider is whether an item can be, for example, legendary and conjured, which would break the object-oriented approach and complicate the `CalcQualityChange` methods.