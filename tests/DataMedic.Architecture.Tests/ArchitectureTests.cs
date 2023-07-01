using DataMedic.Application;
using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Domain;
using DataMedic.Infrastructure;
using DataMedic.Persistence;
using DataMedic.Presentation;

using FluentAssertions;

using NetArchTest.Rules;

namespace DataMedic.Architecture.Tests;

public class ArchitectureTests
{
    private const string ApplicationNamespace = "DataMedic.Application";
    private const string DomainNamespace = "DataMedic.Domain";
    private const string PersistenceNamespace = "DataMedic.Persistence";
    private const string InfrastructureNamespace = "DataMedic.Infrastructure";
    private const string PresentationNamespace = "DataMedic.Presentation";
    private const string WebNamespace = "DataMedic.Web";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = DomainAssembly.Assembly;
        var otherProjects = new[]
        {
            ApplicationNamespace,
            PersistenceNamespace,
            InfrastructureNamespace,
            PresentationNamespace,
            WebNamespace
        };

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = ApplicationAssembly.Assembly;
        var otherProjects = new[]
        {
            PersistenceNamespace,
            InfrastructureNamespace,
            PresentationNamespace,
            WebNamespace
        };

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = InfrastructureAssembly.Assembly;
        var otherProjects = new[] { PersistenceNamespace, PresentationNamespace, WebNamespace };

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Persistence_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = PersistenceAssembly.Assembly;
        var otherProjects = new[] { InfrastructureNamespace, PresentationNamespace, WebNamespace };

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Presentation_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = PresentationAssembly.Assembly;
        var otherProjects = new[] { PersistenceNamespace, InfrastructureNamespace, WebNamespace };

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handlers_Should_Have_DependencyOnDomain()
    {
        // Arrange
        var assembly = ApplicationAssembly.Assembly;
        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_Should_HaveDependencyOnMediatR()
    {
        // Arrange
        var assembly = PresentationAssembly.Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .And()
            .DoNotHaveNameStartingWith("Errors")
            .And()
            .DoNotHaveNameStartingWith("Api")
            .Should()
            .HaveDependencyOn("MediatR")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void QueryHandlers_Should_Not_InjectUnitOfWork()
    {
        // Arrange
        var assembly = ApplicationAssembly.Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("QueryHandler")
            .ShouldNot()
            .HaveDependencyOn(typeof(IUnitOfWork).AssemblyQualifiedName)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
}