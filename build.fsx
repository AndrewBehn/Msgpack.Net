#r @"tools/FAKE/tools/FakeLib.dll"
open Fake
open Fake.DotNetCli
open Fake.NuGetHelper

let projects = (@".\src\Msgpack.NetCore", @".\src\UnitTests");
let msgpackProj, unitTestProj = projects;
let version = getBuildParamOrDefault "version" "0.1.0-dev"

Target "SetVersion" (fun _ -> SetVersionInProjectJson version (msgpackProj + "\project.json"))

Target "Restore" (fun _ ->
    Restore (fun p -> p))

Target "Test" (fun _ -> Test (fun p -> { p with Project = unitTestProj;
                                                Configuration = "Debug"}))

Target "Pack" (fun _ ->
    Pack (fun p -> { p with Project = msgpackProj;
                            Configuration = "Release";
                            }))

Target "Default" (fun _ ->
    trace "Building Msgpack.NetCore")

"SetVersion" ==> "Restore" ==> "Test" ==> "Pack" ==> "Default"

RunTargetOrDefault "Default"