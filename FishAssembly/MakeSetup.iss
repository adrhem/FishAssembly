#define MyAppName "FishAssembly"
#define MyAppVersion "1.0"
#define MyAppPublisher "BinCat Solutions©"
#define MyAppExeName "Assembly.exe"

[Setup]
AppId={{73D10464-13FC-4D36-B391-F8D697FC9AE4}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
LicenseFile=C:\Users\cima\Desktop\Ensamblador\FishAssembly\Acuerdo de Licencia de FishAssembly.rtf
OutputDir=C:\Users\cima\Desktop\Ensamblador
OutputBaseFilename=setup_fishAssembly
SetupIconFile=C:\Users\cima\Desktop\Ensamblador\FishAssembly\source\fish.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "C:\Users\cima\Desktop\Ensamblador\FishAssembly\bin\Debug\Assembly.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\cima\Desktop\Ensamblador\FishAssembly\bin\Debug\TABOP.txt"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

