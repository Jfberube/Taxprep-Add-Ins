@echo off
setlocal

set AddinShortName=VSUnitTestAddin
set AddinName=C# Unit Test Add-in
set AddinGuid=EEAC0C48-F6B1-4865-B60A-8304F6F10ABF
set AddinVersion=1.0.0.0
set AddinDLL=%~dp0bin\VSUnitTestAddin\VSUnitTestAddin.dll

set directory=%~dp0
set exe=%directory%\..\..\api\AddinReg.exe
set proxy=%~dp0Proxy\VsProxy.dll
set T1=T1 Taxprep 2014
set T2=T2 Taxprep 2014-2

rem uncomment the lines below for the direct registration
rem "%exe%" "%T1%" -register "%AddinShortName%" "%AddinDLL%"
rem "%exe%" "%T2%" -register "%AddinShortName%" "%AddinDLL%"

rem comment the lines below for the direct
"%exe%" "%T1%" -register -p "%AddinShortName%" "%AddinName%" "%AddinGuid%" "%AddinVersion%" "%AddinDLL%" "%proxy%"
"%exe%" "%T2%" -register -p "%AddinShortName%" "%AddinName%" "%AddinGuid%" "%AddinVersion%" "%AddinDLL%" "%proxy%"