# VisiWin.Toolkit

**VisiWin.Toolkit** is an open-source toolkit for **VisiWin (WPF/Modern UI) projects (Inosoft)**.  
It provides reusable components, utilities, and patterns to improve structure, maintainability, and type safety in HMI applications.

> ⚡ The toolkit is continuously evolving and will be extended with additional modules over time.

---

## 📦 Projects

### 🔹  VisiWin.Toolkit.Base

**VisiWin.Toolkit.Base** provides foundational building blocks for the VisiWin.Toolkit ecosystem.

It is a **.NET Standard 2.0** library containing common utilities, helper classes, and extension methods that are independent of VisiWin and WPF.

> 🎯 Goal: Provide a clean, reusable, and testable foundation shared across all toolkit modules.

#### ✨ Features

- ✅ Common helper utilities  
- ✅ Extension methods for everyday use  
- ✅ No dependency on VisiWin or WPF  
- ✅ Fully testable and reusable  
- ✅ Compatible with multiple .NET platforms  

#### 📦 Target Framework

```text
.NET Standard 2.0
````
---
### 🔹 ModernUI(WPF) Helper (MarkupExtensions, Converter...)

Coming soon...

---
### 🔹 Controls

Coming soon...

---
### 🔹 MEF Microsoft.DependencyInjection Adapter

Coming soon...

---
### 🔹 Infrastructure

Base classes for adapters and services that want to implement asynchronous logic in the adapter and service hooks (e.g. OnViewAttached, OnViewDetached, etc.).

```csharp
[ExportAdapter(nameof(MainViewAdapter))]
public class MainViewAdapter : AsyncAdapterBase
{
     protected override async Task OnViewAttachedAsync(IView view)
     {
         await base.OnViewAttachedAsync(view);
         await Task.Delay(1000); //your async code
     }
}

[ExportService(typeof(ISettingsService))]
[Export(typeof(ISettingsService))]
public class SettingsService : AsyncServiceBase, ISettingsService
{
    protected override async Task OnLoadProjectCompletedAsync()
    {
        await base.OnLoadProjectCompletedAsync();
        await Task.Delay(1000); //your async code
    }
}
```

---
### 🔹 PlcSymbol

Provides a **type-safe way to define and use PLC symbols** instead of relying on raw string paths from VisiWin variable definitions.

#### Features

- Central **definition class** for PLC variables
- Supports:
  - ✔ Single symbols (`PlcSymbol`)
  - ✔ Array symbols (`PlcArraySymbol`)
- Optional **array bounds validation**
- Seamless usage in:
  - XAML (via `MarkupExtension`)
  - MVVM / adapters

---

#### 🔧 Example: Definition Class

```csharp
namespace HMI.PlcSymbols
{
    public static class Definitions
    {
        public static PlcSymbol MyVariable { get; } = new PlcSymbol("MyVariable");
        public static PlcArraySymbol MyArrayVariable { get; } = new PlcArraySymbol("MyArray", 3);
    }
}
```

---

#### 🔧 Usage
> [!NOTE]
> **Variable Browser Config:** Import arrays as single elements!
>
> <img width="414" height="514" alt="Variable Browser Config" src="https://github.com/user-attachments/assets/c959f98c-7f46-474a-b3d3-97fb72775f36" />


**In code (adapter / MVVM):**

```csharp
await _variableService.SetValueAsync(PlcSymbols.Definitions.MyVariable, "VariableValueFromAdapter");
```

**In XAML:**

```xml
<vw:TextVarIn
    LabelText="VarIn MyVariable"
    VariableName="{plcsym:PlcSymbolPath Symbol={x:Static plc:Definitions.MyVariable}}" />

<vw:TextVarOut
    LabelText="VarOut MyVariable"
    VariableName="{plcsym:PlcSymbolPath Symbol={x:Static plc:Definitions.MyVariable}}" />

<vw:NumericVarIn
    LabelText="VarIn Array[0]"
    VariableName="{plcsym:PlcSymbolPath Symbol={x:Static plc:Definitions.MyArrayVariable}, Index=0}" />
```

---

#### 🎮 Demo Project

**`VisiWin.Toolkit.Demo.PlcSymbol`**

A standalone demo project demonstrating the usage of `PlcSymbol` and `PlcArraySymbol` in a real VisiWin HMI context.

---

## 🤝 Contributing

Contributions are welcome! Feel free to open issues or submit pull requests to help improve the toolkit.

## 📄 License

This project is licensed under the [MIT License](LICENSE).
