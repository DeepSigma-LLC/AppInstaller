# AppInstaller

A Windows-based installer and updater for desktop applications that keeps a target install directory in sync with a source deployment folder.

This repository contains a **WinUI 3 desktop app**, a **CLI launcher**, shared **installation business logic**, and **unit tests**. It is designed for scenarios where an application is distributed as files in a directory (optionally versioned), and you want a simple installer that can copy, update, and optionally expose CLI binaries on the user's `PATH`.

## Highlights

- Installs or updates an application from a source folder
- Supports **versioned deployment folders** named like `1_0_0_0`
- Can install both a **main app** payload and a **CLI** payload
- Preserves selected files and folders with `AppIgnore.txt`
- Can optionally add the installed CLI directory to the **user PATH**
- Includes a lightweight CLI that can launch the UI and pass install arguments through
- Ships with unit tests for command-discovery behavior

## Repository structure

```text
AppInstaller/
├─ AppInstaller/          # WinUI 3 desktop application
├─ AppInstallerCLI/       # Console entry point / launcher
├─ AppInstallerConfig/    # Config-related project
├─ BusinessLogic/         # Core install/update logic
├─ BusinessLogic.Test/    # xUnit tests
└─ ConsoleLibrary/        # Shared console argument helpers
```

## How it works

At a high level, the installer:

1. Loads application settings from `config.json`
2. Accepts install arguments such as app name, source path, target path, CLI source path, and auto-install mode
3. Detects the latest version folder when the source contains subfolders in `#_#_#_#` format
4. Cleans the destination directory while honoring ignore filters from `AppIgnore.txt`
5. Copies the source files into the target install location
6. Optionally adds the installed CLI directory to the user environment `PATH`

The installer separates the target layout into:

- `Main/` for the primary application
- `CLI/` for command-line tools

These folder names come from configuration and can be changed in `config.json`.

## Tech stack

- **C#**
- **.NET 9**
- **WinUI 3 / Windows App SDK**
- **xUnit**

## Requirements

### Runtime
- Windows
- Git installed and available on `PATH` if you use the repository download helper
- Permission to write to the chosen install target

### Development
- .NET 9 SDK
- Visual Studio 2022 (recommended for the WinUI project)
- Windows 10 SDK / Windows App SDK tooling appropriate for the solution target

## Configuration

Both the UI and CLI projects load settings from a `config.json` file with an `AppSettings` section.

Example:

```json
{
  "AppSettings": {
    "AppName": "App Installer",
    "AppAuthor": "DeepSigma LLC",
    "AppDescription": "A simple application installer for applications.",
    "MainDirectory": "Main",
    "CLIDirectory": "CLI",
    "AutoClose": true
  },
  "Logging": {
    "LogLevel": "Information"
  }
}
```

### Important settings

- `AppName`: display name of the installer
- `MainDirectory`: subfolder used for the main installed app
- `CLIDirectory`: subfolder used for installed CLI binaries
- `AutoClose`: whether the installer should close automatically after completion

## Ignore rules

The installer supports an `AppIgnore.txt` file to protect files and folders during updates.

The sample ignore file includes entries such as:

```text
Config.json
AppIgnore.txt
```

This allows you to preserve local configuration or installer metadata between updates.

Comments in the ignore file are prefixed with `#`.

## Command-line arguments

The UI parses key/value arguments in the form:

```text
app=MyApp
source=C:\Deployments\MyApp
target=C:\Apps
clisource=C:\Deployments\MyAppCLI
auto=true
```

### Supported arguments

- `app`: application name to install
- `source`: source directory for the main app
- `target`: root install location
- `clisource`: source directory for CLI files
- `auto`: whether the app should auto-start installation when possible

### Example

```powershell
AppInstallerUI.exe app=MyApp source=C:\Releases\MyApp target=C:\Apps clisource=C:\Releases\MyAppCLI auto=true
```

## CLI usage

The CLI project acts as a lightweight launcher for the UI and supports a few utility flags.

### Available commands

```bash
AppInstaller.exe --version
AppInstaller.exe --path
AppInstaller.exe
```

Behavior:

- `--version`: prints the current application version
- `--path`: prints the current application directory
- no arguments: launches the UI
- unrecognized install-style arguments that are passed through the console controller can be forwarded to the UI launch flow

## Versioned deployments

If your source directory contains subfolders named like this:

```text
1_0_0_0
1_1_0_0
1_1_2_0
```

the installer selects the **highest version folder** automatically and installs from there.

This makes it well suited for deployment directories that publish each release into its own folder.

## Build and run

### Open the solution

The solution file lives at:

```text
AppInstaller/AppInstaller.sln
```

### Build with Visual Studio

1. Open `AppInstaller.sln`
2. Select a configuration such as `Debug` or `Release`
3. Select a platform such as `x64`
4. Build the solution
5. Run the WinUI app project or the CLI project

### Build with the .NET CLI

From the repository root, build individual projects as needed:

```bash
dotnet build BusinessLogic/BusinessLogic.csproj
dotnet build BusinessLogic.Test/BusinessLogic.Test.csproj
dotnet build AppInstallerCLI/AppInstaller.csproj
dotnet build AppInstaller/AppInstallerUI.csproj
```

> Note: the WinUI project is Windows-specific and is typically easiest to build from Visual Studio on Windows.

## Run tests

```bash
dotnet test BusinessLogic.Test/BusinessLogic.Test.csproj
```

Current tests validate the `WindowsProcess.IsProgramInstalled(...)` behavior against known command availability.

## Installation flow example

A typical deployment pattern looks like this:

```text
C:\Releases\MyApp\
└─ 1_4_0_0\
   ├─ MyApp.exe
   ├─ config.defaults.json
   └─ ...
```

Then install to:

```text
C:\Apps\MyApp\
├─ Main\
└─ CLI\
```

The installer will:

- resolve the latest source version
- validate the target location
- delete old files not protected by ignore rules
- copy the new files
- optionally add the CLI directory to `PATH`

## Notes and caveats

- This project is **Windows-only**
- The installer logic assumes applications are distributed as file directories rather than MSI/EXE package installers
- `GitRepoDownload` shells out to `git clone`, so Git must be installed for that feature to work
- The auto-install path only works when all required arguments are present and the target location is valid
- The test suite is currently small and focused on a narrow behavior slice

## Suggestions for future improvement

- Expand unit and integration test coverage for install/update scenarios
- Add a release pipeline and packaged binaries
- Document the expected `AppInstallerConfig` project role
- Add examples for preserving user settings across updates
- Add logging and error-reporting guidance
- Include screenshots of the WinUI workflow

## License

This project is licensed under the **MIT License**. See [`LICENSE`](LICENSE) for details.
