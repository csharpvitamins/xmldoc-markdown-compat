# XML Markdown Compatibility

A small executable that **removes the leading white space** from Visual Studios XML Documentation, to make the content compatible with white-space significant formats e.g. Markdown.

This tool was built to solve issues when consuming Visual Studio produced comments as Markdown in tools like [Swagger](http://swagger.io/) (via [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle)) and [ASP.NET Web Api Help Page](https://www.nuget.org/packages/Microsoft.AspNet.WebApi.HelpPage).


## How it works

The `xmldoc-markdown-compat.exe` can be called via command line, or referenced and used just like any other .NET DLL. 

When executing, it loads an XML file and iterates over well-known elements, detecting if the text content of the element has leading white-space and removes it.

The consistency of leading white space is important. Mixing tabs and spaces may prevent the a the white space pattern being found. 


### Command line
	
	xmldoc-markdown-compat.exe "path\to\a\file.xml" [/replace] [/output:"path\to\a\new.xml"]

Examples:
	
 1. overwrite file.xml with a new version
    
        xmldoc-markdown-compat.exe file.xml /replace

 2. save changes to new.xml
	
        xmldoc-markdown-compat.exe file.xml /output:new.xml
	
 3. output the changes to console
	
        xmldoc-markdown-compat.exe file.xml
	
	   


## Setting it up

The easiest approach, if you're using Visual Studio, is to set up a post-build event to enumerate the documentation files in your project, performing an in-place conversion on each file.

Available on [NuGet](https://www.nuget.org/packages/CSharpVitamins.XmlDocumentation.Compatibility/). To install, run the following command in the Package Manager Console:

	PM> Install-Package CSharpVitamins.XmlDocumentation.Compatibility -Pre


### A Batch Example

Post-build event command line:

	$(ProjectDir)post-build.bat $(SolutionDir) $(TargetDir)


Add a `post-build.bat` to your *project*

	echo off
	
	set SOLUTION_ROOT=%1
	set TRANSFORM="%SOLUTION_ROOT%packages\CSharpVitamins.XmlDocumentation.Compatibility.1.0.0-alpha\tools\xmldoc-markdown-compat.exe"
	set BIN=%2
	
	for /r "%BIN%" %f in (*.xml) do (
		echo xmldoc-markdown-compat %%f
		%TRANSFORM% %%f /replace
	)


### A PowerShell example

Post-build event command line:

	powershell -file $(ProjectDir)post-build.ps1 -solutionRoot $(SolutionDir) -bin $(TargetDir)


Add a `post-build.ps1` to your *project*

	Param (
		[string]$solutionRoot,
		[string]$bin
	)
	
	$transform = Join-Path $solutionRoot 'packages\CSharpVitamins.XmlDocumentation.Compatibility.1.0.0-alpha\tools\xmldoc-markdown-compat.exe'
	
	$files = Get-ChildItem -path $bin -filter *.xml
	foreach ($file in $files) {
		Write-Host "xmldoc-markdown-compat $($file.Name)"
		& $transform $file.FullName /replace
	}

