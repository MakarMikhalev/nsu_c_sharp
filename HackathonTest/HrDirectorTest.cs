using HackathonContract.Model;
using HackathonHrDirector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace HackathonTest;

[TestClass]
public class HrDirectorTest
{
    [Test, TestCaseSource(nameof(CalculateMeanHarmonicTestCases))]
    public void Success_CalculateHarmonicMean_ReturnsCorrectResult(
        List<Wishlist> wishlists1,
        List<Wishlist> wishlists2,
        List<Team> teams,
        double expected)
    {
        var hackathonMetaInfo = new HackathonMetaInfo(wishlists1, wishlists2, teams);

        var hrDirector = new HrDirector();

        var result = hrDirector.CalculateMeanHarmonic(hackathonMetaInfo);
        Assert.AreEqual(expected, result);
    }

    [Test]
    [TestCase(3, new[] { 1, 2, 3 }, 1)]
    [TestCase(5, new[] { 1, 1, 1, 1, 5 }, 1)]
    public void CalculateSatisfactionScoreTest(int employeeId, int[] ratings, int expected)
    {
        var hrDirector = new HrDirector();
        var result = hrDirector.CalculateSatisfactionScore(employeeId, ratings);
        Assert.AreEqual(expected, result);
    }
    
    [Test, TestCaseSource(nameof(CalculateHarmonicTestCases))]
    public void CalculateHarmonicTest(List<double> values, double expectedResult)
    {
        var hrDirector = new HrDirector();
        var result = hrDirector.CalculateHarmonic(values);
        Assert.AreEqual(expectedResult, result);
    }
    
    private static IEnumerable<TestCaseData> CalculateMeanHarmonicTestCases()
    {
        yield return new TestCaseData(
            new List<Wishlist>
            {
                new(1, new[] { 1, 2 }),
                new(2, new[] { 2, 1 })
            },
            new List<Wishlist>
            {
                new(1, new[] { 1, 2 }),
                new(2, new[] { 2, 1 })
            },
            ModelFactory.GenerateTeams(2),
            2.0
        );

        yield return new TestCaseData(
            ModelFactory.Wishlists,
            ModelFactory.Wishlists,
            ModelFactory.GenerateTeams(3),
            2.25
        );
    }
    
    private static readonly TestCaseData[] CalculateHarmonicTestCases =
    {
        new(new List<double> { 2, 6 }, 3),
        new(new List<double> { 1, 4, 4 }, 2),
        new(new List<double> { 1, 2, 4, 4}, 2)
    };
}