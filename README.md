# Florence
1. Introduction
2. Origins
3. The Name
4. Build Instructions
5. Getting Started

## Introduction
Florence is a plotting library for .NET/Mono.  While still in its early stages of development, Florence aims to provide a core API that is full featured, flexible, object-oriented, and GUI Toolkit independent.  Additionally, it will provide utilities to make it easy to work with plots interactively (in a REPL/interactive console). Currently, WinForms, Gtk#, and BitMap backends are implemented.  Gtk# support is new and still a bit buggy. System.Web support is leftover from the NPlot days and is in an unknown state.

## Origins
Florence began as a fork of the NPlot library (http://sourceforge.net/projects/nplot/) and owes a good deal of its functionality to that project and its contributors.  Major contributors to NPlot include:
* Matt Howlett

## The Name
Florence is named in honor of Florence Nightingale, an early prolific user of graphs for practical purposes.

## Build Instructions
1. Have Visual Studio 2010 or later installed.
2. Open Florence.sln
3. Build

## Getting Started
* The best place to start with the object oriented API is the demo, located in demo/csharp/FlorenceDemo.sln. There are WinForms and Gtk# versions of the demo.
* The best place to start with the interactive support is the example, located at examples\SimpleTestApp.sln. There are WinForms and Gtk# versions of example.

