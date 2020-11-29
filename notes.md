# Blazor Presentation

### Introduction
* My name's Charles, I'm a technical lead at Ghyston, a software consultancy based up in Bristol.
* Today I'm going to talk to you about Blazor, and our experiences of using it to build web applications in C#
* We're going to see how to build Blazor apps, how it works under the hood and what it's actually like to build real projects using it and when it's a good fit.

### Set the scene - Web apps without Javascript?
* Traditional building blocks of the web
  * HTML - content
  * CSS - styling
  * JS - interactivity
* Forced to use JS because need a language that runs in the browser
* Some downsides:
  * JS is good, but not the right tool for every job
  * It's not a high performance language - interpreted
  * Means we can't reuse any logic that we might already have in our backend language.
  * eg want to share types across API calls, business logic, validation, etc
* Worked around that by compiling down to JS - eg Typescript, Kotlin, Fable for F#
* WebAssembly gives us a true alternative

### What is Web Assembly?
* Wouldn't it be great if we could do something more like building a native app, and compile our code down to
  some sort of machine / assembly code that runs in the browser?
* We couldn't compile down to native code directly - we don't know what platform the code will run on
* So we need to compile down to some sort of intermediate bytecode

* This is exactly what WebAssembly is
* It's a low level, assembly-like language that can run within the browser
* It's got a real focus on performance - it uses a compact binary format, doesn't need to be parsed and interpreted by the browser, can be close to native speed.
* Similarly to physical assembly languages, there's a corresponding text format
* It's stack based - operations push and pop values on the stack
* We can't access the DOM or browser APIs directly - have to go via JS interop.


### How can we build a full UI based app?
* How do we go from basic function invocations, to creating a UI?
* Need to do all DOM interop via JS, so can't really use Wasm only
* It's going to be too hard to do this directly - need a UI framework

### What is Blazor?
* Blazor is a new framework from Microsoft, for building rich interactive client-side web apps, written in C#
* We're going to see how we can write C# code, and use .NET libraries directly in the browser
* We'll take a look at what its' like to write a Blazor app, and take a look at how it works under the hood

### Demo

### How does it work?
* How do we get C# running on WebAssembly?

* First need to pick a .NET runtime. THe mono team have been working on getting Mono running on Wasm for a while.

* We might think we'll just compile our code directly down to WebAssembly
* This AOT compilation is hard
  * Problems like file size, build speed
  * https://github.com/mono/mono/issues/10222 - Blazor AOT issue
  * https://github.com/dotnet/aspnetcore/issues/5466#issuecomment-639806998 - Blazor AOT issue

* Let's think about how C# works - our code is compiled to an intermediate language, which is JITed to machine code at runtime by the .NET runtime
* So, instead of compiling the C# code directly to Wasm - we can get the runtime working on Wasm
* Then the runtime can execute your normal compiled code
* Your app code is interpretted - no JIT. Does have performacne implications
* Demo in devtools


### Overall tech stack
* Underpinning everything: WebAssembly
* Used to execute Mono runtime
* That executes the Blazor UI framework, which is responsible for actually figuring out how to update the DOM
* Blazor is responsible for transforming the Razor files we write into standard C# classes
* Blazor will monitor for events that cause a state change, and then figure out how the DOM should be updated - essentially uses a virtual DOM

### Other rendering modes
* Blazor can run on the server. Instead of running your .NET code in the browser via WebAssembly, this mode runs it on a normal .NET server, and communicates DOM updates over a SignaR websockets connection. Removes the need for Wasm, but means all your interactivity must go via a server, so latency and scaling concerns
* Does mean you have the option of pre-rendering your code on the server

* Microsoft have indicated that in the long run they may try to take Blazor further than just web apps
* Could potentially run it on desktop, via Electron or a similar solution

* The blazor component model is separate from the actual rendering logic. So we could theoretically render something else under the hood
* So for example, what about rendering native mobile controls? Or using Xamarin? There are some highly experimental demos from the Blazor team out there.


