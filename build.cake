#addin nuget:?package=Cake.CodeGen.OpenAPI&version=1.0.2
using Cake.CodeGen.OpenApi;


var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    CleanDirectory($"./src/RideSaver.Server/bin/{configuration}");
    CleanDirectory($"./src");
});

Task("GenerateOpenAPI")
    .IsDependentOn("Clean")
    .Does(() =>
{
    OpenApiGenerator.Generate("openapi.yaml", "aspnetcore", "./src", new OpenApiGenerateSettings()
    {
        ConfigurationFile = "./openapi-codegen.json"
    });
});

Task("Build")
    .IsDependentOn("GenerateOpenAPI")
    .Does(() =>
{
    DotNetBuild("./src/RideSaver.Server.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

// Task("Test")
//     .IsDependentOn("Build")
//     .Does(() =>
// {
//     DotNetTest("./src/RideSaver.Server", new DotNetTestSettings
//     {
//         Configuration = configuration,
//         NoBuild = true,
//     });
// });

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
