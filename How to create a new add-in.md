# Overview

As each add-in have to be signed with iSolutions certificate, developers could use the Proxy add-in wrapper. 

## Using proxy add-in to develop the new add-in

The compiled binaries for the proxy add-in is locating in :

`bin\ProxyAddin\1.0.0.0\`

Each developed add-in is assigned to the apropriate TaxPrep Unlock Code and have to be registered with iSolutions.

Repository contains the pre-registered proxy add-ins:

1. For T1 (unlock code is 68HG-A4HF-CCD8-B289):
`<repositorypath>\Addins\bin\ProxyAddin\Registered\T1\68HG-A4HF-CCD8-B289\`
2. For T1 (unlock code is B42D-FBH3-8F7G-845D):
`<repositorypath>\Addins\bin\ProxyAddin\Registered\T2\B42D-FBH3-8F7G-845D\`


### Creating a "hello world" add-in ###


- Create a new class library project in Visual Studio.
- Rename the Class1 to AddinInstance.
- Add "UnmanagedExports" NuGet package (Manage NuGet Packages->Seach for "UnmanagedExports"->Install).
- Open project options and change Platform target from "Any CPU" to "x86".
- Add TaxprepAddinAPI.dll to references (<repopath>\Addins\api\bin\TaxprepAddinAPI.dll)
- Copy sources from <repopath>\Addins\api\lib to your project folder
- In solution explorer click "Show all files".
- Right click on lib folder and press "Include in project".
- Add `using TaxprepAddinAPI;` to the AddinInstance.cs.
- Add `using TxpAddinLibrary;` to the AddinInstance.cs.
- Mark AddinInstance class with attribute [AddinInstance] and derive it from AddinInstanceBase.
- Override ReleaseApp method to handle any clean-up needed when add-in is unloaded from TaxPrep:
    
>     public override void ReleaseApp()
>     {
>         base.ReleaseApp();
>         // some finalization code
>     }


- Add a constructor. The guid, name and version should be the same as defined in ProxyAddin configuration and registered with iSolutions:

>        public AddinInstance()
>            : base(new Guid("26A2ECE8-ED75-47B9-8797-32B3C0D227A8"), 
>            "EmptyAddinExample", "1.0.0.0")
>        { 
>            //some class initizalition code
>        }

- Override an initialization method. This method can create menu items and register all required event handlers:

>        public override void InitializeTaxPrepAddin()
>        {
>            var appMenuService = (IAppMenuService)_appInstance;
>            if (appMenuService != null)
>            {
>                var subMenu = appMenuService.AddRootMenu("Empty add-in");
>                subMenu.Visible = true;
>
>                var item = subMenu.AddItem("Hello world", false);
>                item.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoHelloWorld);
>                item.Visible = true;
>                item.Enabled = true;
>            }
>        }

- Implement the handlers code:

>        private void DoHelloWorld()
>        {
>            var app = (IAppTaxApplicationService)_appInstance;
>            app.ShowMessageString("Test message", "Hello world!");
>        }


**The add-in created by that actions is available in repository: (<repositorypath>\Addins\Examples\EmptyAddin)*

### Registering the add-in with TaxPrep

To register add-in, please run the appropriate AddinReg.bat file. The batch file will:

- Correct the VSProxy.dll.config file with apropriate add-in path.
- Register your add-in in the system registry (`HKCU\Software\CCH\<ProductName>\Addins\<AddinShortName>`).

### List of available interfaces

To see the list of available interfaces, please refer to CalcAPI.md.

### Unmanaged export side effects

Sometimes, when the add-in become bigger, due to unmanaged export the project shows the compile-time errors. You will get this error for sure if add a new Windows form to add-in. As a work-around for C# it's recommended to move all your UI into a separated dll.

### Error handling
All unhandled error in your add-in will be shown as crash report in TaxPrep. 

### Debug instructions.

- Register your add-in with apropriate bach file.
- Go to your Project->Properties->Debug
- Set the TaxPrep executable as a "Start external program" 
- Run the project.

OR

- Register your add-in with apropriate bach file.
- Run TaxPrep from Start menu
- Debug-> Attach to Process -> <TaxPrep application>