### What's it like to use?
* Overall, it definitely achieves the aims of being able to write UI frameworks using C#
* There's a lot of good about it, it's quite pleasurable to use
* This is one of the best implementations of a non-JS language based web framework I've seen, it gets a lot of things right
* It's easy to pick up, and is a good choice for building interactive applications

* Hard to overstate how good being able to share .NET across backend and frontend is. Can share type defitions, complex validation code, business logic
* Obviously .NET has a great standard library and set of primitives to work with

* Framework has lots of things you wnat out the box
  * Routing
  * Forms, validation
  * Authenticaiton, authorisation
* Framework is opinionated, but it's not too hard to plug in replacements where you want to be different - eg forms.
* Does make it very quick to get started, and it's well documented.
* Hooks into the .NET ecosystem and standard library
  * Same primitives as server-side ASP.NET. Can re-use DI, validation, logging.


### Downsides
* Development experience is a little rough
  * No hot reload - changes require a full recompile. This is pretty painful when you're making small styling tweaks, and is far worse than the equivalent JS frameworks.
  * Debugging is harder, compared to traditional C# or just direct JS debugging
  * No devtools - e.g. React devtools to visualise component tree
  * IDE support can be lacking - eg Rider doesn't have full support

* Performance issues
  * Overall, perforamcne is worse than normal C# code, and worse than other JS UI frameworks
  * The code is interpreted, which means there's no JIT going on like with normal C#
  * DOM manipulations / browser APIs have to go through JS interop calls - which is slower than Wasm
  * E.g. we've seen renders be slow when there are a lot of components
  * E.g we've hit issues building complex forms, which haven't been a problem in JS. Forms by default only update on `onInput`, changing to `onChange` causes issues
  * There are ways around this - e.g. only conditionally re-rerender, virtualise where possible - but you end up doing work you don't need to do with JS frameworks
  * Performance improvements are planned in the upcoming .NET 5.0 release in autumn.
  * The big win would be AOT compilation - this is somethign still being worked on by Mono, isn't likley to come into Blazor in the immediate future.

* Download size
  * You need to download the mono WASM runtime, and any .NET code you're running. This can be upwards of 2MB, even when compressed.
  * Blazor does feature an IL linker to strip out unused code from the final build to keep the build size down. Think something like JS tree shaking, but more powerful as the compiler naturally has more info.
  * Even with this, the sizes are still far larger than we'd be used to for UI frameworks

* Can you escape Javscript / CSS / Webpack?
  * Still need to write JS code for interop - on big projects, it's likely you'll want to do something with a JS library which hasn't been wrapped for Blazor yet
  * You still need to do styling - you're probably going to want a CSS build infrastructure for SASS, auto-prefixing, minifactions, etc
  * So we've found we still end up having a Webpack build - i.e. you now need to know both Webpack and .NET.

* It's still very raw and new
  * Only 1.0 in May
  * Solid OSS ecosystem, but nowhere near as large as JS frameworks.
  * e.g. there's a good testing library, but hasn't yet 1.0d and have quickly run into issues using it.
  * A lot of these problems I've mentioned are planned to be fixed, but it's not going to happen overnight

* These are solved by Server-Side blazor, but that has some other downsides - latency is an issue

### When should you use it?
* If you use it,  you need to be aware it's new and that you're on the forefront. You are going to run into unsolved problems.
* Concerns around performance, bundle size, etc make it a harder sell for large, public-facing sites where the user experience is key. Going to be a
  big concern on low powered mobile devices over flaky 3G connections.
* If you can rule those out, can be a good fit - i.e. very good for internal line-of-business apps, admin sites, etc.
* The framework is opionated. I would say you need to be willing to compromise your designs to do things "the Blazor way". If that's a deal-breaker, avoid it.

* It's an interesting space - look to see improvements over the next few years.

### Where can I learn more?
ASP.NET docs
Blazor university


### Other notes
* 5.0 roadmap: https://github.com/dotnet/aspnetcore/issues/21514, https://visualstudiomagazine.com/articles/2020/05/22/blazor-future.aspx
  * Performance improvments
  * No AOT
  * CSS isolation