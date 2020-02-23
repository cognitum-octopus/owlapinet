/* ============================================================================================================== 
 * This file is part of OWL API for .Net.
 * © 2014 Cognitum, Poland. All rights reserved.  
 * 
 * Licensed under DUAL LICENSE: the Apache 2.0 OR GPLv3
 * Choose the license that is compatible with your
 *
 * License 1:
 * Apache License, Version 2.0 (the "License"); you may not use this file except in compliance  
 * with the License. You may obtain a copy of the License at http: *www.apache.org/licenses/LICENSE-2.0 
 * Unless required by applicable law or agreed to in writing, software distributed under the License is  
 * distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
 * See the License for the specific language governing permissions and limitations under the License. 
 *
 * License 2:
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License v3 as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License v3 for more details.
 *
 * You should have received a copy of the GNU General Public License v3.
 * If not, see <http://www.gnu.org/licenses/gpl-3.0.txt>.
 *
 * ============================================================================================================== 
 */

/**
 * @author Alessandro Seganti, a.seganti@cognitum.eu, Cognitum Poland
 *         , Date: 18-Aug-2014
 */
using org.semanticweb.owlapi.apibinding;
using org.semanticweb.owlapi.model;
using org.semanticweb.owlapi.reasoner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cognitum.OwlApi.Net.Test
{
    class TryOwlReasoner
    {
        static OWLOntologyManager manager = null;
        static IRI ontologyIRI = IRI.create("http://ontorion.com/unknown.owl/#");

        /// <summary>
        /// Searches for reasoners in reasonerDllFolder, instantiate a reasoner and shows the info about this reasoner.
        /// </summary>
        /// <param name="reasonerDllFolder"></param>
        public static void GetOwlInfo(string reasonerDllFolder)
        {
            manager = OWLManager.createOWLOntologyManager();
            OWLOntology ontology = manager.createOntology(ontologyIRI);

            ReasoningService aa = new ReasoningService(reasonerDllFolder);
            OWLReasoner reasoner = aa.reasonerFact.createReasoner(ontology);
            Console.WriteLine(aa.reasonerFact.getReasonerName() + " Version:" + aa.reasonerFact.getReasonerVersion() + "\r\n Description:" + aa.reasonerFact.getReasonerDescription());
            Console.WriteLine("Press enter to go further...");
            Console.ReadLine();
        }


        public static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly asm = null;

            if (args.Name.Contains(".Net"))
            {
                string baseDir = @"Reasoners\";
                asm = Assembly.LoadFrom(Path.Combine(baseDir, args.Name + ".dll"));
            }

            return asm;
        }
    }

    class ProgressMonitor : ReasonerProgressMonitor
    {
        ReasoningService me;
        public ProgressMonitor(ReasoningService me) { this.me = me; }
        public void reasonerTaskBusy()
        {
            me.fireReasonerTaskBusy();
        }
        public void reasonerTaskProgressChanged(int i1, int i2)
        {
            me.fireReasonerTaskProgressChanged(i1, i2);
        }
        public void reasonerTaskStarted(string str)
        {
            me.fireReasonerTaskStarted(str);
        }
        public void reasonerTaskStopped()
        {
            me.fireReasonerTaskStopped();
        }
    }

    class ReasoningService
    {
        public Cognitum.OwlApi.Net.NetReasonerFactory reasonerFact = null;
        /// <summary>
        /// Searches for classes that have a class NetReasonerFactoryImpl and instantiate the reasoner implementation.
        /// </summary>
        /// <param name="basePath"></param>
        public ReasoningService(string basePath)
        {
            foreach (string file in Directory.GetFiles(basePath))
            {
                if (file.EndsWith(".dll"))
                {
                    Assembly ass = Assembly.LoadFrom(file);

                    string classBaseName = Path.GetFileName(file).Replace(".dll", "");
                    Type reasonerImpl = ass.GetType(classBaseName + ".NetReasonerFactoryImpl");
                    if (reasonerImpl != null)
                    {
                        reasonerFact = (Cognitum.OwlApi.Net.NetReasonerFactory)Activator.CreateInstance(reasonerImpl);
                    }
                }
            }
            
            reasonerFact.SetProgrMonitor(new ProgressMonitor(this));
        }
        public void fireReasonerTaskBusy() { }
        public void fireReasonerTaskProgressChanged(int i1, int i2) { }
        public void fireReasonerTaskStarted(string str) { }
        public void fireReasonerTaskStopped() { }
    }
}
