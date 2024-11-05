namespace HackathonContract.Model;

public record HackathonResult(
    IEnumerable<Employee> JuniorEmployees,
    IEnumerable<Employee> TeamLeadEmployees,
    HackathonMetaInfo HackathonMetaInfo
);