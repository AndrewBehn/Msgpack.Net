@echo off
cls
set arg1=%1
"tools\nuget\nuget.exe" "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion"
"tools\FAKE\tools\Fake.exe" "build.fsx" "version="%arg1%