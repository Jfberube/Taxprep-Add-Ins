@echo off
setlocal

set AddinShortName=EmptyAddin
set AddinName=Empty hello world add-in example via ProxyAddin
set AddinGuid=26A2ECE8-ED75-47B9-8797-32B3C0D227A8
set AddinVersion=1.0.0.0
set AddinDLL=%~dp0bin\Debug\EmptyAddin.dll

set directory=%~dp0
set exe=%directory%\..\..\api\AddinReg.exe
set proxy=%~dp0Proxy\VsProxy.dll
set T1=T1 Taxprep 2014
set T2=T2 Taxprep 2014-2

rem uncommment the line below for the direct registration
rem "%exe%" "%T1%" -register "%AddinShortName%" "%AddinDLL%"
rem "%exe%" "%T2%" -register "%AddinShortName%" "%AddinDLL%"

rem comment the line below for the direct registration
"%exe%" "%T1%" -register -p "%AddinShortName%" "%AddinName%" "%AddinGuid%" "%AddinVersion%" "%AddinDLL%" "%proxy%"
"%exe%" "%T2%" -register -p "%AddinShortName%" "%AddinName%" "%AddinGuid%" "%AddinVersion%" "%AddinDLL%" "%proxy%"