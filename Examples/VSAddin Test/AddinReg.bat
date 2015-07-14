@echo off
setlocal

set AddinShortName=VSAddinTest
set AddinName=C# Addin Test
set AddinGuid=8C6E1349-8287-42C7-83ED-6AAE51DE11A5
set AddinVersion=1.0.0.0
set AddinDLL=%~dp0bin\VSAddinTest\VSAddinTest.dll

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