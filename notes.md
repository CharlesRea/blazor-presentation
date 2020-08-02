# Blazor Presentation

## Pre-talk setup

* Restart PC
* Run zoom tool
* Close down Slack, Teams, Discord, Outlook, Skype
* Turn on silent mode in windows
* Open slides, IDE, Chrome
* Close Chrome tabs
* Set resolution
* Set up timer


## Talk overview 

Blazor is Microsoft's brand new framework for building client-side web applications.
So what makes it special, compared to all the other front-end frameworks out there? 
Blazor has a unique feature: you write the code in C#, which runs in the browser using 
WebAssembly - with no Javascript involved! In this talk, we'll see how we can write 
applications using Blazor, and take a look at its syntax and component model. We'll 
explore how Blazor works under the hood, and how WebAssembly is used to run .NET 
code within the browser. We'll discuss the advantages and disadvantages of this 
approach, and get an understanding of when it makes sense to use Blazor.

### Session takeaways
* Knowledge of how to get started creating Blazor applications
* Understand how WebAssembly enables running non-Javascript languages in the browser
* Appreciate when Blazor is a good fit for an application, and when to avoid it

## Rough timings
45 mins total:
* 5 mins intro
* 15 mins demo
* 10 mins - how it works
* 5 mins - wider ecosystem, other rendering modes
* 5 mins - downsides, when to use it
* 5 mins questions

## Talk notes

### Introduction
* Who I am, what I do
* Ghyston


### What is Blazor?
* New framework from Microsoft, for building rich interactive client-side web apps, written in C#
* Couple of ways of running - we're going to mostly focus on WebAssembly
* We're going to see how we can write C# code, and use .NET libraries directly in the browser
* We'll take a look at what its' like to write a Blazor app, and take a look at how it works under the hood


### Let's take a look at it in action
* Basic demo, showing compontent-based UI, basic component state, rendering
* Hello world example: click a button, show an alert / log to console?
* Or counter? Shows state, events, components


### More complicated demo
* Deliveroo clone?
  * Single restaurant
  * List of food options - get from API
  * Add to order button - adds to basket, client side
  * Address form - shows form components
  * Map showing where the driver is - shwoing JS interop


### How does it work?
* What is Web Assembly?
* Mono runtime compiled to WebAssembly
* C# is intepretted, downloads compiled binaries - no difference in compilation vs normal C# (apart from linker)
* Explain Blazor compiles .razor files down to C# classes
* Show debugging demo

### Other rendering modes
Server side - potentially show demo?
Pre-rendering - potentially show demo?
Experimental mobile bindings


### Pros
* Write your code in C#, share across backend and front-end
  * Hard to overstate how awesome this is. Share type defintions, complex validation code, utility methods
* Full power of the .NET ecosystem & standard library
  * runs on ASP.NET framework, so has the same primitives as used there. Can re-use DI, validation, logging, etc.
* Full framework
  * Routing
  * Forms, validation
  * Authentication and authorisation
  * Opinionated, but very easy to plug in replacements
* Run across different hosting models

### Cons
* Performance concerns
  * Bundle size - give specific numbers.
  * Mention the linker.
  * .NET IL code is interpreted at runtime. So not as fast as C# (QQ get number here), and slower than JS. E.g we've hit issues building complex forms, which haven't been a problem in JS.
  * Mention potential improvements coming over time

* It's still very raw and new
  * Only 1.0 in May
  * Solid OSS ecosystem, but nowhere near as large as JS frameworks.
  * e.g. there's a good testing library, but hasn't yet 1.0d and have quickly run into issues using it.

* Development experience is a little rough
  * No hot reload
  * Debugging is harder, compared to traditional C# or just direct JS debugging
  * No devtools - e.g. React devtools to visualise component tree
  * IDE support can be lacking - in Rider

* No IE support, due to WebAssembly

* These are solved by Server-Side blazor, but that has some other downsides - latency is an issue

### When should you use it?
* Concerns around performance, bundle size, etc make it a harder sell IMO for public-facing sites. Going to be a
  big concern on low powered mobile devices over flaky 3G connections.
* If you can rule those out, can be a good fit - i.e. very good for internal line-of-business apps, admin sites, etc.


### Questions?