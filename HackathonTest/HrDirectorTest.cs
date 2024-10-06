using HackathonContract.Model;
using HackathonHrDirector;
using HackathonHrManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace HackathonTest;

[TestClass]
public class HrDirectorTest
{
    [Test]
    public void CalculateMeanHarmonicTest()
    {
        var hackathonMetaInfo = new HackathonMetaInfo(
            new List<Wishlist>
            {
                new(1, [1, 2, 3]),
                new(2, [3, 1, 2]),
                new(3, [3, 2, 1])
            },
            new List<Wishlist>
            {
                new(1, [1, 2, 3]),
                new(2, [3, 1, 2]),
                new(3, [3, 2, 1])
            },
            new List<Team>
            {
                new(new Employee(1, "TeamLead-1"), new Employee(1, "Junior1-")),
                new(new Employee(2, "TeamLead-2"), new Employee(2, "Junior-2")),
                new(new Employee(3, "TeamLead-3"), new Employee(3, "Junior-3"))
            }
        );

        var hrDirector = new HrDirector();

        var result = hrDirector.CalculateMeanHarmonic(hackathonMetaInfo);
        var correctResult = 1.7999999999999998;
        Assert.AreEqual(correctResult, result);
    }

    [Test]
    public void CalculateSatisfactionScoreTest()
    {
        var hrDirector = new HrDirector();
        var result = hrDirector.CalculateSatisfactionScore(3, [1, 2, 3]);
        Assert.AreEqual(1, result);
    }
    
    [Test]
    public void CalculateHarmonicTest()
    {
        var hrDirector = new HrDirector();
        var result = hrDirector.CalculateHarmonic([2, 6]);
        Assert.AreEqual(3, result);
    }
}