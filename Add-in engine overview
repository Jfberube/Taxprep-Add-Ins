# Add-in Engine Overview

## Requirements

The main goal consists of developing a function bootstrap in the external system to be able to connect an external interface with the Taxprep application. Taxprep will be responsible to load the external Add-in. The end result may look (but is not limited to) to the Microsoft Office Add-In architecture.

The initial SOW can be found here: "MNP - Taxprep Add-in SOW 2014 V2.docx":https://isolutionslab.com/attachments/download/335/MNP%20-%20Taxprep%20Add-in%20SOW%202014%20V2.docx

Here are some important assumptions we rely in the Add-in engine
* Add-in module can be installed without admin rights (is not located in the application folder, registered for the specific user, etc.)
* Add-in works in the same thread with the host application[1]
* Add-in is a passive module (all communication is initiated by the host application)
* Add-in has no User Interface[2]
* Add-ins are loaded on application start up, unloaded on exit (you need to restart the application to add or remove an add-in module to it)

---

fn1. Add-in itself may have any number of threads, but it is the add-in responsibility to synchronize all calls to the host application with the application thread

fn2. Add-in may show a modal dialog while an add-in method is running but the dialog should be closed before the control is returned to the host application. A modeless UI should be implemented in a separate application

## Introduction

This WIKI page is addressed to the host application developers, who needs to maintain and/or extend the Add-in engine within Taxprep applications. 
It could be also useful for the add-in developers to understand the existing add-in architecture. 

To describe the architecture we need to define two basic entities: *_add-in host_* and *_add-in module_*.
* *_Add-in host_* is a module or an application, which defines the add-in interface and manage add-in live cycle (load and unload the add-in modules). In our case, add-in host (or host application) is a Taxprep application.
* *_Add-in module_* is an external library (a DLL file) which is using the provided interface to extend the functionality of the host application. Add-in itself is a standalone module. It may be designed in any language, any development environment. It just need to be compatible with the interface defined by the host application.

*_Add-in engine_* is the part of the host application, which implements all the required functionality to support add-in modules. Actually, you may treat the engine as a broker between the host application and add-in module (see the picture).

!/attachments/download/350/Intoduction.svg(Intoduction.svg)!

As you could see the objects of the host application (main menu, tax return, etc.) have no access to the add-in modules same as the add-in modules has no direct access to the objects of the host application. 
It is the responsibility of the add-in engine to supervise the communication between the host application and add-in module(s).

*NB* Please note that the add-in interface may not repeat the architecture of the host application. Usually it should hide the internals of the host application from the add-in modules. Actually, only a high-level application interface is often provided to the add-in modules. This should allow keeping this interface stable, while allowing the host application developers to modify the internals of the host application according to the recent requirements.

## Add-ins Technology Stack

The technology he have selected to develop Taxprep Add-in modules is very similar to Microsoft’s COM(Component Object Model). Any developer familiar with COM basics should be able to develop an add-in module for Taxprep. 

On disk, an add-in module is a special DLL(Dynamic Linked Library) file which use interfaces to communicate with the host application.

Although we do have TLB file, we do inherit all add-in interfaces from IUnknown, we do use the same call conventions together with standard COM types for methods parameters it is still not right to assume that we are using COM. TLB file is used just as an automatic tool to generate the interface definitions in different languages. The rest ones actually the only thing things we are using from COM.

In other words we do have something common with COM, but COM has much bigger functionality which is not used in our add-in engine (we do not use class factories and COM registration, add-in engine is not an in- or out-of-process server, etc.)

As we are not using COM factories and we are not implementing a COM server inside Taxprep applications, same we are not expecting it from add-in modules. Thus, both host application and add-in modules are not using COM to retrieve interfaces of each other themselves. Actually, it means that we need to make a handshake between the host application and add-in module manually. 

