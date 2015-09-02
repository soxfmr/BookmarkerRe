# BookmarkerRe [![Build Status](https://travis-ci.org/soxfmr/BookmarkerRe.svg)](https://travis-ci.org/soxfmr/BookmarkerRe)

### 0x0 What's BookrmarkerRe

**BookmarkerRe** is a set of libraries with an achieving tool that aim to classify the Chrome bookmarks automatically.

### 0x1 How it works

The structure of **BookmarkerRe** show as the figure below:

![](http://i1.tietuku.com/0c9bea3fa87a70a6.png?raw=true)

### 0x2 Build Environment
* Visual Studio Community 2015
* .Net Framework 4.0

This repository has contain the libraries and the user interface with Bootstrap framework. You can build it up from the sources. Notice that the user interface files should be located in MAIN directory which is under the base directory of the application.

### 0x3 Rule File
The rule file is a extreme important role in BookmarkerRe. Rule file is XML-formatted. There are the full of semantic formating of the rule file.

* **catalogs** must be the root element in rule file
* **catalog** name will be a directory name of a series of bookmarks
* The catalog can include the sub directory
* There are three types of rule which can be used in the list : **url**, **simple** and **common**
* The **url** of type will be only match the URL of the bookmark
* The **simple** of type will be only match the title of the bookmark
* The **common** of type will be match both of URL and title of the bookmark

Here is an example of rule file:
```
<?xml version="1.0" encoding="utf-8" ?>
<catalogs>
	<catalog name="Mobile">
		<!-- Sub-Directory -->
		<catalog name="Android">
			<rule>
				<!-- It means the common of type -->
				<item>Android</item>
				<item>XDA</item>
			</rule>
		</catalog>
		<catalog name="Apple">
			<rule>
				<simple>
					<item>iPhone</item>
					<item>iOS</item>
				</simple>
			</rule>
		</catalog>
		<rule>
			<url>
				<item>mobile</item>
			</url>
			<simple>
				<item>GPRS</item>
				<item>GSM</item>
			</simple>
			<item>Symbian</item>
		</rule>
	</catalog>
</catalogs>
```

### 0x3 Bookmark File
You can export the html format file from Chrome via the bookmark management tool.





###Publish Under MIT License
