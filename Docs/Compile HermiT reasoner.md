**Compile HermiT**

* Compile the OWL API as explained in [compile the owl api](compile-the-owl-api)
* Download HermiT [here ](-url_http___hermit-reasoner.com_download.html)
* Unzip to HERMIT_FOLDER
* Copy the **.jar** in HERMIT_FOLDER\project\lib\ **{"(!! do not copy owlapi...jar !!)"}** to HERMIT_LIBS
* Copy the HERMIT_FOLDER\org.semanticweb.HermiT.jar to HERMIT_LIBS
* Compile it with the command:   makeDLL.sh -o TestOWLReasoners.Net/Reasoners/HermiT/hermit.dll -ref Reasoners/owlapi.dll HERMIT_LIBS