#addin nuget:?package=Cake.CodeGen.OpenAPI&version=1.0.2
using Cake.CodeGen.OpenApi;


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
        PackageName = packageName
    });
});

Task("Build")
    .IsDependentOn("GenerateOpenAPI")
    .Does(() =>
{
    DotNetBuild($"{output_dir}/{packageName}.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
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
    NuGet.Pack($"{output_dir}/src/{packageName}/{packageName}.nuspec", new NuGetPackSettings{
        Files = new [] {
            new NuSpecContent {Source = $"{ouput_dir}/{packageName}/bin/{configuration}/net6.0/TestNuGet.dll", Target = "bin"},
        },
    });
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
