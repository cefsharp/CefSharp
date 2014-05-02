##CefSharp Project Contribution Guide
Thanks for your interest in contributing to the project! Please follow these simple guidelines:

### General
- **Please read the full contents of [the FAQ](https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions) before submitting an issue, or posting to the Google Group. It's quite likely your question has already been answered before.** If something is unclear in the FAQ, of course feel free to ask; the idea is just to reduce the level of "noise" we have to go through, reading the same questions over and over again.
- If you are unsure if something is a "bug" or a "feature", discuss it with the Google Group first. Don't cross-post: if you create an issue, and all the information is contained there, that's perfectly enough. There's no reason to also post it to the group; it just creates "line noise". The project maintainers are very busy people like you and me, and things will sometimes take a few weeks (or in worst case, more) to answer. If you are in a rush - do your very best to investigate the problem thoroughly; if possible, fix the bug yourself and submit a pull request.
- Before creating a GitHub issue or pull request, try looking through the list & issue archives to make sure the issue at hand hasn't been raised before. [Google](http://www.google.com) can also be helpful: just typing "cefsharp appdomain" for example (when trying to see whether AppDomain support has been discussed before) will often give you helpful results.

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