The handshake is implemented by a special *_[[AddinEngine#Add-in-Initialization|GetAddinInstance]]_* function, which should be exported by the DLL file (see [[AddinEngine#Add-in-Initialization|Add-in Initialization]] for details).

# Basic Concepts

This topic will give you the key principles of the add-in engine. It will describe the general overview of the engine classes and it functionality.

Let's say we have a host application (Taxprep) with its own functionality and we need to extend it. By extending the functionality we would assume:
* we need to provide a service to access some objects of the host application
* we need the add-in modules to be able to handle some events of the host application.

Let's say we need to create a menu item by the add-in. Normally, the menu item is owned by the Main form of the host application. The item should keep the reference to the add-in handler as it should notify the add-in when it pressed. The add-in should keep the reference to the menu item too as it needs to update item's Caption, Visible and Enable states.

As the result we have introduced two engine entities: *_AppService_* and *_AddinHandler_*.

* *_AppService_* is provides the functionality of the host application to the add-in modules.

* *_AddinHandler_* is an add-in callback which is called by the host application each time the corresponding event occurs.

We may treat services and handlers as the links between the host application and the add-in modules. Add-in keeps the references to the application services while application keeps the references to the add-in handlers.

The add-in engine is designed to keep both these references in the valid state. In other words the engine will automatically detect when the referenced object is released and clear the corresponding reference. If the host application or an add-in will try to access the released object an appropriate exception will be raised(thrown).

To be more exact add-ins are not referring application services directly. The engine has a special internal *_AppServiceAdapter_* instance in the middle. Adapter classes are actually implements the required interfaces. They are also used to isolate the live time of the service objects and add-in reverences to it. Actually add-in may keep the reference even after the service has been released. It will get a proper exception if it tries to access the released service. 
In other words all actual links between the add-in and host application are fully managed by the add-in engine. Thus the engine may release all links to the add-in handles when required same as it may detect memory leaks (all the services referenced by a specific add-in).

# Generating API Interfaces Definitions

As it was mentioned above, the definitions for add-in interfaces are automatically generated from a TLB file. Actually, we have created a separate Delphi 7 project (see EP\Tools\TaxprepAddinAPI\) to edit the TLB file. When we edit the TLB file within this project Delphi IDE automatically updates the corresponding source file (TaxprepAddinAPI_TLB.pas) which is used in add-in engine inside Taxprep Projects. 

We also have created a batch file near the TLB file (GenCSharpDll.bat) which will create an interop DLL (EP\Tools\TaxprepAddinAPI\bin\TaxprepAddinAPI.dll) for C# projects. Currently the interop DLL should be referenced by a C# add-in project. Please note this DLL is required for compile time only, it should not be deployed either with the host application or with add-in module. If we will find a tool, which will be able to create a C# file with proper interface definitions similar to TaxprepAddonAPI_TLB.pas we will definitely remove the batch file together with the interop DLL.

# Add-in Engine Classes

## EcchAddinException

EcchAddinException is a base class for all add-in engine exceptions.

      EcchAddinException = class(Exception)
    `</pre>

    ## TCustomAddinEngine 

    TCustomAddinEngine is a main class for the add-in engine. It loads and keeps the list of all available add-ins.

    <pre>`
      TCustomAddinEngine = class(TObject)
    `</pre>

    ## TCustomAddin

    TCustomAddin encapsulates a properties of a single add-in module. Add-in engine keeps a separate instance of TCustomAddin class for each loaded add-in module.

    <pre>`
      TCustomAddin = class(TObject)
    `</pre>

    ## TCustomAppService

    TCustomAppService is a base class for all add-in services. TCustomAppService descendants provide the functionality of the host application to the add-in modules.

    There are two main service classes
    * TCustomAppGlobalService provides the functionality to the global application objects like Main Menu, Tax Application, Client File Manager, etc. 
    * TCustomAppLocalService provides the functionality to the local (add-in specific) application objects like Menu Item, Tax Cell, Client File, etc. 

    The main difference between the global and local services is its owner. All global services are owned by the add-in engine,

    !/attachments/download/347/EngineOverview.svg(EngineOverview.svg)!

    while all local services are usually owned by a specific add-in or one of it's own objects. Local services could be also used as a temporary objects to provide proper parameters to the add-in methods. Such services will be released after the add-in method is finished and a proper exception will be generated if the add-in will keep the reference to the parameters and try to access them later.

    <pre>`
      TCustomAppService = class(TObject)
      TCustomAppGlobalService = class(TCustomAppService)
      TCustomAppLocalService = class(TCustomAppService)
    `</pre>

    ## TCustomAppLocalServiceList

    TCustomAppLocalServiceList is a base class to keep the list of the local services. TCustomAppLocalServiceList has two descendants:

    * TAppAddinLocalServiceList is used to keep the local services inside add-in instance
    * TAppChildLocalServiceList is used to keep a child local services inside another local service

    <pre>`
      TCustomAppLocalServiceList = class(TObject)
      TAppAddinLocalServiceList = class(TCustomAppLocalServiceList)
      TAppChildLocalServiceList = class(TCustomAppLocalServiceList)
    `</pre>

    ## TAppLocalServiceRef

    TAppLocalServiceRef is used to keep a reference to a local service in other objects. This class may be used if you need to keep a reference to a local service outside of the add-in engine (for example, in a menu item). Actually TAppLocalServiceRef class is used to automatically update (clear) the the reference to a local service before the service is destroyed.

    <pre>`
      TAppLocalServiceRef = class(TObject)
    `</pre>

    ## TCustomAppAdapter 

    TCustomAppAdapter is a base class for all application adapters. TCustomAppAdapter descendants implements' all Taxprep interfaces provided to add-in modules.

    <pre>`
      TCustomAppAdapter = class(TInterfacedObject, IInternalGetService, ISupportErrorInfo)
      TCustomAddinServiceAdapter = class(TCustomAppAdapter)
    `</pre>

    ## TCustomAppInstanceAdapter

    TCustomAppInstanceAdapter implements the main application instance interface (a main entry point for the add-in modules).
    Add-in may cast this interface to any specific global service.

    <pre>`
      TCustomAppInstanceAdapter = class(TCustomAppAdapter, IUnknown)
    `</pre>

    ## TCustomAddinServiceAdapter

    TCustomAddinServiceAdapter is a base class for all service adapters.
    TCustomAddinServiceAdapter is an internal object, the descendant of this class should be hidden from the host application (declared in the implementation part of the unit with corresponding service).

    <pre>`
      TCustomAddinServiceAdapter = class(TCustomAppAdapter)
    `</pre>

    ## TCustomAddinHandler

    TCustomAddinHandler is a base class to keep a handler (callback) interface provided by the add-in.
    TCustomAddinHandler is an internal object, owned by the engine. The descendant of this class should be hidden from the host application (declared in the implementation part of the unit)

    <pre>`
      TCustomAddinHandler = class(TObject)
    `</pre>

    ## TAddinHandlerRef

    TAddinHandlerRef is used to keep a reference to a single handler instance inside the engine.
    Usually TAddinHandlerRef class is used inside a local service created by the add-in which should provide a add-in callback top the host application.

    <pre>`
      TAddinHandlerRef = class(TObject)
    `</pre>

    ## TCustomAddinHandlers

    TCustomAddinHandlers is a base class to keep the list of the add-in handlers. Same as TCustomAddinHandler this class should be also hidden from the host application. 
    Usually TCustomAddinHandlers are used within a global service (it requires to keep a separate handler per each add-in).

    <pre>`
      TCustomAddinHandlers = class(TObject)
    `</pre>

    # Add-in Initialization / Finalization

    ## Overview

    Add-in engine is developed to load any number of add-in modules. The list of add-in modules is loaded from the registry. Each add-in module should have its own registry key with a string *_FileName_* value which should specify a full file name of the add-in module.

    For example, an add-in module for T1 application (T1Txp2014.exe) should create a _FileName_ value in
    <pre>HKEY_CURRENT_USER\Software\CCH\T1 Taxprep 2014\Addins\AddinName</pre> where
    * _"HKEY_CURRENT_USER\Software\CCH\T1 Taxprep 2014\"_ is the root application key (this name is defined in the application resources)
    * _"Addins"_ is a root add-ins key
    * _"AddinName"_ is a name of the add-in registry key. It should be a unique add-in identifier. For example you may generate a separate GUID for it.

    Add-in engine is trying to load all add-in modules separately. If an exception occur while loading one add-in module user will see the Crash Report dialog and the engine will continue to load all the rest add-in modules.

    ## Initialization

    To perform the handshake between the host application and add-in module the module (a DLL libray) should export a special (“GetAddinInstance”) function. While loading the add-in library the host application checks that the dll library exports the function and call it to initialize the add-in module.

    The function should have next signature 
    <pre>`
    function GetAddinInstance(const AAppInstance: IAppInstance; out AAddinInstance: IAddinInstance): HResult; stdcall;
    `</pre>

    C# equivalent should look like 
    <pre>`[DllExport("GetAddinInstance", CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static int GetAddinInstance([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(AddinMarshaler))]IAppInstance appInstance, out IAddinInstance addinInstance)
    `</pre>

    *NB* Visual Studio treat the interfaces provided by the add-in engine as COM objects by default so it may show some "DisconnectedContext" messages during add-in debugging. To avoid these messages a custom marshaler should be used for for the initialization function in .NET.

    If the function succeeded it should return S_OK result and an interface to initialized add-in instance in the AAddinInstance parameter.
    If the function fails it should return a corresponding HResult. To provide the proper error message to the host application the function should initialize the "error information object for the current thread":http://msdn.microsoft.com/en-us/library/aa910593.aspx.

    ## Finalization

    Add-in engine calls *_ReleaseApp_* and *_ReleaseAddinInstance_* procedures before add-in is unloaded. 

    _ReleaseApp_ is a method of the add-in instance. It allows the add-in to to release all the references to the host application interfaces. In most cases you may leave this method empty as the all the references are kept inside the add-in instance so they will be released automatically after the add-in instance is released.

    _ReleaseAddinInstance_ is an optional function which may be exported by the add-in DLL. Add-in engine calls it after the reference to the add-in instance has been released.  In most cases the function should be used to wait for the garbage collector to actually release all objects.

    This function should have next signature

    *Delphi*
    <pre>`
    function ReleaseAddinInstance: HResult; stdcall;
    `</pre>

    *C#*
    <pre>`
    [DllExport("ReleaseAddinInstance", CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static int ReleaseAddinInstance()
    `</pre>

    *NB* Ad-in engine will call the add-in finalization procedures even if the add-in was not initialized successfully

    # Implementing a .NET Add-in

    ## Overview

    To create a new add-in module you should
    * Create a new Class Library project
    * Add UnmanagedExports package
    * *[[CopyTaxprepAddinAPI|Copy]]* and add a reference to TaxprepAddinAPI library
    * Implement AddinInstance class
    * Implement Initialization/Finalization procedures
    * Implement Assembly Resolve fix (optional)

    *NB* You may find a sample add-in implementation in the VSAddinTest\VSEmptyTest folder of the Taxprep repository.

    ## Create a new Class Library project

    Start Visual Studio and create a new Class Library project

    ## Add UnmanagedExports package

    A C# project cannot export an unmanaged DLL function by default. To achieve the required behavior we are using a "UnmanagedExports package":https://www.nuget.org/packages/UnmanagedExports/1.2.6 to the project 

    To install the package you may press "Manage NuGet packages" command from the project context menu,enter "UnmanagedExports" in the "Search online" editor and press Install button near the required package.

    ## Add a reference to TaxprepAddinAPI library

    Please, run the *Tools\TaxprepAddinAPI\GenCSharpDll.bat* to generate the library and add the library reference it to the project.

    *NB* the library will be generated in the *bin* folder (Tools\TaxprepAddinAPI\bin\TaxprepAddinAPI.dll)

    ## Implement AddinInstance class

    Add a new AddinInstance class to the project. 
    The class should implement the IAddinInstance interface.

    For example you may use next code
    <pre>`
    private static readonly Guid AddinKey = new Guid(<Generate a new GUID here>);

    public Guid Key { get { return AddinKey; } }
    public string Name { get { return <Put add-in name here>; } }
    public string Version { get { return "1.0.0.0"; } }

    public void ReleaseApp()
    {
        // do nothing
    }
    `</pre>

    *NB* Please do not forget to generate a new GUID for add-in key

    ## Implement Initialization/Finalization procedures

    Implement a custom marshaler to avoid "DisconnectedContext" messages during add-in debugging
    <pre>`
    public class AddinMarshaler : ICustomMarshaler
    {
        private static readonly AddinMarshaler Instance;

        static AddinMarshaler()
        {
            Instance = new AddinMarshaler();
        }

        // return the static instance
        public static ICustomMarshaler GetInstance(string cookie)
        {
            return Instance;
        }

        public void CleanUpManagedData(object managedObj)
        {
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
        }

        public int GetNativeDataSize()
        {
            return 4;
        }

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            throw new NotImplementedException();
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return Marshal.GetObjectForIUnknown(pNativeData);
        }
    }
    `</pre>

    Now you may implement the add-in initialization procedure
    <pre>`
    [DllExport("GetAddinInstance", CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static int GetAddinInstance([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(AddinMarshaler))]IAppInstance appInstance, out IAddinInstance addinInstance)
    {
        try
        {
            addinInstance = new AddinInstance(appInstance);

            return 0;
        }
        catch (Exception error)
        {
            addinInstance = null;
            return Marshal.GetHRForException(error);
        }
    }
    `</pre>

    .NET does not release the interfaces immediately so the finalization procedure is also required
    <pre>`
    [DllExport("ReleaseAddinInstance", CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static int ReleaseAddinInstance()
    {
        try
        {
            // !!! make sure all interfaces are released by garbage collector
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return 0;
        }
        catch (Exception error)
        {
            return Marshal.GetHRForException(error);
        }
    }
    `</pre>

    ## Implement Assembly Resolve fix (optional)

    In case your add-in module depends on other assemblies you may need to implement the assembly resolve fix.
    The below procedure allows to find .NET assemblies located in the same folder with the add-in module.
    You may need to modify this procedure according to your needs.

    <pre>`
    private static Assembly DoAssemblyResolve(Object sender, ResolveEventArgs e)
    {
        var lSearchPath = System.IO.Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        if (lSearchPath == null)
            return null;

        var lFullFileName = System.IO.Path.Combine(System.IO.Path.GetFullPath(lSearchPath), new AssemblyName(e.Name).Name) + ".dll";
        if (!System.IO.File.Exists(lFullFileName))
            return null;

        var lAssemblyFullName = AssemblyName.GetAssemblyName(lFullFileName).FullName;

        if (String.Compare(lAssemblyFullName, e.Name, StringComparison.CurrentCultureIgnoreCase) != 0)
            return null;

        return Assembly.LoadFrom(lFullFileName);
    }
    `</pre>

    And do not forget to add next line to the initialization procedure
    <pre>`
    AppDomain.CurrentDomain.AssemblyResolve += DoAssemblyResolve;

# Exception handling

Add-in engine assumes that both add-in and host application interfaces are implemented using the standard COM call convention which allows to "transfer" the exceptions between the modules. Actually it means that all exceptions are properly handled within interface methods, the proper HResult code is returned and a corresponding error information object is initialized for the current thread. Such behavior allows the caller to detect and raise/throw the proper exception to the upper level.

The required behavior is automatically implemented by both Delphi and .NET environments so no extra code is required.

*NB* Delphi may not generate the proper error message if we try to access the field of an unassigned(nil) object. Add-in engine do perform all required checks to avoid this but if the issue will occur inside the existing Taxprep code you may still get a wrong error message "Catastrophic failure (Exception from HRESULT: 0x8000FFFF (E_UNEXPECTED))"

# .NET DLL loading

Add-in modules cannot be placed near the Taxprep application (as it will require admin rights for this) so you may reach some "DLL HELL" issues.

Please, keep in mind that it is an add-in responsibility to load all its assemblies(modules) correctly.

If you have implemented an add-in in .NET you should be aware of:
* .NET will use the Taxprep application folder as the base path (it may cause some "DLL HELL" issues if you do not have a stand alone DLL file)
* .NET add-in modules may affect assembly resolving logic of other add-ins as they may modify global settings inside .NET core

To avoid such issues it is highly recommended to
* use stand alone add-in modules
* load all the required assemblies manually using the full path
* use strong library names

Please see the next link for more details "Deploying Assemblies":http://msdn.microsoft.com/en-us/magazine/cc164080.aspx
