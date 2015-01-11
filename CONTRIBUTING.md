##CefSharp Project Contribution Guide
Thanks for your interest in contributing to the project! Please follow these simple guidelines:

### General
- **Please read the full contents of [the FAQ](https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions) before submitting an issue, or posting to the Google Group. It's quite likely your question has already been answered before.** If something is unclear in the FAQ, of course feel free to ask; the idea is just to reduce the level of "noise" we have to go through, reading the same questions over and over again.
- If you are unsure if something is a "bug" or a "feature", discuss it with the Google Group first. Don't cross-post: if you create an issue, and all the information is contained there, that's perfectly enough. There's no reason to also post it to the group; it just creates "line noise". The project maintainers are very busy people like you and me, and things will sometimes take a few weeks (or in worst case, more) to answer. If you are in a rush - do your very best to investigate the problem thoroughly; if possible, fix the bug yourself and submit a pull request.
- Before creating a GitHub issue or pull request, try looking through the list & issue archives to make sure the issue at hand hasn't been raised before. [Google](http://www.google.com) can also be helpful: just typing "cefsharp appdomain" for example (when trying to see whether AppDomain support has been discussed before) will often give you helpful results.

#### Guidelines For Creating An Issue

So we're increasingly getting a large number of _'it doesn't work, please tell me how to fix it'_ issues that provide none of the detail required for us to actually help you! A few things to note:

- We do appreciate cultural/languages differences, that being said **never** demand that someone help you, this is not a commercial application with paid support!
- This is a volunteer project, we give of our time freely and we ask for you to do the same. Contributions can be simple like updating/adding new entries in the [FAQ](https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions), creating new pages in the [WIKI](https://github.com/cefsharp/CefSharp/wiki), updating the examples. (Anyone with a `GitHub` account can edit the `WIKI`)
- We have **very** limited active contributors so please spend as much time looking into your own problem as possible, the more you help yourself, the quicker things will get fixed.
- Asking the same questions over and over again is **Wasting our time**, search open/closed issues to see if your issue has already been addressed.
- Having to constantly query users to gather information is **very frustrating**!
- Please don't hijack issues, if your problem is distinct then please create a unique issue (after searching previous issues).
- `CefSharp` simply takes the building blocks provided by `Cef` and attempts to provide a usable '.Net' implementation, the upstream `Cef` forum is a valuable resource, if your issues seems fairly low level, then please conduct some research before posting.
- Ideally each issue is a useful resource for references purposes (Don't take offence if someone edits your description).
- When including code limit to small chunks, large blocks post as gist or similar
- Please no links to binaries, e.g. zip files, either contribute your example as a github project, a gist or another public code sharing service.

#### What detail is required you ask?

And the answer is **lots**. Please include all of the following:

- **What is the expected output? What do you see instead?**
- **What steps will reproduce the problem?**
- **What version of the product are you using? On what operating system? x86 or x64?**
    - What version are you using? Nuget? CI Nuget? build from a branch? If so which branch?
    - Win7, Win 8, etc?
- **Please provide any additional information below.**
    - A stack trace if available, any Exception information.
    - Does the cef log provide any relevant information? (By default there should be a debug.log file in your bin directory)
- Any other background information that's relevant? Are you doing something out of the ordinary? 3rd party controls?

#### Example Bug Report
-- TODO: Insert sample bug report here.

### Coding Style
- **Please** follow existing coding style when submitting pull requests.
- Most importantly, **use spaces** - do not use tabs (which is the default setting for C++ projects in Visual Studio).
- The "preview" function when creating a Pull Request on GitHub is very useful for identifying whitespace issues (and for checking out the impact of your changes) - please use it.
- Max number of columns are 132, please format your code accordingly
- Please don't add #Regions to the code
- When adding new files, please prepend the standard license disclaimer (just copy and paste from another source file)
- Do your best to follow these guidelines but don't be afraid to make mistakes when trying to apply them. We are all novices in the beginning.

### Pull Requests/Feature Branches
- Please limit your changes to a logical grouping, keeping changesets small increases the likelihood they will be merged
- If you then want to make subsequent changes, it's actually best to not do them before the feature is merged. Please wait for feedback/review before progressing. This greatly improves our ability to review your changes and dramatically increases the likelihood they will be merged in a timely fashion.
- If you wish to keep progressing on your work, please maintain a feature branch independent of the branch referenced by your pull request. From your WIP branch you can selectively merge in changes to the PR branch as required.
- In general, it's much better to be "too granular" with PR's that contain "change-the-world"-kind of changes, which usually tend to lag behind a lot longer before getting merged (if they ever will...).
- Small (really, minimalistic) commits. Each individual commit only adds one specific thing. The basic approach to achieving this is to read your commit message. Do you feel the need to add multiple lines? Then you're doing too much at the same time.
