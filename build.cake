#tool nuget:?package=NuGet.CommandLine&version=5.9.1
#addin nuget:?package=Cake.Git&version=2.0.0
#addin nuget:?package=Cake.CodeGen.OpenAPI&version=1.0.2
using Cake.CodeGen.OpenApi;
using Cake.Common.Tools.NuGet.NuGetAliases;


var target = Argument("target", "Bundle");
var configuration = Argument("configuration", "Release");
var generator = Argument("generator", "aspnetcore");
var output_dir = Argument("output_dir", $"./build/{generator}");
var packageName = Argument("package_name", "RideSaver.Server");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    CleanDirectory($"{output_dir}");
});

Task("GenerateOpenAPI")
    .IsDependentOn("Clean")
    .Does(() =>
{
    OpenApiGenerator.Generate("openapi.yaml", generator, $"{output_dir}", new OpenApiGenerateSettings()
    {
        ConfigurationFile = "./openapi-codegen.json",
        PackageName = packageName,
        TemplateDirectory = "./templates/csharp"
    });
});

Task("Build")
    .IsDependentOn("GenerateOpenAPI")
    .Does(() =>
{
    DotNetBuild($"{output_dir}/{packageName}.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
        Framework = "net6.0",
        OutputDirectory = $"./build/{generator}/src/{packageName}/bin/{configuration}/lib/net6.0",
        NoDependencies = false,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    // DotNetTest($"{output_dir}/{packageName}", new DotNetTestSettings
    // {
    //     Configuration = configuration,
    //     NoBuild = true,
    // });
});

Task("Bundle")
    .IsDependentOn("Test")
    .Does(() => 
{
    var nuGetPackSettings = new NuGetPackSettings {
        Id = "RideSaver",
        Version = "0.0.0.1",
        Description = "Initial Build of RideSaver API",
        Authors = new[] { "Elias, John"},
        Files = new[] {
            new NuSpecContent { Source = $"./net6.0/{packageName}.dll", Target = "lib/net6.0"},  
        },
        Dependencies = new[] {
            new NuSpecDependency { TargetFramework = "net6.0" },
        },
        BasePath = $"{output_dir}/src/{packageName}/bin/{configuration}/lib",
        OutputDirectory = $"{output_dir}/nuget",
        Repository = new NuGetRepository {
            Type = "Git",
            Branch = GitBranchCurrent(".").CanonicalName,
            Url = GitBranchCurrent(".").Repositories.First().Url
        }
    };

    NuGetPack($"{output_dir}/src/{packageName}/{packageName}.nuspec", nuGetPackSettings);
});


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
