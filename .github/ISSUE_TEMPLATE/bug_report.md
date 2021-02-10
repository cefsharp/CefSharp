---
name: Bug report
about: Create a report for a reproducible bug
title: ''
labels: ''
assignees: ''

---

### Please only open an issue if you have a BUG to report, if you simply have a question or require some assistance keep reading for info. If you do have a BUG to report, please use the Bug Report template below.

So you have a question to ask, where can you look for answers? Read on. Think you've found a bug? Please take the time to fill out the bug report below, provide as much information as you can, make sure you provide information for every heading. Thank you! We'd like to keep issues exclusively for **bug reports**, so please ask your questions in the `Discussions` section (https://github.com/cefsharp/CefSharp/discussions)

- Start by reading the General Usage guide, it answers all the common questions https://github.com/cefsharp/CefSharp/wiki/General-Usage
- Check out the FAQ, lots of useful information there, specially if your having trouble deploying to a different machine : https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions
- GitHub proves a fantastic search feature, it'll search through past issues and code. So check that out (Search box at the top of this page) : https://github.com/cefsharp/CefSharp
- You can see all the `CefSharp` tagged issues on `Stackoverflow`, some useful stuff there : http://stackoverflow.com/questions/tagged/cefsharp
- You can search the `Gitter Chat Channel` for past questions/conversations, you can search through every discussion from the beginning : https://gitter.im/cefsharp/CefSharp

Still have a question? Great, ask it on [Discussions](https://github.com/cefsharp/CefSharp/discussions) or [Stackoverflow](http://stackoverflow.com/questions/tagged/cefsharp). Larger complex questions should be asked on `Stackoverflow`

**Note: CefSharp is just a wrapper around the Chromium Embedded Project, it's worth searching http://magpcss.org/ceforum/index.php if your problem involves a low level Chromium error message**

We ask that you put in a reasonable amount of effort in searching through the resources listed above. The developers have full time jobs, they have lives, families, the time they have available to contribute this project is a precious resource, make sure you use it wisely! Remember the more time we spend answering the same questions over and over again, less time goes into writing code, adding new features, actually fixing bugs! 

Still have a question to ask or unsure where to go next? Start with the Gitter Chat room : https://gitter.im/cefsharp/CefSharp

Before posting a bug report please take the time to read https://codeblog.jonskeet.uk/2012/11/24/stack-overflow-question-checklist/

---
### Bug Report
Delete this line and everything above, and then fill in the details below.

- **What version of the product are you using?**
    - What version are you using? Nuget? CI Nuget? build from a branch? If so please link to the relevant commit.
	- Please include the exact version number you are using e.g. 86.0.241 (no ambiguous statements like `Latest from Nuget`)
    - Please only create an issue if you can reproduce the problem with version 86.0.241 or greater.

- **What architecture x86 or x64?**
    <x86/x64>
    
- **On what operating system?**
    <Win7/Win8.1/Win10>

- **Are you using `WinForms`, `WPF` or `OffScreen`?**
    <WinForms/WPF/OffScreen>
    
- **What steps will reproduce the problem?**
    - Please provide detailed information here, enough for someone else to reproduce your problem. 
    - Does the problem reproduce using the [MinimalExample](https://github.com/cefsharp/CefSharp.MinimalExample)?
    - If code is required to reproduce your problem then please provide one of the following
      - Fork the [MinimalExample](https://github.com/cefsharp/CefSharp.MinimalExample) and push your changes to `GitHub` (this is the preferred option).
      - Use a code sharing service list `Gist` or `Pastebin`
      - Paste your **formatted code as part of this issue** (only do this for small amounts of code and make sure you **format the code so it's readable**)
      - Please no binary attachments (zip, 7z, etc), code needs to be easily reviewed in a web browser.

- **What is the expected output? What do you see instead?**

- **Please provide any additional information below.**
    - A stack trace if available, any Exception information.
      - If you are seeing a crash in `libcef.dll` then please download `libcef.dll.pdb` and place it next to `libcef.dll` to obtain a detailed stack trace, see https://github.com/cefsharp/CefSharp/wiki/Trouble-Shooting#loading-native-symbols-for-easier-diagnosis for details.

    - Does the cef log provide any relevant information? (By default there should be a debug.log file in your bin directory)

    - Any other background information that's relevant? Are you doing something out of the ordinary? 3rd party controls?

- **Does this problem also occur in the `CEF` Sample Application**
    - Download one of the following:
    - For x86 download https://cef-builds.spotifycdn.com/cef_binary_87.1.13%2Bg481a82a%2Bchromium-87.0.4280.141_windows32_client.tar.bz2
    - For x64 download https://cef-builds.spotifycdn.com/cef_binary_87.1.13%2Bg481a82a%2Bchromium-87.0.4280.141_windows64_client.tar.bz2
	- Extract and run cefclient.exe
		- If you are using WPF/OffScreen run
		```
		cefclient.exe --multi-threaded-message-loop --off-screen-rendering-enabled --enable-gpu --no-sandbox --disable-site-isolation-trials
		```
		- If you are using WinForms run
		```
		cefclient.exe --multi-threaded-message-loop --no-sandbox --disable-site-isolation-trials
		```
	- **MAKE SURE TO TEST WITH THE COMMAND LINE ARGS LISTED ABOVE**
    - If you can reproduce the problem with `cefclient` then please report the issue on https://bitbucket.org/chromiumembedded/cef/overview (Make sure you search before opening an issue). If you open an issue here it will most likely be closed as `upstream` as the bug needs to be fixed in `CEF`.
