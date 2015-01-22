# Taxprep Add-In

A framework to extend the various Taxprep applications. Somehow similar to the Microsoft Office (TM) Add-In architecture.

###To Intall Taxprep T1:
- [Taxprep T1 Setup] (http://static.isolutionslab.com/TaxprepSDK/TXPT1102014.exe) unlock code : 68HG-A4HF-CCD8-B289
- For more info about Taxprep T1 aka Personal Taxprep : [Visit this site] (http://taxprep.com/en/products/t1/index.asp) 

###To install Taxprep T2:
- [Taxprep T2 Setup] (http://static.isolutionslab.com/TaxprepSDK/TXPT2202014.exe) unlock code : B42D-FBH3-8F7G-845D
- For more info about Taxprep T2 aka Corporate Taxprep : [Visit this site] (http://taxprep.com/en/products/t2/index.asp) 

For any questions, [please click here] (https://github.com/Wolters-Kluwer-Canada/Taxprep-Add-Ins/issues/new)

Taxprep Add-In Documentations:

- [Intruction on how to create your first Taxprep Add-in ](https://github.com/Wolters-Kluwer-Canada/Taxprep-Add-Ins/blob/master/How%20to%20create%20a%20new%20add-in.md)

- [Information about the Calc API or how to query mostly eveything about a TaxReturn host by the Taxprep Software ](https://github.com/Wolters-Kluwer-Canada/Taxprep-Add-Ins/blob/master/CalcAPI.md)

- [Information on how to implement a custom Drag from the Taxprep Software ](https://github.com/Wolters-Kluwer-Canada/Taxprep-Add-Ins/blob/master/DragAndDropAPI.md)

Source code:

  - \Examples\ contains a blank Add-in, a sample test Add-in and a unit test Add-in supporting the MSTest framework
  - \Tools\ contains the source code to facilitate the regisration of an Add-in
  - \API\ contain the wrapper arround the COM compatible interface privide by Taxprep and used by the various c# samples application. An Add-in could also be implemented by using an unmanage pprogramming language (e.g.: C++, Delphi).
  - \bin\ contains the Add-in stub to allow to load of custom Add-in into the Taxprep software.


LICENSE
------------
Copyright 2015 Wolters Kluwer Limited<br/>

Licensed under the Apache License, Version 2.0: http://www.apache.org/licenses/LICENSE-2.0
