Support: https://discord.gg/AQ4sDF6Mkz

# Installation
- Download [MelonLoader](https://github.com/LavaGang/MelonLoader/releases/latest)
  - requires [Microsoft Visual C++ 2015-2019 Redistributable 64 Bit](https://aka.ms/vs/16/release/vc_redist.x64.exe) and [dotnet x64 6.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0#runtime-desktop-6.0.19)
- Install MelonLoader 0.7.2 on Slowly Sliding Ducks Demo
- Put [the mod](https://github.com/luckycdev/SSD_Demo_Fixer/releases/latest) in the newly created Mods/ folder in the Slowly Sliding Ducks Demo game files
- Run the game!

# Features
- Game selector to play solo
- Restores main menu buttons
- Press <code>R</code> to restart
- Press <code>0</code> to return to main menu

# Building
- Download [MelonLoader](https://github.com/LavaGang/MelonLoader/releases/latest) (these 2 steps are to get the required dependencies for building)
  - requires [Microsoft Visual C++ 2015-2019 Redistributable 64 Bit](https://aka.ms/vs/16/release/vc_redist.x64.exe) and [dotnet x64 6.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0#runtime-desktop-6.0.19)
- Install MelonLoader 0.7.2 on Slowly Sliding Ducks Demo (these 2 steps are to get the required dependencies for building)
- Install [dotnet x64 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Install [Visual Studio 2022 Community](https://aka.ms/vs/17/release/vs_community.exe)
- Edit the csproj to change your <code>GameDir</code> if needed (line 11 of <code>SSD_Demo_Fixer.csproj</code>)
- Open <code>SSD_Demo_Fixer.csproj</code> in Visual Studio 2022
- Click Build at the top and click Build Solution
- Your mod file should be located at <code>SSD_Demo_Fixer\bin\Debug\net6.0\win-x64\SSD_Demo_Fixer.dll</code>
