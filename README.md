![.NET Core](https://github.com/enorfelt/MobSwitcher/workflows/.NET%20Core/badge.svg?branch=master)
# Tool for Remote Mob Programming

![mob Logo](logo.svg)

Swift handover for remote mobs using git.
`mob` is a CLI tool written in .net core.
It keeps your master branch clean and creates WIP commits on `mob-session` branch.

## How to use it?

```powershell
# simon begins the mob session as typist
simon> mob start 10
# WORK
# after 10 minutes...
simon> mob next
# carola takes over as the second typist
carola> mob start 10
# WORK
# after 10 minutes...
carola> mob next
simon> mob start 10
# WORK
# After 6 minutes the work is done.
simon> mob done
simon> git commit --message "describe what the mob session was all about"
```

## How does it work?

- `mob start 10` creates branch `mob-session` and pulls from `origin/mob-session`, and creates a ten minute timer
- `mob next` pushes all changes to `origin/mob-session`in a `WIP [ci-skip]` commit
- `mob done` squashes all changes in `mob-session` into staging of `master` and removes `mob-session` and `origin/mob-session`

- `mob status` display the mob session status and all the created WIP commits
- `mob reset` deletes `mob-session` and `origin/mob-session`

## How can one customize it?
You can set several environment variables, such as `MOBSWITCHER_WipBranch` and `MOBSWITCHER_RemoteName`, that will be picked up by `mob`. See [the appsettings.json for an extensive list](https://github.com/enorfelt/MobSwitcher/blob/master/src/MobSwitcher.Cli/appsettings.json). Use `MOBSWITCHER_`as prefix.
You can also set config per repository by `mob config set UsePrettyPrint true`. That will create a `.mob` folder at the root of your repository. The folder will contain the settings file.

Pretty print is enabled by default. It uses the [Nerd Fonts](https://github.com/ryanoasis/nerd-fonts) Hack font face. You need to install it and enable it in your terminal.
If you want to disable pretty print, you need to set MOBSWITCHER_UsePrettyPrint to false.

## How to install on Windows vi Chocolatey

Install [Chocolatey](https://chocolatey.org/) if you don't have so.

```bash
> choco install mobswitcher
# Now, you can use the mob tool from any directory in the terminal
```

To upgrade using Chocolatey, run the following:

```bash
> choco upgrade mobswitcher
# Now, you can use the mob tool from any directory in the terminal
```

## How to contribute

Create a pull request.

## Credits

- Special thanks to the orignial creators and concept by [remotemobprogramming/mob](https://github.com/remotemobprogramming/mob)
