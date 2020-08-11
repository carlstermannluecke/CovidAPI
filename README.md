# CovidAPI
Implementation of the Backend test task

# Task
For a system that collects all infected people, allows updating their state (infected,
recovered, deceased) and allows fetching data, the task was to provide an API that:
- creates new entries,
- updates the state of entries,
- and finds all cases of newly infected from a given timestamp and allows to filter by their
state.

# Content
This repository consists of the code of a Visual Studio Solution, which contains three projects:
- Covid19API1, which is the library I was supposed to implement
- CovidClient1, which is a graphical client using Windows Forms that I have implemented in order to test the library functions
- CovidClientText, which is a non-interactive client that performs some very simple tests on the library

# How to run
The library Covid19API1 has to be compiled first, because the clients depend on it. When it is compiled, the clients have to be referenced to it. In Visual Studio 2017, this works by opening the client project in the solution explorer, right-clicking on "References", clicking on "Browse...", then searching for the file Covid19API1\bin\Debug\netstandard2.0\Covid19API1.dll and importing it as a reference. Make sure that a client project is marked as startup-project, since the library does not have an entry-point where it could launch.

# Design decisions
In this version of the API, it is storing the data in a volatile way. That means, when you start a client, you will always have an empty data base.

Since official statistics on Covid-19 cases contain errors that are later corrected (visible in occasionally falling numbers of total infections), I decided not to include sanity checks for updates. In theory, it should not be possible for an already deceased person to be infected again, or to recover, and to my current knowledge, recovered people can also not get infected again. In practise, only after the death of a person, it might turn out that they have died of non-Covid-related causes, thus making an update from "deceased" to "recovered" necessary. This is why any updates are possible in the API.
