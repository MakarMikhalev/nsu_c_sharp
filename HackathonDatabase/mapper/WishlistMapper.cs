using HackathonDatabase.model;

namespace HackathonDatabase.mapper;

public static class WishlistMapper
{
    public static WishlistEntity Entity(EmployeeEntity employee,
        List<EmployeeEntity> desiredEmployees,
        HackathonEntity hackathonEntity)
    {
        return new WishlistEntity
        {
            // Employee = employee,
            // DesiredEmployees = desiredEmployees,
            // Hackathon = hackathonEntity
        };
    }
}