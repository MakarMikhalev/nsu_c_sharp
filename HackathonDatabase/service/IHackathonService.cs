using HackathonContract.Model;

namespace HackathonDatabase.service;

public interface IHackathonService
{
    void SaveHackathon(double harmonicMean, HackathonMetaInfo hackathonMetaInfo);
}