# Taxprep-Add-Ins Documentation

The main goal consists of developing a function bootstrap in the external system to be able to connect an external interface with the Taxprep application. Taxprep will be responsible to load the external Add-in. The end result may look (but is not limited to) to the Microsoft Office Add-In architecture.

Here are some important assumptions for the Add-in engine
- Add-in module can be installed without admin rights (is not located in the application folder, registered for the specific user, etc.)
- Add-in works in the same thread with the host application1
- Add-in is a passive module (all communication is initiated by the host application)
- Add-in has no User Interface2
- Add-ins are loaded on application start up, unloaded on exit (you need to restart the application to add or remove an add-in module to it)

To Intall Taxprep:
- [Taxprep T1 Setup] (static.isolutionslab.com/TaxprepSDK/TXPT1102014.exe) unlock code : 68HG-A4HF-CCD8-B289
- [Taxprep T2 Setup] (static.isolutionslab.com/TaxprepSDK/TXPT2202014.exe) unlock code : B42D-FBH3-8F7G-845D

For any questions, [please click here] (https://github.com/Wolters-Kluwer-Canada/Taxprep-Add-Ins/issues/new)
