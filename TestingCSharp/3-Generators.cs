using System;
using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace TestingCSharp
{
    public enum Color
    {
        Red, Green, Blue
    }

    public static class ColorInstances
    {
        public static Arbitrary<Color> ArbitraryColor()
        {
            return Arb.From(ColorGenerator);
        }

        private static Gen<Color> ColorGenerator =
            Gen.Elements(Color.Red, Color.Blue);
    }

    [Arbitrary(typeof(ColorInstances))]
    public class Generators
    {
        [Property]
        public void NoGreensAreGenerated(Color color)
        {
            Assert.NotEqual(Color.Green, color);
        }
    }

    public static class GenExamples
    {
        // Elements(params T[]) -> T:
        public static Gen<char> ABC = Gen.Elements('a', 'b', 'c');

        // Choose(int, int) -> int:
        public static Gen<int> FiveToTen = Gen.Choose(5, 10);
        public static Gen<int> TwentyToThirty = Gen.Choose(20, 30);

        // OneOf(params Gen<T>[]) -> T:
        public static Gen<int> Numbers = Gen.OneOf(FiveToTen, TwentyToThirty);

        // ArrayOf(Gen<T>) -> T[]:
        public static Gen<int[]> FivesToTens = Gen.ArrayOf(FiveToTen);   


        // For complex expressions, use the LINQ syntax:
        public static Gen<Tuple<int, int>> Complex =
            // from ... in Gen<T> produces a T
            from left in FiveToTen
            from right in TwentyToThirty
            select Tuple.Create(left, right);
    }


    static class AnimalGenerators
    {
        public static Arbitrary<Animal> Animal => Arb.From(GenerateAnimal);

        private static readonly Gen<Animal> GenerateAnimal =

            from legs in Gen.Choose(1, 10)
            from name in Arb.Generate<string>()

            select new Animal(legs, name);
    }
}
