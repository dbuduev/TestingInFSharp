using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using Xunit.Sdk;

namespace TestingCSharp
{
    public class User
    {
        public User(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; }

        public int Age { get; }

        public override string ToString() => $"Name: '{Name}' Age: {Age}";
    }


    public interface IUserHandler
    {
        void AddUser(User user);

        User GetUser(string name);

        void DeleteUser(string user);
    }


    public abstract class Operation
    {
        public abstract void Apply(IUserHandler handler);
    }


    public class AddOperation : Operation
    {
        public User User { get; }

        public AddOperation(User user)
        {
            User = user;
        }

        public override void Apply(IUserHandler handler)
        {
            handler.AddUser(User);
        }

        public override string ToString() => $"Add({User})";
    }

    public class GetOperation : Operation
    {
        public NonNull<string> UserName { get; }

        public GetOperation(NonNull<string> userName)
        {
            UserName = userName;
        }

        public override void Apply(IUserHandler handler)
        {
            handler.GetUser(UserName.Item);
        }

        public override string ToString() => $"Get('{UserName.Item}')";
    }

    public class DeleteOperation : Operation
    {
        public NonNull<string> UserName { get; }

        public DeleteOperation(NonNull<string> username)
        {
            UserName = username;
        }

        public override void Apply(IUserHandler handler)
        {
            handler.DeleteUser(UserName.Item);
        }

        public override string ToString() => $"Delete('{UserName.Item}')";
    }


    public static class OperationGenerators
    {
        public static Arbitrary<User> User() =>
            Arb.From(GenerateUser, ShrinkUser);

        private static Gen<User> GenerateUser =
            from age in Gen.Choose(1, 80)
            from name in Arb.Generate<NonNull<string>>()
            select new User(name.Get, age);

        private static IEnumerable<User> ShrinkUser(User user)
        {
            foreach (var name in Arb.Shrink(user.Name))
            {
                if (name != null)
                {
                    yield return new User(name, user.Age);
                }
            }

            foreach (var age in Arb.Shrink(user.Age))
            {
                yield return new User(user.Name, age);
            }
        }

        public static Arbitrary<Operation> Operation() =>
            Arb.From(GenerateOperation, ShrinkOperation);

        private static Gen<Operation> GenerateOperation =
            Gen.OneOf(
                Arb.Generate<DeleteOperation>().Select(x => (Operation)x), 
                Arb.Generate<GetOperation>().Select(x=> (Operation)x),
                Arb.Generate<AddOperation>().Select(x => (Operation)x));

        private static IEnumerable<Operation> ShrinkOperation(Operation operation)
        {
            if (operation is DeleteOperation)
            {
                var names = Arb.Shrink(((DeleteOperation)operation).UserName);
                return names.Select(name => new DeleteOperation(name));
            }
            else if (operation is AddOperation)
            {
                var users = Arb.Shrink(((AddOperation)operation).User);
                return users.Select(user => new AddOperation(user));
            }
            else
            {
                var names = Arb.Shrink(((GetOperation)operation).UserName);
                return names.Select(name => new GetOperation(name));
            }
        } 
    }

    class ProductionUserHandler : IUserHandler
    {
        private readonly Dictionary<string, User> _users = new Dictionary<string, User>();

        public int UserCount => _users.Count;
   
        public void AddUser(User user)
        {
            _users[user.Name] = user;
        }

        public User GetUser(string name)
        {
            User result;
            _users.TryGetValue(name, out result);
            return result;
        }

        public void DeleteUser(string user)
        {
            if (user.Contains("*")) // catastrophic bug!
            {
                _users.Clear();
            }
            else
            {
                _users.Remove(user);
            }
        }
    }


    class UserHandlerModel : IUserHandler
    {
        private readonly HashSet<string> _users = new HashSet<string>(); 

        public void AddUser(User user)
        {
            _users.Add(user.Name);
        }

        public User GetUser(string name)
        {
            return null;
        }

        public void DeleteUser(string user)
        {
            _users.Remove(user);
        }

        public void Validate(ProductionUserHandler realHandler)
        {
            Assert.Equal(_users.Count, realHandler.UserCount);
        }
    }

    [Arbitrary(typeof(OperationGenerators))]
    public class ModelBasedTests
    {
        private void ApplyOperations(IEnumerable<Operation> operations, IUserHandler handler)
        {
            foreach (var operation in operations)
            {
                operation.Apply(handler);
            }
        }

        [Property]
        public void CheckImplementationAgainstModel(List<Operation> operations)
        {
            var real = new ProductionUserHandler();
            var model = new UserHandlerModel();

            ApplyOperations(operations, real);
            ApplyOperations(operations, model);

            model.Validate(real);
        }
    }
}
