# biz.dfch.CS.StateMachine
[![License](https://img.shields.io/badge/license-Apache%20License%202.0-blue.svg)](https://github.com/dfensgmbh/biz.dfch.CS.StateMachine/blob/master/LICENSE)


A simple C# based State Machine that can be configured via State Transitions based on an idea by [Juliet](http://stackoverflow.com/users/40516/juliet) "[Simple state machine example in C#?](http://stackoverflow.com/questions/5923767/simple-state-machine-example-in-c)"

You can download this package via [NuGet](http://nuget.org) with [Install-Package biz.dfch.CS.StateMachine](https://www.nuget.org/packages/biz.dfch.CS.StateMachine/).


# DESCRIPTION

The project contains an extendable StateMachine that defines a few simple states and two conditions ("Continue", "Cancel") that can be used to advance (transition) through that state machine.

When instatiating the StateMachine with the default constructor the following states, conditions and transitions will be set up per default

## States

* Running
* InternalErrorState
* Completed
* Cancelled
* Disposed


## Conditions

* Continue
* Cancel


## Transitions

Source state | Condition | Target state
:-----|:-----|:------
Created | Continue | Running
Created | Cancel | InternalErrorState
Running | Continue | Completed
Running | Cancel | Cancelled
Completed | Continue | Disposed
Completed | Cancel | InternalErrorState
Cancelled | Continue | Disposed
Cancelled | Cancel | InternalErrorState
InternalErrorState | Continue | Disposed


## Basic functionalities

1. The [`Continue`](./biz.dfch.CS.StateMachine/StateMachine.cs#L94) condition makes a transition from an arbitrary state to the next state as the "good case"
1. The [`Cancel`](./biz.dfch.CS.StateMachine/StateMachine.cs#L02) condition makes a transition from an arbitrary state to the next state as the "bad case"
1. Furthermore there is the [`GetNext`](./biz.dfch.CS.StateMachine/StateMachine.cs#L306) method to transit to the next state based on a given condition.

There are as well methods for exporting and importing the configuration along with the states:

* [`GetStringRepresentation()`](https://github.com/dfensgmbh/biz.dfch.CS.StateMachine/blob/master/biz.dfch.CS.StateMachine/StateMachine.cs#L345)
* [`SetupStateMachine(String configuration, String currentState = null, String previousState = null)`](https://github.com/dfensgmbh/biz.dfch.CS.StateMachine/blob/master/biz.dfch.CS.StateMachine/StateMachine.cs#L135)

## Release Notes

### 1.2.0 20150926

* adjust namespace
* States and Conditions can now be retrieved by consumer
* added code contracts pre-conditions
* enabled CodeContracts assemlby
