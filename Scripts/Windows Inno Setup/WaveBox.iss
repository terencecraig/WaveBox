; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "WaveBox"
#define MyAppVersion "Beta 1"
#define MyAppPublisher "Einstein Times Two Software"
#define MyAppURL "http://waveboxapp.com"
#define MyAppExeName "WaveBoxLauncher.vbs"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{C0E9344E-7B81-48A2-BD9C-2DDE9C2496B1}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DisableDirPage=yes
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputBaseFilename=setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\WaveBoxPostInstall.bat"; Flags: nowait postinstall skipifsilent runhidden; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"

[Files]
Source: "C:\Users\Justin Hill\Desktop\Debug\Bass.Net.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Bass.Net.dll.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Cirrious.MvvmCross.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Cirrious.MvvmCross.Plugins.Sqlite.dll"; DestDir: "{app}"; Flags: ignoreversion  
Source: "C:\Users\Justin Hill\Desktop\Debug\log4net.dll"; DestDir: "{app}"; Flags: ignoreversion           
Source: "C:\Users\Justin Hill\Desktop\Debug\log4net.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Microsoft.AspNet.SignalR.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Microsoft.AspNet.SignalR.Owin.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Microsoft.Owin.Diagnostics.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Microsoft.Owin.Host.HttpListener.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Microsoft.Owin.Hosting.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Mono.Nat.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Mono.Zeroconf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Ninject.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Owin.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\System.Data.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\taglib-sharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\WaveBox.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\WaveBox.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\WaveBoxLauncher.bat"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\Mono.Zeroconf.Providers.Bonjour.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\lib_native\bass.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\lib_native\sqlite3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\WaveBoxLauncher.vbs"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\WaveBoxPostInstall.bat"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Justin Hill\Desktop\Debug\WaveBoxPreUninstall.bat"; DestDir: "{app}"; Flags: ignoreversion

[UninstallRun]
Filename: "{app}\WaveBoxPreUninstall.bat"; Flags: waituntilterminated runhidden