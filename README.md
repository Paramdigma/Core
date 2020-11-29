# Architectrual Geometry Library for .Net

![Project Status](https://img.shields.io/badge/status-Under%20Development-red.svg)
[![Target Framework](https://img.shields.io/badge/Target%20Framework-.NetStandard2.0-blueviolet.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![License](https://img.shields.io/github/license/Paramdigma/Core.svg)](https://github.com/Paramdigma/Core/blob/master/LICENSE)

![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/paramdigma/core?sort=semver)
![Main language](https://img.shields.io/github/languages/top/Paramdigma/Core.svg)
![Code Size](https://img.shields.io/github/languages/code-size/Paramdigma/Core.svg)

**Paramdigma.Core** is _(or will be)_ an independent and open source library for **_Architectural Geometry_** algorithms developed by @AlanRynne.

The core idea is to create a complete package of 3D entities and functions that could be easily connected to different software solutions, via secondary projects that will act as wrappers for the library.

Currently, we are starting development of:

- [McNeel Rhinoceros/Grasshopper](https://github.com/paramdigma/core.grasshopper)
- [Autodesk Revit/Dynamo](https://github.com/paramdigma/core.dynamo)
- [WebApp with viewer]()

If you are looking for just a geometry library to plug into a project we also provide a NuGet package.
- [Paramdigma.Core NuGet Package (GPR hosted)](https://github.com/Paramdigma/Core/packages/268763)

> Github Package Registry is kind of new... If you don't know how to setup GPR for your projects, follow the first step in [THIS GUIDE](https://help.github.com/en/packages/using-github-packages-with-your-projects-ecosystem/configuring-dotnet-cli-for-use-with-github-packages#authenticating-with-a-personal-access-token). Once that is done, you are good to go! 🚀

## Status

|                 | `master`                                                                                                                                                               | `develop`                                                                                                                                                                   |
| --------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| _CI Status_     | [![Build Status](https://travis-ci.com/Paramdigma/Core.svg?branch=master)](https://travis-ci.com/Paramdigma/Core)                                                      | [![Build Status](https://travis-ci.com/Paramdigma/Core.svg?branch=develop)](https://travis-ci.com/Paramdigma/Core)                                                           |
| _Code Quality_  | [![CodeFactor](https://www.codefactor.io/repository/github/Paramdigma/Core/badge/master)](https://www.codefactor.io/repository/github/Paramdigma/Core/overview/master) | [![CodeFactor](https://www.codefactor.io/repository/github/Paramdigma/Core/badge/develop)](https://www.codefactor.io/repository/github/Paramdigma/Core/overview/develop) |
| _Test Coverage_ | [![codecov](https://codecov.io/gh/Paramdigma/Core/branch/master/graph/badge.svg)](https://codecov.io/gh/Paramdigma/Core/branch/master)                                 | [![codecov](https://codecov.io/gh/Paramdigma/Core/branch/develop/graph/badge.svg)](https://codecov.io/gh/Paramdigma/Core/branch/develop)                                 |

## Usage

> This section will be written very shortly!

## Documentation

You can find the Docfx built documentation for the latest release on the 'master' branch at:

[https://paramdigma.com/Core/](https://paramdigma.com/Core/)

> I'm planning on supporting multiple versions on the docs in the future, for now, only the latest release will be documented.
> 
> For previous releases, you can always build the docs locally with DocFx.

## Contributing

I haven't developed any contributing guidelines, although the `master` branch is _push protected_ and is connected to Travis-CI so all contributions should pass build tests.

If you want to contribute to this library, feel free to fork this repo and create a pull request with any modifications.

The makes heavy use of GitHub Actions automation capabilities to test and deploy the library. Reach out to @AlanRynne for any doubts on this.