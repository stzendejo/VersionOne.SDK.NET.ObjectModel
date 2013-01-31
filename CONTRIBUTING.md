# Contributing to VersionOne SDK.NET ObjectModel Library

 1. [Getting Involved](#getting-involved)
 2. [Reporting Bugs](#reporting-bugs)
 3. [Contributing Code](#contributing-code)
 4. [Improving Documentation](#improving-documentation)

## Getting Involved

We need your help to make the VersionOne SDK.NET ObjectModel Library a useful tool for developing integrations and complementary applications/utilities. While third-party patches are absolutely essential, they are not the only way to get involved. You can help the project by discovering and [reporting bugs](#reporting-bugs), [improving documentation](#improving-documentation), and helping others on the [versionone-dev group](http://groups.google.com/group/versionone-dev/) and [GitHub issues](https://github.com/versionone/VersionOne.SDK.NET.ObjectModel/issues).

## Reporting Bugs

Before reporting a bug on the project's [issues page](https://github.com/versionone/VersionOne.SDK.NET.ObjectModel/issues), first make sure that your issue is caused by the ObjectModel Library, not your application code (e.g. passing incorrect arguments to methods, etc.). Second, search the already reported issues for similar cases, and if it has been reported already, just add any additional details in the comments.

After you made sure that you have found a new Object Model Library bug, here are some tips for creating a helpful report that will make fixing it much easier and quicker:

 * Write a **descriptive, specific title**. Bad: *Problem with filtering*. Good: *Scope.GetThisAndAllChildProjects() always returns an empty list*.
 * Whenever possible, include **Class and Method** info in the description.
 * Create a **simple test case** that demonstrates the bug (e.g. using [NUnit](http://www.nunit.org/)).
 
## Contributing Code

### Considerations for Accepting Patches

Before sending a pull request with a new feature, first check if it has been discussed before already (either on [GitHub issues](https://github.com/CloudMade/Leaflet/issues) or [Leaflet UserVoice](http://leaflet.uservoice.com/)), and then ask yourself two questions:

 1. Are you sure that this new feature is important enough to justify its presense in the Leaflet core? Or will it look better as a plugin in a separate repository?
 2. Is it written in a simple, concise way that does not add bulk to the codebase?

If your feature or API improvement did get merged into master, please consider submitting another pull request with the corresponding [documentation update](#improving-documentation).

### Making Changes to Leaflet Source

If you are not yet familiar with the way GitHub works (forking, pull requests, etc.), be sure to check out the awesome [article about forking](https://help.github.com/articles/fork-a-repo) on the GitHub Help website &mdash; it will get you started quickly.

You should always write each batch of changes (feature, bugfix, etc.) in **its own topic branch**. Please do not commit to the `master` branch, or your unrelated changes will go into the same pull request.

You should also follow the code style and whitespace conventions of the original codebase.

Before commiting your changes, run `jake lint` to catch any JS errors in the code and fix them.

You can build minified Leaflet by running `jake` (it will be built from source in the `dist` folder). But please **do not commit the built files** (`leaflet.js` and `leaflet-src.js`) along with your changes, otherwise there may problems merging the pull request. These files are only commited in the `master` branch of the main Leaflet repository.

### Open Source Licenses and Attribution
* Regardless of whether attribution is required by a dependency, we want to acknowledge the work that we depend up on and make it easy for people to evaluate the legal implications of using this project. Therefore, we require all dependencies be attributed in the ACKNOWLEDGEMENTS.md. This should include the persons or organizations who contributed the libraries, a link to the source code, and a link to the underlying license (even when this project sub-licenses under the modified BSD license).

## Improving Documentation

The code of the live Leaflet website that contains all documentation and examples is located in the `gh-pages` branch and is automatically generated from a set of HTML and Markdown files by [Jekyll](https://github.com/mojombo/jekyll).

The easiest way to make little improvements such as fixing typos without even leaving the browser is by editing one of the files with the online GitHub editor: browse the [gh-pages branch](https://github.com/CloudMade/Leaflet/tree/gh-pages), choose a certain file for editing (e.g. `reference.html` for API reference), click the Edit button, make changes and follow instructions from there. Once it gets merged, the changes will immediately appear on the website.

If you need to make edits in a local repository to see how it looks in the process, do the following:

 1. [Install Ruby](http://www.ruby-lang.org/en/) if do not have it yet.
 2. Run `gem install jekyll`.
 3. Run `jekyll --auto` inside the `Leaflet` folder.
 4. Open the website from the `_site` folder.

Now any file changes will be reflected on the generated pages automatically. After commiting the changes, just send a pull request.

If you need to update documentation according to a new feature that only appeared in the master version (not stable one), you need to make changes to `gh-pages-master` branch instead of `gh-pages`. It will get merged into the latter when released as stable.

## Thank You

Not only are we grateful for any contributions, &mdash; helping Leaflet and its community actually makes you AWESOME. Join [this approved list of awesome people](https://github.com/CloudMade/Leaflet/graphs/contributors) and help us push the limits of what is possible with online maps!
