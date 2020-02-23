#!/bin/bash

# ============================================================================================================== 
 # This file is part of OWL API for .Net.
 # © 2014 Cognitum, Poland. All rights reserved.  
 # 
 # Licensed under DUAL LICENSE: the Apache 2.0 OR GPLv3
 # Choose the license that is compatible with your
 #
 # License 1:
 # Apache License, Version 2.0 (the "License"); you may not use this file except in compliance  
 # with the License. You may obtain a copy of the License at http: #www.apache.org/licenses/LICENSE-2.0 
 # Unless required by applicable law or agreed to in writing, software distributed under the License is  
 # distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
 # See the License for the specific language governing permissions and limitations under the License. 
 #
 # License 2:
 # This program is free software: you can redistribute it and/or modify
 # it under the terms of the GNU General Public License v3 as
 # published by the Free Software Foundation, either version 3 of the
 # License, or (at your option) any later version.
 #
 # This program is distributed in the hope that it will be useful,
 # but WITHOUT ANY WARRANTY; without even the implied warranty of
 # MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 # GNU General Public License v3 for more details.
 #
 # You should have received a copy of the GNU General Public License v3.
 # If not, see <http://www.gnu.org/licenses/gpl-3.0.txt>.
 #
 # ============================================================================================================== 
 #

##
 # @author Alessandro Seganti, a.seganti@cognitum.eu, Cognitum Poland
 #         , Date: 18-Aug-2014
 #
 
