using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactRules.Core.Entities;
using TransactRules.Core.Mock;

namespace TransactRules.Dox.Test
{
     [TestClass]
    public class MockTests
    {
        public class Party : Entity 
        {
            public virtual string Name { get; set; }     
        }

        public class Person : Party {
            public virtual DateTime DateOfBirth { get; set; }
        }

        public class Company : Party {
            public virtual string LegalName { get; set; }
        }

        [TestMethod]
        public void TestMockUnitOfWork()
        {
            var unitOfWork = new MockUnitOfWork();
            var newPerson = new Person { Name = "Joe", DateOfBirth = new DateTime(1972, 6, 8) };

            unitOfWork.Create(newPerson);

            Assert.AreEqual(newPerson.Id, 1);

            var newCompany = new Company { Name = "ABC Bank", LegalName = "ABC Bank Pty. Ltd." };

            unitOfWork.Create(newCompany);

            Assert.AreEqual(newCompany.Id, 2);

            var updatedCompany = new Company { Id = 2, Name = "ABC Bank 2.0", LegalName = "ABC Bank Pty. Ltd." };

            unitOfWork.Update(updatedCompany);

            var refCompany = (Company)  unitOfWork.GetById(typeof(Company), 2);

            Assert.AreEqual("ABC Bank 2.0", refCompany.Name);

            var anotherPerson = new Person { Name = "John", DateOfBirth = new DateTime(1954, 1, 1) };

            unitOfWork.Create(anotherPerson);

            var q1 =  (IQuery<Party>) unitOfWork.CreateQuery(typeof(Party));

            Assert.AreEqual(2, q1.Items.Where(p => p.Name.StartsWith("Jo")).Count());

            var q2 = (IQuery<Person>)unitOfWork.CreateQuery(typeof(Person));

            Assert.AreEqual(1, q2.Items.Where(p => p.DateOfBirth > new DateTime(1970,1,1)).Count());
        
        }
        
    }
}
