using HackathonRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using CollectionAssert = Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert;

namespace HackathonTest;

[TestClass]
public class HackathonTest
{
    
    [Test]
    public void CreateWishlistTest()
    {
        Hackathon hackathon = new Hackathon();
        var jEnumerable = GeneratorEmployer.GenerateJuniors();
        var sEnumerator = GeneratorEmployer.GenerateSeniors();
        
        var wishlist = hackathon.Start(jEnumerable,sEnumerator);
        Assert.AreEqual(3, wishlist.TeamLeadsWishlists.Count());
        Assert.AreEqual(3, wishlist.JuniorsWishlists.Count());
        
        var teamLeadsWishlists = wishlist.TeamLeadsWishlists.Select(w => w.EmployeeId);
        var teamLeads = sEnumerator.Select(e => e.Id);
        Assert.IsTrue(teamLeadsWishlists.SequenceEqual(teamLeads));
        
        var juniorsWishlists = wishlist.JuniorsWishlists.Select(w => w.EmployeeId);
        var juniors = jEnumerable.Select(e => e.Id);
        Assert.IsTrue(juniorsWishlists.SequenceEqual(juniors));
    }
    
    [Test]
    public void CreateWishlistDesiredEmployeesTest()
    {
        var hackathon = new Hackathon();
        var jEnumerable = GeneratorEmployer.GenerateJuniors();
        var sEnumerator = GeneratorEmployer.GenerateSeniors();
    
        var wishlist = hackathon.Start(jEnumerable, sEnumerator);

        foreach (var juniorWishlist in wishlist.JuniorsWishlists)
        {
            var desiredEmployees = juniorWishlist.DesiredEmployees;
            var teamLeadsIds = sEnumerator
                .Select(e => e.Id)
                .Distinct()
                .ToArray(); 

            Assert.AreEqual(teamLeadsIds.Length, 
                desiredEmployees.Length, 
                "Количество team lead-ов должно соответсвовать количетсву juniors");
            CollectionAssert.AreEquivalent(teamLeadsIds, 
                desiredEmployees, 
                "Все участники должны присутсвтовать в списке juniors");
        }
    }
}