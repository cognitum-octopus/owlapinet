# owlapinet
An open source project developed by Cognitum that port the java OWLAPI to .Net.

**Project Description**

OWL API for .Net is a project developed by [Cognitum](http://www.cognitum.eu/) the creator of ontology editor [Fluent Editor](http://www.cognitum.eu/semantics/FluentEditor/).

OWL API for .Net is an open source project that port the java [OWLAPI](http://owlapi.sourceforge.net/) in .Net. In this project we include the scripts to compile (through IKVM) the OWLAPI library and the reasoners jar in Windows libraries (dll). Furthermore in this project we have a **console test application**, a .Net version of the Owl reasoner interface and the sample implementation of this interface for [HermiT](http://hermit-reasoner.com/) and [Pellet](http://clarkparsia.com/pellet/).

In the console application, it is possible to test the reasoners and the examples from the [OWLAPI examples](https://github.com/owlcs/owlapi/blob/master/contract/src/test/java/org/coode/owlapi/examples/Examples.java).

If you are interested in adding a new OWL Java reasoner or change the version of the reasoners currently loaded in the project, you have to compile it through IKVM to Windows Libraries. For this purpose we added in the project the **makeDLL.sh **script. This script is a BASH script (yes you can use bash in window, see at the end for further info) which search for all jars in a certain Directory (parameter) and pack it through **IKVM** (see section IKVM below).

Then to 

### OWLAPI:

*   Download the latest version of the OWLApi from [here](http://sourceforge.net/projects/owlapi/files/OWL%20API%20%28for%20OWL%202.0%29/).
*   compile it with the command: makeDLL.sh -o owlapi.dll FOLDER-WITH-OWLAPI-JAR

### Sample 1 - HermiT:

*   Download HermiT [here](http://hermit-reasoner.com/download.html "HermiT")
*   Unzip to HERMIT_FOLDER
*   Copy the **.jar** in HERMIT_FOLDER\project\lib\ (!! do not copy owlapi...jar !!) to HERMIT_LIBS
*   Copy the HERMIT_FOLDER\org.semanticweb.HermiT.jar to HERMIT_LIBS
*   Compile it with the command:   makeDLL.sh -o TestOWLReasoners.Net/Reasoners/HermiT/hermit.dll -ref Reasoners/owlapi.dll HERMIT_LIBS

### Sample 2 - Pellet:

*   Download Pellet [here](http://clarkparsia.com/pellet/download "Pellet")
*   Unzip to PELLET_FOLDER
*   Copy the **.jar and folders **in PELLET_FOLDER\lib to PELLET_LIBS
*   Compile it with the command:   makeDLL.sh -o TestOWLReasoners.Net/Reasoners/Pellet/pellet.dll -ref Reasoners/owlapi.dll PELLET_LIBS

###  IKVM:

*   Download IKVM from [here](http://sourceforge.net/projects/ikvm/files/ikvm/)
*   Copy everything to BASEPROJECTFOLDER/IKVM.

### License considerations

Keep in mind that different reasoners came with different licenses. E.g. HermiT is licensed by its owners under LGPL. Pellet is licensed by its owners under dual license - you may buy it from ClarkParsia or use it in an open-source projects under AGPL license. Other OWLAPI compatible reasoners may have other licenses.

The whole source (not the 3rd party libraries) of this project is is licensed under dual license: Apache 2.0 and GPLv3 - make your own choice that is most suited for your project.

#### Bash for windows:

The simplest way to have bash in windows is by downloading [Git for window](http://git-scm.com/download/win) otherwise you can find it [here](http://www.gnu.org/software/bash/bash.html)
