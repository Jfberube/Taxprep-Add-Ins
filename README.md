# Taxprep Add-In

A framework to extend the various Taxprep applications. Somehow similar to the Microsoft Office (TM) Add-In architecture.

## To Intall Taxprep T1:
- [Taxprep T1 Setup] (http://static.isolutionslab.com/TaxprepSDK/TXPT1402015.Release.exe) unlock code : ga59-2e95-c882-e84e
- For more info about Taxprep T1 aka Personal Taxprep : [Visit this site] (http://taxprep.com/en/products/t1/index.asp) 

## To install Taxprep T2:
- [Taxprep T2 Setup] (http://static.isolutionslab.com/TaxprepSDK/TXPT2222015.Release.exe) unlock code : d359-egeb-9cf4-365h
- For more info about Taxprep T2 aka Corporate Taxprep : [Visit this site] (http://taxprep.com/en/products/t2/index.asp) 

For any questions, [please click here] (issues/new)


## Taxprep Add-In Documentation:

- [Intruction on how to create your first Taxprep Add-in ](Documentation/How%20to%20create%20a%20new%20add-in.md)

- [Information about the Calc API or how to query mostly eveything about a TaxReturn host by the Taxprep Software ](Documentation/CalcAPI.md)

- [Information on how to implement a custom Drag from the Taxprep Software ](Documentation/DragAndDropAPI.md)

- [Information on how use the Loader.dll to use add-in without the Unmanaged Exports package](Documentation/Loader.md)

Source code:

  - `\Solution\` contains an Hello World Add-in, a Sample Add-in and a Unit Yest Add-in supporting the MSTest framework.
  - `\3Party\Wolters Kluwer\` contains a group of runtime library to run and to register an Add-In into a Taxprep software.
  - `\Documentation\` contains the related documentation for this project


## LICENSE

Copyright 2016 Wolters Kluwer Limited

Licensed under the Apache License, Version 2.0: http://www.apache.org/licenses/LICENSE-2.0