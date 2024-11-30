using HackathonContract.Model;
using HackathonDatabase.model;

namespace HackathonDatabase.service;

public interface IHackathonService
{
    int SaveHackathon(double harmonicMean, HackathonMetaInfo hackathonMetaInfo);
    
    HackathonEntity GetHackathonById(int id);

    double CalculateAverageHarmonicMean();
}