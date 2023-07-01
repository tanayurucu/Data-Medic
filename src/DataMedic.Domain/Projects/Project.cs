using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.Projects.ValueObjects;

namespace DataMedic.Domain.Projects;

public sealed class Project : AggregateRoot<ProjectId>
{
    public string JiraId { get; set; }
    public DepartmentId Department { get; set; }
}