#if(( $# > 4 ))
#then
################### CHECK ARGUMENTS

	NBASEARG=0
	OUTPUTFILENAME=""
	RECURSIVE=0
	REFERENCE=""
	NFOLDERS=0
	FOLDERS=""

	while [[ $# > 0 ]]
	do

	key="$1"
	
	case $key in
		-o|--output)
		shift
		OUTPUTFILENAME="$1"
		NBASEARG=$(( $NBASEARG + 1))
		shift
		;;
		-r|--recursive)
		shift
		RECURSIVE="$1"
		NBASEARG=$(( $NBASEARG + 1))
		shift
		;;
		-ref|--reference)
		shift
		REFERENCE="$1"
		NBASEARG=$(( $NBASEARG + 1))
		shift
		;;
		-h|--help)
		echo "USAGE: -o <outputFileName (completePath)> -ref <Reference> -r <Recursive (0 --> yes, 1-->no || DEFAULT = 0)> <folder with jar files1> <folder2> ...\n !! Folder names with slash !!"
		exit
		;;
		*)
			break	
			# unknown option
		;;
	esac
	done
	
	while [[ $# > 0 ]]
	do
		FOLDERS=$FOLDERS" $1"
		shift
	done
		
	if [ -z $FOLDERS ]; then
		echo "Give at least one folder."
		exit
	fi	
	
	if [ -z $OUTPUTFILENAME ]; then
		echo "OutputfileName is required!"
		exit
	fi
	
#	OUTPUTFILENAME=$1
#   REFERENCE=$2
#	RECURSIVE=$3
#	NFOLDERS=$4

	#NBASEARG=4
	echo "OUTPUTFILENAME:"$OUTPUTFILENAME" :"$REFERENCE" BASEARG:"$NBASEARG" NFOLDERS:"$NFOLDERS" RECURSIVE:"$RECURSIVE
	
	if(( $RECURSIVE != 0 && $RECURSIVE != 1 ))
	then
		echo "Expecting for RECURSIVE 0 or 1"
		exit
	fi
	
#################### LOOP THROUGH DIRECTORIES AND SEARCH FOR JAR FILES.
	libListTot=""
	NlibsTot=0
	for i in $FOLDERS
	do
		NFOLDERS=$(( $NFOLDERS + 1 ))
		LIBDIR=$i
		# FIND THE NUMBER OF JARS PRESENT IN THE FOLDER
		if(( $RECURSIVE == 1 ))
		then
			Nlibs=$(ls $LIBDIR*.jar | wc -l) # search on the current directory
		elif (( $RECURSIVE == 0 ))
		then
			Nlibs=$(find $LIBDIR -iname '*.jar' | wc -l) # search from the current directory to all subdirectories
		fi

		echo -e "***\n Found "$Nlibs" libs in "$LIBDIR"\n****\n"

		if(( $Nlibs > 0 ))  # IF FOUND SOME JARS, RETRIEVE THEIR NAMES
		then
			if(( $RECURSIVE == 1 ))
			then
				libList=$(ls "$LIBDIR"*.jar | awk '{printf("%s ",$1);}END{printf("\n");}' | sed 's|/|\\|g' )
			elif (( $RECURSIVE == 0 ))
			then
				libList=$(find $LIBDIR -iname '*.jar' | awk '{printf("%s ",$1);}END{printf("\n");}' | sed 's|/|\\|g' )
			fi
			
			libListTot=$libListTot$libList
			NlibsTot=$(( $NlibsTot + $Nlibs ))
		else
			echo "###########################"
			echo "No libs found in "$LIBDIR"... exiting."
			echo "###########################"
			exit
		fi	
	done

	
	if(( $NFOLDERS < 1 ))
	then
		echo "Give at least one folder where to search."
		exit
	fi
############ IF FOUND MORE THAN 100 JARS, PROBABLY THE COMMAND TO GIVE TO IKVM WONT FIT IN THE COMMAND LINE, WE HAVE TO MERGE SOME JARS
	if(( $NlibsTot > 100 ))
	then
		n=0
		k=1
		#JAR_CMD="/C/Program Files/Java/jdk1.7.0_45/bin/jar"
		JAR_CMD=$JAVA_HOME
		JAR_CMD=$(echo $JAR_CMD | sed 's|C:|/C|' | sed 's|\\|/|')
		JAR_CMD=$JAR_CMD"\bin\jar"
		
		### CREATE A TEMPORARY FOLDER WHERE TO STORE THE MERGED JARS
		baseDir=$(pwd)
		baseDir=$baseDir"/"
		baseJarDir="/D/"
		tmpJar="JAVA_TMP"
		tmpJarSub="JAR_CONTENT"
		tmpNewLibList=""
		if [ -d $baseJarDir$tmpJar ]; then
			echo "removing old "$baseJarDir$tmpJar    ### REMOVE THE OLD FOLDER IF ALREADY PRESENT
			rm -r $baseJarDir$tmpJar
		fi
		mkdir $baseJarDir$tmpJar && cd $baseJarDir$tmpJar
		mkdir $tmpJarSub && cd $tmpJarSub
		
		##### LOOP THROUGH THE JARS AND MERGE mergeSize jars together --> the final number of jars will be NlibsTot/(mergeSize+1)
		mergeSize=3
		totJarCombined=0
		for i in $libListTot
		do
			n=$(( $n + 1 ))
			fileName=$( echo $i | sed 's|\\|/|g' )
			echo "extracting jar "$baseDir$fileName"...."
			"$JAR_CMD" -xf $baseDir$fileName   # extract jar file inside tmpJarSub
			if (( $n > $mergeSize ))  ## arrived at merge size --> combine
			then
				JAR_NAME="combined_"$k".jar"
				echo "Combining JARS "
				"$JAR_CMD" -cvf "../"$JAR_NAME * 1>tmp.txt
				rm -r *   # remove all jars already extracted
				tmpNewLibList=$tmpNewLibList$baseJarDir$tmpJar"/"$JAR_NAME" "
				echo "Combined "$n" jar files in "$JAR_NAME
				totFileCombined=$(( $totFileCombined + $n ))
				n=0
				k=$(( $k + 1 ))
				
				tmpNewLibList=$(echo $tmpNewLibList | sed 's|/D|D:|g' | sed 's|/|\\|g')
				tmpNewLibList=$tmpNewLibList" "
			fi
		done

		NjarsLeft=$(ls * | wc -l)
		## IF THERE ARE STILL SOME JAR TO COMBINE.. DO IT NOW
		if(( $NjarsLeft > 0 ))
		then
			JAR_NAME="combined_"$k".jar"
			"$JAR_CMD" -cvf "../"$JAR_NAME * 1>tmp.txt
			tmpNewLibList=$tmpNewLibList$baseJarDir$tmpJar"/"$JAR_NAME
			tmpNewLibList=$(echo $tmpNewLibList | sed 's|/D|D:|g' | sed 's|/|\\|g')
			tmpNewLibList=$tmpNewLibList" "
			echo "Combined "$n" jar files in "$JAR_NAME" at the end"
		fi
		
		cd $baseDir
		libListTot=$tmpNewLibList
		echo "##################################"
		awk 'BEGIN{print "Combined ",'$NlibsTot'," in ",'$NlibsTot'/('$mergeSize'+1)," files.";}'
		echo "##################################"
		
		libListTot=$(echo $libListTot | sed 's|/D|D:|g' | sed 's|/|\\|g')
		libListTot=$libListTot" "
		echo $libListTot
	fi
	
	if [ ! -z "$REFERENCE" ]; then
			libListTot="-r:"$REFERENCE" "$libListTot 
			echo "Added the reference "$REFERENCE" to the IKVM command"
	fi
	
	echo -e "\n*****\nFound a total of "$NlibsTot" jar files in "$NFOLDERS" folders\n*****\n"
	
######################### WRITE THE COMMAND FOR IKVM AND RUN IT	
	DIRN=$(dirname $OUTPUTFILENAME)
	DIRNWIN=$(dirname $OUTPUTFILENAME | sed 's|/|\\|g')
	echo "excuting "$DIRNWIN"\makeDLL.cmd"
	OUTPNAME=$( echo $OUTPUTFILENAME | sed 's|/|\\|g' )
	echo "IKVM\bin\ikvmc.exe -target:library -out:"$OUTPNAME" "$libListTot >$DIRN"/makeDLL.cmd"
	echo "Running IKVM\bin\ikvmc.exe"
	cmd.exe /c $DIRNWIN"\makeDLL.cmd >tmp.txt 2>tmp_err.txt"
	

	echo "###########################"
	echo "GENERATED OUTPUT IN "$OUTPNAME" you probably want to copy it in the base directory."
	echo "###########################"
#else
#	echo "usage:<outputFileName (completePath)> <Reference (can be empty)> <Recursive (0 --> yes, 1-->no)> <Nfolders> <folder with jar files1> <folder2> ...\n !! Folder names with slash !!"
#fi