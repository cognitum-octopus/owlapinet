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
using org.semanticweb.owlapi.model;
using org.semanticweb.owlapi.reasoner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HermiT.Net
{

    public class NetReasonerFactoryImpl : Cognitum.OwlApi.Net.NetReasonerFactory
    {
        public NetReasonerFactoryImpl()
        {
            //Assembly assReasoner = Assembly.LoadFrom(@"Reasoners\HermiT\hermit.dll");
        }

        private ReasonerProgressMonitor progrMonitor = null;
        public void SetProgrMonitor(ReasonerProgressMonitor progrMonitorExt){
            progrMonitor = progrMonitorExt; 
        }

        public org.semanticweb.owlapi.reasoner.OWLReasoner createNonBufferingReasoner(OWLOntology ontology)
        {
            var config = new org.semanticweb.HermiT.Configuration();
            config.reasonerProgressMonitor = progrMonitor;
            config.throwInconsistentOntologyException = false;

            return new org.semanticweb.HermiT.Reasoner(config, ontology);
        }

        public org.semanticweb.owlapi.reasoner.OWLReasoner createNonBufferingReasoner(OWLOntology ontology, OWLReasonerConfiguration config)
        {
            org.semanticweb.HermiT.Configuration configuration = new org.semanticweb.HermiT.Configuration();
            configuration.reasonerProgressMonitor = config.getProgressMonitor();
            configuration.throwInconsistentOntologyException = false;

            return new org.semanticweb.HermiT.Reasoner(configuration, ontology);
        }

        public org.semanticweb.owlapi.reasoner.OWLReasoner createReasoner(OWLOntology ontology)
        {
            var config = new org.semanticweb.HermiT.Configuration();
            config.reasonerProgressMonitor = progrMonitor;
            config.throwInconsistentOntologyException = false;

            return new org.semanticweb.HermiT.Reasoner(config, ontology);
        }

        public org.semanticweb.owlapi.reasoner.OWLReasoner createReasoner(OWLOntology ontology, org.semanticweb.owlapi.reasoner.OWLReasonerConfiguration config)
        {
            org.semanticweb.HermiT.Configuration configuration = new org.semanticweb.HermiT.Configuration();
            configuration.reasonerProgressMonitor = config.getProgressMonitor();
            configuration.throwInconsistentOntologyException = false;

            return new org.semanticweb.HermiT.Reasoner(configuration, ontology);
        }

        public string getReasonerName()
        {
            OWLOntologyManager owlMan = org.semanticweb.owlapi.apibinding.OWLManager.createOWLOntologyManager();
            OWLOntology ont = owlMan.createOntology();
            OWLReasoner reas = this.createReasoner(ont);
            return reas.getReasonerName();
        }

        public string getReasonerDescription()
        {
            return @"HermiT is reasoner for ontologies written using the Web Ontology Language (OWL). 
Given an OWL file, HermiT can determine whether or not the ontology is consistent, identify subsumption relationships between classes, and much more.
HermiT is the first publicly-available OWL reasoner based on a novel “hypertableau” calculus which provides much more efficient reasoning than any previously-known algorithm. 
Ontologies which previously required minutes or hours to classify can often by classified in seconds by HermiT, 
and HermiT is the first reasoner able to classify a number of ontologies which had previously proven too complex for any available system to handle.
HermiT uses direct semantics and passes all OWL 2 conformance tests for direct semantics reasoners.";
        }

        public string getReasonerVersion()
        {
            OWLOntologyManager owlMan = org.semanticweb.owlapi.apibinding.OWLManager.createOWLOntologyManager();
            OWLOntology ont = owlMan.createOntology();
            OWLReasoner reas = this.createReasoner(ont);
            org.semanticweb.owlapi.util.Version ver = reas.getReasonerVersion();
            return ver.getMajor()+"."+ver.getMinor()+"."+ver.getPatch();
        }
    }
}
