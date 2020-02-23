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

namespace Pellet.Net
{
    public class NetReasonerFactoryImpl : Cognitum.OwlApi.Net.NetReasonerFactory
    {
        public NetReasonerFactoryImpl()
        {
            //Assembly assReasoner = Assembly.LoadFrom(@"Reasoners\Pellet\pellet.dll");
        }

        private ReasonerProgressMonitor progrMonitor=null;
        public void SetProgrMonitor(ReasonerProgressMonitor progrMonitorExt)
        {
            progrMonitor = progrMonitorExt;
        }

        public org.semanticweb.owlapi.reasoner.OWLReasoner createNonBufferingReasoner(OWLOntology ontology)
        {
            var config = new SimpleConfiguration(progrMonitor);

            return new com.clarkparsia.pellet.owlapiv3.PelletReasoner(ontology, config, BufferingMode.NON_BUFFERING);
        }

        public org.semanticweb.owlapi.reasoner.OWLReasoner createNonBufferingReasoner(OWLOntology ontology, OWLReasonerConfiguration config)
        {
            return new com.clarkparsia.pellet.owlapiv3.PelletReasoner(ontology, config, BufferingMode.NON_BUFFERING);
        }

        public org.semanticweb.owlapi.reasoner.OWLReasoner createReasoner(OWLOntology ontology)
        {
            var config = new SimpleConfiguration(progrMonitor);

            return new com.clarkparsia.pellet.owlapiv3.PelletReasoner(ontology, config, BufferingMode.BUFFERING);
        }

        public org.semanticweb.owlapi.reasoner.OWLReasoner createReasoner(OWLOntology ontology,org.semanticweb.owlapi.reasoner.OWLReasonerConfiguration config)
        {
            return new com.clarkparsia.pellet.owlapiv3.PelletReasoner(ontology, config, BufferingMode.BUFFERING);
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
            return @"Pellet is an OWL 2 reasoner. Pellet provides standard and cutting-edge reasoning services for OWL ontologies.";
        }

        public string getReasonerVersion()
        {
            OWLOntologyManager owlMan = org.semanticweb.owlapi.apibinding.OWLManager.createOWLOntologyManager();
            OWLOntology ont = owlMan.createOntology();
            OWLReasoner reas = this.createReasoner(ont);
            org.semanticweb.owlapi.util.Version ver = reas.getReasonerVersion();
            return ver.getMajor() + "." + ver.getMinor() + "." + ver.getPatch();
        }
    }
}
