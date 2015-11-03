using System;
using FsCheck;
using FsCheck.Xunit;
using Newtonsoft.Json;
using Xunit;

namespace TestingCSharp
{
    public class Animal : IEquatable<Animal>
    {
        public Animal(int legs, string name)
        {
            Legs = legs;
            Name = name;
        }

        public int Legs { get; }

        public string Name { get; }

        public bool Equals(Animal other)
        {
            return other != null &&
                Legs == other.Legs &&
                Name == other.Name;
        }

        public override bool Equals(object obj) => Equals(obj as Animal);

        public override int GetHashCode() => unchecked((Legs * 397) ^ Name.GetHashCode());

        public override string ToString() => $"The '{Name}' has {Legs} legs";
    }

    // [Arbitrary(typeof(AnimalGenerators))]
    public class CustomTypes
    {
        static T RoundTripViaJson<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(json);
        }

        [Property]
        public void AnimalCanBeRoundtripped(int legs, string name)
        {
            var animal = new Animal(legs, name);
            var fromJson = RoundTripViaJson(animal);

            Assert.Equal(animal, fromJson);
        }

        [Property]
        public void AnimalCanBeRoundtripped_Better(Animal animal)
        {
            var fromJson = RoundTripViaJson(animal);
            Assert.Equal(animal, fromJson);
        }   
    }

}
